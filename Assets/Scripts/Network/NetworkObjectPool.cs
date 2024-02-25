using System;
using System.Collections.Generic;

using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;


/// <summary>
/// Object Pool for networked objects, used for controlling how objects are spawned by Netcode. Netcode by default will allocate new memory when spawning new
/// objects. With this Networked Pool, we're using custom spawning to reuse objects.
/// Hooks to NetworkManager's prefab handler to intercept object spawning and do custom actions
/// </summary>
public class NetworkObjectPool : NetworkBehaviour
{
    private static NetworkObjectPool _instance;

    public static NetworkObjectPool Singleton => _instance;

    [SerializeField]
    List<PoolConfigObject> PooledPrefabsList;

    HashSet<GameObject> prefabs = new HashSet<GameObject>();

    Dictionary<GameObject, Queue<NetworkObject>> pooledObjects = new Dictionary<GameObject, Queue<NetworkObject>>();

    private bool m_HasInitialized = false;

    public void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    public override void OnNetworkSpawn() {
        if (IsServer) { InitializePool(); }
        
    }

    public override void OnNetworkDespawn() {
        ClearPool();
    }

    public override void OnDestroy() {
        if (_instance == this) {
            _instance = null;
        }

        base.OnDestroy();
    }


    public void OnValidate() {
        for (var i = 0; i < PooledPrefabsList.Count; i++) {
            var prefab = PooledPrefabsList[i].Prefab;
            if (prefab != null) {
                Assert.IsNotNull(
                    prefab.GetComponent<NetworkObject>(),
                    $"{nameof(NetworkObjectPool)}: Pooled prefab \"{prefab.name}\" at index {i.ToString()} has no {nameof(NetworkObject)} component."
                );
                Assert.IsNotNull(
                    prefab.GetComponent<NetworkPooledObject>(), 
                    $"{nameof(NetworkObjectPool)}: Pooled prefab \"{prefab.name}\" at index {i.ToString()} has no {nameof(NetworkPooledObject)} component."
                );

            }

            var prewarmCount = PooledPrefabsList[i].PrewarmCount;
            if (prewarmCount < 0) {
                Debug.LogWarning($"{nameof(NetworkObjectPool)}: Pooled prefab at index {i.ToString()} has a negative prewarm count! Making it not negative.");
                var thisPooledPrefab = PooledPrefabsList[i];
                thisPooledPrefab.PrewarmCount *= -1;
                PooledPrefabsList[i] = thisPooledPrefab;
            }
        }
    }

    /// <summary>
    /// Gets an instance of the given prefab from the pool. The prefab must be registered to the pool.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public NetworkObject GetNetworkObject(GameObject prefab) {
        return GetNetworkObjectInternal(prefab, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// Gets an instance of the given prefab from the pool. The prefab must be registered to the pool.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position">The position to spawn the object at.</param>
    /// <param name="rotation">The rotation to spawn the object with.</param>
    /// <returns></returns>
    public NetworkObject GetNetworkObject(GameObject prefab, Vector3 position, Quaternion rotation) {
        return GetNetworkObjectInternal(prefab, position, rotation);
    }
    

    /// <summary>
    /// Return an object to the pool (reset objects before returning).
    /// </summary>
    public void ReturnNetworkObject(NetworkObject networkObject, GameObject prefab) {
        var go = networkObject.gameObject;

        // handle physics grabable returning to server
        PhysicsGrabable physicsGrabable = go.GetComponent<PhysicsGrabable>();
        NetworkPooledObject networkObjectPool = go.GetComponent<NetworkPooledObject>();


        if (physicsGrabable != null) {
            physicsGrabable.ReturnOwnershipServerRpcCopy();
        }
        
        if (networkObjectPool != null) {
            networkObjectPool.DisableOnClientRpc(transform.position, transform.rotation);
        }

        go.SetActive(false);

        pooledObjects[prefab].Enqueue(networkObject);
    }


    public void ReturnNetworkObject(NetworkObject networkObject, int NthPrefabInList) {
        var prefab = PooledPrefabsList[NthPrefabInList].Prefab;
        ReturnNetworkObject(networkObject, prefab);
    }


    /// <summary>
    /// Adds a prefab to the list of spawnable prefabs.
    /// </summary>
    /// <param name="prefab">The prefab to add.</param>
    /// <param name="prewarmCount"></param>
    public void AddPrefab(GameObject prefab, int prewarmCount = 0)
    {
        var networkObject = prefab.GetComponent<NetworkObject>();

        Assert.IsNotNull(networkObject, $"{nameof(prefab)} must have {nameof(networkObject)} component.");
        Assert.IsFalse(prefabs.Contains(prefab), $"Prefab {prefab.name} is already registered in the pool.");

        RegisterPrefabInternal(prefab, prewarmCount);
    }


    /// <summary>
    /// Builds up the cache for a prefab.
    /// </summary>
    private void RegisterPrefabInternal(GameObject prefab, int prewarmCount) {
        prefabs.Add(prefab);

        var prefabQueue = new Queue<NetworkObject>();
        pooledObjects[prefab] = prefabQueue;
        for (int i = 0; i < prewarmCount; i++)
        {
            var go = CreateInstance(prefab, Vector3.zero, Quaternion.identity);
            ReturnNetworkObject(go.GetComponent<NetworkObject>(), prefab);
        }

        // Register Netcode Spawn handlers
        NetworkManager.Singleton.PrefabHandler.AddHandler(prefab, new PooledPrefabInstanceHandler(prefab, this));
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private GameObject CreateInstance(GameObject prefab, Vector3 position, Quaternion rotation) {
        GameObject spawnedObject = Instantiate(prefab, position, rotation);
        spawnedObject.GetComponent<NetworkObject>().Spawn();

        return spawnedObject;
    }


    /// <summary>
    /// This matches the signature of <see cref="NetworkSpawnManager.SpawnHandlerDelegate"/>
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private NetworkObject GetNetworkObjectInternal(GameObject prefab, Vector3 position, Quaternion rotation) {
        var queue = pooledObjects[prefab];

        NetworkObject networkObject;
        if (queue.Count > 0) {
            networkObject = queue.Dequeue();
        } else {
            networkObject = CreateInstance(prefab, position, rotation).GetComponent<NetworkObject>();
        }

        // Here we must reverse the logic in ReturnNetworkObject.
        var go = networkObject.gameObject;
        go.SetActive(true);

        NetworkPooledObject networkPooledObject = go.GetComponent<NetworkPooledObject>();
        PhysicsGrabable physicsGrabable = go.GetComponent<PhysicsGrabable>();
    
        if (physicsGrabable != null) {
            physicsGrabable.NetworkPoolEnable();
        }



        if (networkPooledObject != null) {
            networkPooledObject.EnableOnClientRpc(transform.position, transform.rotation);
        }


        go.transform.position = position;
        go.transform.rotation = rotation;


        return networkObject;
    }


    /// <summary>
    /// Registers all objects in <see cref="PooledPrefabsList"/> to the cache.
    /// </summary>
    public void InitializePool() {
        if (m_HasInitialized) return;
        foreach (var configObject in PooledPrefabsList) {
            RegisterPrefabInternal(configObject.Prefab, configObject.PrewarmCount);
        }
        m_HasInitialized = true;
    }


    /// <summary>
    /// Unregisters all objects in <see cref="PooledPrefabsList"/> from the cache.
    /// </summary>
    public void ClearPool() {
        foreach (var prefab in prefabs) {
            // Unregister Netcode Spawn handlers
            NetworkManager.Singleton.PrefabHandler.RemoveHandler(prefab);
        }
        pooledObjects.Clear();
    }





}

[Serializable]
struct PoolConfigObject
{
    public GameObject Prefab;
    public int PrewarmCount;
}

class PooledPrefabInstanceHandler : INetworkPrefabInstanceHandler
{
    GameObject m_Prefab;
    NetworkObjectPool m_Pool;

    public PooledPrefabInstanceHandler(GameObject prefab, NetworkObjectPool pool)
    {
        m_Prefab = prefab;
        m_Pool = pool;
    }

    NetworkObject INetworkPrefabInstanceHandler.Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        var netObject = m_Pool.GetNetworkObject(m_Prefab, position, rotation);
        return netObject;
    }

    void INetworkPrefabInstanceHandler.Destroy(NetworkObject networkObject)
    {
        m_Pool.ReturnNetworkObject(networkObject, m_Prefab);
    }
}
