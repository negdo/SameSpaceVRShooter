using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditorInternal;
using UnityEngine;

public class PhysicsGrabable : Grabable
{
    [Header("Components")]
    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private Rigidbody rigidbodyComponent;

    [Header("Throw settings")]
    [SerializeField] private float throwSpeedMultiplier = 5.0f;
    [SerializeField] private int frameAverageDivider = 10;



    public NetworkVariable<bool> grabEnabled = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> isGrabbed = new NetworkVariable<bool>(false);
    private NetworkVariable<bool> thrownGiveBackToServer = new NetworkVariable<bool>(false);
    public NetworkVariable<ulong> ownerId = new NetworkVariable<ulong>(0);

    private GameObject lastHand;
    private Vector3 GrabbedVelocity = Vector3.zero;
    private Vector3 GrabbedAngularVelocity = Vector3.zero;
    private Vector3 lastPosition;
    private Vector3 lastRotation;


    public override bool OnGrab(GameObject Hand)
    {
        if (isGrabbed.Value || !grabEnabled.Value)
        {
            return false;
        }

        GrabGetOwnershipServerRpc();
        rigidbodyComponent.isKinematic = true;
        lastHand = Hand;
        lastPosition = transform.position;
        lastRotation = transform.rotation.eulerAngles;
        GrabbedVelocity = Vector3.zero;
        GrabbedAngularVelocity = Vector3.zero;

        return true;
    }


    public override void OnRelease()
    {
        if (!isGrabbed.Value)
        {
            return;
        }

        rigidbodyComponent.isKinematic = false;
        rigidbodyComponent.velocity = GrabbedVelocity * throwSpeedMultiplier;
        rigidbodyComponent.angularVelocity = GrabbedAngularVelocity;

        print("Release");
        print("velocity: " + rigidbodyComponent.velocity + "   angularVelocity: " + rigidbodyComponent.angularVelocity);

        ReleaseServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GrabGetOwnershipServerRpc(ServerRpcParams rpcParams = default)
    {
        isGrabbed.Value = true;
        thrownGiveBackToServer.Value = false;
        // Set ownership to this client
        ownerId.Value = rpcParams.Receive.SenderClientId;
        networkObject.ChangeOwnership(rpcParams.Receive.SenderClientId);
    }

    [ServerRpc]
    private void ReleaseServerRpc(ServerRpcParams rpcParams = default)
    {
        isGrabbed.Value = false;
        thrownGiveBackToServer.Value = true;
    }

    [ServerRpc]
    private void ReturnOwnershipServerRpc(ServerRpcParams rpcParams = default)
    {
        // Set ownership to server
        networkObject.ChangeOwnership(0);
        ownerId.Value = 0;
        thrownGiveBackToServer.Value = false;
        rigidbodyComponent.isKinematic = true;
        rigidbodyComponent.isKinematic = false;
        rigidbodyComponent.velocity = Vector3.zero;
    }



    void Update()
    {
        UpdateGrabbableTransform();
    }

    private void UpdateGrabbableTransform()
    {
        // if object is grabbed by local player
        if (isGrabbed.Value && ownerId.Value == NetworkManager.Singleton.LocalClientId)
        {
            // update position and rotation
            transform.position = lastHand.transform.position;
            transform.rotation = lastHand.transform.rotation;


            // average velocity over last frames to smooth out throwing
            Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
            Vector3 angularVelocity = (transform.rotation.eulerAngles - lastRotation) / Time.deltaTime;

            GrabbedVelocity = (GrabbedVelocity + velocity) / frameAverageDivider;
            GrabbedAngularVelocity = (GrabbedAngularVelocity + angularVelocity) / frameAverageDivider;
            
            lastPosition = transform.position;
            lastRotation = transform.rotation.eulerAngles;

        } else if (thrownGiveBackToServer.Value && ownerId.Value == NetworkManager.Singleton.LocalClientId)
        {
            // if object is thrown, should be given back to server when it stops moving
            if (rigidbodyComponent.velocity.magnitude < 0.01f && rigidbodyComponent.angularVelocity.magnitude < 0.01f)
            {
                print("Return ownership");
                ReturnOwnershipServerRpc();
            }
        }
    }
}
