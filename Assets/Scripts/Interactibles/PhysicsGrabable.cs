using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PhysicsGrabable : Grabable
{
    public NetworkVariable<bool> grabEnabled = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> isGrabbed = new NetworkVariable<bool>(false);
    public NetworkVariable<ulong> ownerId = new NetworkVariable<ulong>(0);

    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private Rigidbody rigidbodyComponent;

    private GameObject lastHand = null;
    private Vector3 GrabbedVelocity = Vector3.zero;


    public override bool OnGrab(GameObject Hand)
    {
        if (isGrabbed.Value || !grabEnabled.Value)
        {
            return false;
        }

        GetOwnershipServerRpc();
        isGrabbed.Value = true;
        rigidbodyComponent.isKinematic = true;

        return true;
    }


    public override void OnRelease()
    {
        if (!isGrabbed.Value)
        {
            return;
        }

        isGrabbed.Value = false;
        rigidbodyComponent.isKinematic = false;
        rigidbodyComponent.velocity = GrabbedVelocity;
        ReturnOwnershipServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GetOwnershipServerRpc(ServerRpcParams rpcParams = default)
    {
        // Set ownership to this client
        ownerId.Value = rpcParams.Receive.SenderClientId;
        networkObject.ChangeOwnership(rpcParams.Receive.SenderClientId);
    }

    [ServerRpc]
    private void ReturnOwnershipServerRpc(ServerRpcParams rpcParams = default)
    {
        // Set ownership to server
        ownerId.Value = 0;
        networkObject.RemoveOwnership();
    }



    void Update()
    {
        UpdateGrabbableTransform();
    }

    private void UpdateGrabbableTransform()
    {
        // if object is grabbed by local player, update transform to match hand
        if (isGrabbed.Value && ownerId.Value == NetworkManager.Singleton.LocalClientId)
        {
            transform.position = lastHand.transform.position;
            transform.rotation = lastHand.transform.rotation;

            GrabbedVelocity = (GrabbedVelocity + rigidbodyComponent.velocity) / 10;
        }
    }
}
