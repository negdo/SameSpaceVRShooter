using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

// class for objects that are pooled by the NetworkObjectPool
// includes basic functionality for syncing the object's state on clients when getting and returning objects
public class NetworkPooledObject : NetworkBehaviour
{
    [SerializeField] private int selfPrefabPoolNumber;
    IPooledObject OtherComponent;


    void Start() {
        OtherComponent = GetComponent<IPooledObject>();

        if (!IsServer) {
            gameObject.SetActive(false);
            CheckEnableOnServerClientServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void CheckEnableOnServerClientServerRpc() {
        // called on client to check if object is enabled on server when initialized
        if (gameObject.activeSelf) {
            EnableOnClientRpc(transform.position, transform.rotation);

        } else {
            DisableOnClientRpc(transform.position, transform.rotation);
        }
    }



    public void OnEnable() {
        if (IsServer) {
            if (OtherComponent == null) {
                OtherComponent = GetComponent<IPooledObject>();
            }

            // reset object state when object is reused
            OtherComponent.ResetState();
        }
    }


    void Update() {
        if (IsServer) {
            // update object state on server
            OtherComponent.UpdateState();
        }
    }


    public void ReturnToPool() {
        if (IsServer) {
            NetworkObjectPool.Singleton.ReturnNetworkObject(gameObject.GetComponent<NetworkObject>(), selfPrefabPoolNumber);
        }
    }


    [ClientRpc]
    public void EnableOnClientRpc(Vector3 position, Quaternion rotation) {
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;

        // force sync position
        NetworkTransformClient networkTransformClient = GetComponent<NetworkTransformClient>();
        if (networkTransformClient != null) {
            
        }
    }


    [ClientRpc]
    public void DisableOnClientRpc(Vector3 position, Quaternion rotation) {
        transform.position = position;
        transform.rotation = rotation;

        PhysicsGrabable physicsGrabable = GetComponent<PhysicsGrabable>();
        if (physicsGrabable != null) {
            physicsGrabable.NetworkPoolDisable();
        }


        gameObject.SetActive(false);
    }
}

public interface IPooledObject {
    void ResetState();
    void UpdateState();
}
