using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// class for objects that are pooled by the NetworkObjectPool
// includes basic functionality for syncing the object's state on clients when getting and returning objects
public abstract class NetworkPooledObject : NetworkBehaviour
{
    [SerializeField] private int selfPrefabPoolNumber;

    void Start() {
        if (!IsServer) {
            // hide all pooled objects on clients when client connects
            gameObject.SetActive(false);
            Debug.Log("NetworkPooledObject Start IsClient");
        }
    }

    public void OnEnable() {
        if (IsServer) {
            // reset object state when object is reused
            ResetState();
        }
    }

    void Update() {
        if (IsServer) {
            // update object state on server
            UpdateState();
        }
    }

    protected void ReturnToPool() {
        Debug.Log("NetworkPooledObject ReturnToPool");
        if (IsServer) {
            NetworkObjectPool.Singleton.ReturnNetworkObject(gameObject.GetComponent<NetworkObject>(), selfPrefabPoolNumber);
        }
    }

    // reset the object's state when it is reused
    protected abstract void ResetState();

    // update the object's state on the server
    protected abstract void UpdateState();

    [ClientRpc]
    public void EnableOnClientRpc() {
        gameObject.SetActive(true);
    }

    [ClientRpc]
    public void DisableOnClientRpc() {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
    }
}
