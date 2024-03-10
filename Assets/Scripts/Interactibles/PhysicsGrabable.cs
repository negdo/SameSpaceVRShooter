using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsGrabable : Grabable
{
    [Header("Components")]
    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private Rigidbody rigidbodyComponent;

    [Header("Throw settings")]
    [SerializeField] private float throwSpeedMultiplier = 1.0f;
    [SerializeField] private float frameAverageDivider = 0.2f;

    [SerializeField] private float throwAngularVelocityMultiplier = 2;
    [SerializeField] private bool willDespawn = false;
    [SerializeField] private float despawnTime = 30.0f;
    private float despawnTimer = 30.0f;



    public NetworkVariable<bool> grabEnabled = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> isGrabbed = new NetworkVariable<bool>(false);
    protected bool isGrabbedLocal = false;
    private NetworkVariable<bool> thrownGiveBackToServer = new NetworkVariable<bool>(false);
    private bool thrownGiveBackToServerLocal = false;
    public NetworkVariable<ulong> ownerClientId = new NetworkVariable<ulong>(0);

    private GameObject lastHand;
    private Vector3 GrabbedVelocity = Vector3.zero;
    private Vector3 GrabbedAngularVelocity = Vector3.zero;
    private Vector3 lastPosition;
    private Quaternion lastRotation;


    public override void OnNetworkSpawn() {
        if (IsOwner) {
            rigidbodyComponent.isKinematic = false;
        } else {
            rigidbodyComponent.isKinematic = true;
        }

        despawnTimer = despawnTime;
    }


    public override bool OnGrab(GameObject Hand) {
        if (isGrabbed.Value || !grabEnabled.Value) {
            return false;
        }

        isGrabbedLocal = true;
        thrownGiveBackToServerLocal = false;
        GrabGetOwnershipServerRpc();
        rigidbodyComponent.isKinematic = true;
        lastHand = Hand;
        lastPosition = transform.position;
        lastRotation = transform.rotation;
        GrabbedVelocity = Vector3.zero;
        GrabbedAngularVelocity = Vector3.zero;

        return true;
    }


    public override void OnRelease() {
        if (!isGrabbed.Value && !isGrabbedLocal) {
            return;
        }

        rigidbodyComponent.isKinematic = false;
        rigidbodyComponent.velocity = GrabbedVelocity * throwSpeedMultiplier;
        rigidbodyComponent.angularVelocity = GrabbedAngularVelocity * throwAngularVelocityMultiplier;

        isGrabbedLocal = false;
        thrownGiveBackToServerLocal = true;
        ReleaseServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GrabGetOwnershipServerRpc(ServerRpcParams rpcParams = default) {
        thrownGiveBackToServer.Value = false;
        thrownGiveBackToServerLocal = false;
        // Set ownership to this client
        ownerClientId.Value = rpcParams.Receive.SenderClientId;
        networkObject.ChangeOwnership(rpcParams.Receive.SenderClientId);
        isGrabbed.Value = true;
        isGrabbedLocal = true;
    }


    [ServerRpc]
    private void ReleaseServerRpc() {
        isGrabbed.Value = false;
        isGrabbedLocal = false;
        thrownGiveBackToServer.Value = true;
        thrownGiveBackToServerLocal = true;
    }


    [ServerRpc]
    private void ReturnOwnershipServerRpc() {
        // Set ownership to server
        networkObject.ChangeOwnership(0);
        ownerClientId.Value = 0;
        thrownGiveBackToServer.Value = false;
        thrownGiveBackToServerLocal = false;
        isGrabbed.Value = false;
        isGrabbedLocal = false;
        rigidbodyComponent.isKinematic = false;
    }

    public void ReturnOwnershipServerRpcCopy() {
        // called from NetworkObjectPool not over network
        networkObject.ChangeOwnership(0);
        ownerClientId.Value = 0;
        thrownGiveBackToServer.Value = false;
        thrownGiveBackToServerLocal = false;
        rigidbodyComponent.isKinematic = true;
        isGrabbed.Value = false;
        grabEnabled.Value = true;
    }


    void Update() {
        UpdateGrabbableTransform();

        // despawn object after despawnTime
        if (willDespawn && IsServer) {
            if (isGrabbed.Value) {
                despawnTimer = despawnTime;
            } else {
                despawnTimer -= Time.deltaTime;
                if (despawnTimer <= 0) {
                    despawnTimer = despawnTime;
                    NetworkPooledObject networkPooledObject = gameObject.GetComponent<NetworkPooledObject>();
                    if (networkPooledObject != null) {
                        networkPooledObject.ReturnToPool();
                    }
                }
            }
        }
    }

    private void UpdateGrabbableTransform() {

        if (isGrabbedLocal != isGrabbed.Value) {
            return;

        } else if (isGrabbed.Value && isGrabbedLocal && ownerClientId.Value == NetworkManager.Singleton.LocalClientId) {
            // update position and rotation
            transform.position = lastHand.transform.position;
            transform.rotation = lastHand.transform.rotation;


            // average velocity over last frames to smooth out throwing
            Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
            //Vector3 angularVelocity = (transform.rotation.eulerAngles - lastRotation) / Time.deltaTime;
            Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotation);
            deltaRotation.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);
            Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;

            Vector3 angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.deltaTime);

            GrabbedVelocity = frameAverageDivider * velocity + (1 - frameAverageDivider) * GrabbedVelocity;
            GrabbedAngularVelocity = frameAverageDivider * angularVelocity + (1 - frameAverageDivider) * GrabbedAngularVelocity;

            lastPosition = transform.position;
            lastRotation = transform.rotation;


        }
        else if (!isGrabbed.Value && !isGrabbedLocal && thrownGiveBackToServer.Value && thrownGiveBackToServerLocal && ownerClientId.Value == NetworkManager.Singleton.LocalClientId) {
            // if object is thrown, should be given back to server when it stops moving
            if (rigidbodyComponent.velocity.magnitude < 0.0001f && rigidbodyComponent.angularVelocity.magnitude < 0.0001f) {
                thrownGiveBackToServerLocal = false;
                ReturnOwnershipServerRpc();
            }
        }
    }


    public override void OnLostOwnership() {
        updateOtherClientsKinematic();
    }


    public override void OnGainedOwnership() {
        updateOtherClientsKinematic();
    }


    private void updateOtherClientsKinematic() {
        if (!IsOwner) {
            rigidbodyComponent.isKinematic = true;
        }
    }

    public void NetworkPoolDisable() {
        // called from NetworkPooledObject on server
        rigidbodyComponent.isKinematic = true;
        thrownGiveBackToServerLocal = false;
        isGrabbedLocal = false;
    }

    public void NetworkPoolEnable() {
        // called from NetworkPooledObject on server
        rigidbodyComponent.isKinematic = false;
    }
}
