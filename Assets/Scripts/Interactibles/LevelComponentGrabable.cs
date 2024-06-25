using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class LevelComponentGrabable : Grabable
{
    [Header("Components")]
    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private bool EnableDelete = true;


    public NetworkVariable<bool> grabEnabled = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> isGrabbed = new NetworkVariable<bool>(false);
    protected bool isGrabbedLocal = false;
    public NetworkVariable<ulong> ownerClientId = new NetworkVariable<ulong>(0);

    private GameObject lastHand;
    private Vector3 relativeHandPosition;
    private float relativeHandRotationY;


    private Vector3 originalPosition = new Vector3();
    private Quaternion originalRotation = new Quaternion();

    private Vector3 originalHandPosition = new Vector3();
    private Quaternion originalHandRotation = new Quaternion();



    public override bool OnGrab(GameObject Hand) {
        if (isGrabbed.Value || !grabEnabled.Value) {
            return false;
        }

        isGrabbedLocal = true;
        GrabGetOwnershipServerRpc();
        lastHand = Hand;
        relativeHandPosition = transform.position - lastHand.transform.position;
        relativeHandRotationY = transform.rotation.eulerAngles.y - lastHand.transform.rotation.eulerAngles.y;

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalHandPosition = lastHand.transform.position;
        originalHandRotation = lastHand.transform.rotation;


        // disable collider
        GetComponent<Collider>().enabled = false;
        
        return true;
    }

    
    public override void OnRelease() {
        if (!isGrabbed.Value && !isGrabbedLocal) {
            return;
        }

        isGrabbedLocal = false;
        // enable collider
        GetComponent<Collider>().enabled = true;

        ReturnOwnershipServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    private void GrabGetOwnershipServerRpc(ServerRpcParams rpcParams = default) {
        // Set ownership to this client
        ownerClientId.Value = rpcParams.Receive.SenderClientId;
        networkObject.ChangeOwnership(rpcParams.Receive.SenderClientId);
        isGrabbed.Value = true;
        isGrabbedLocal = true;
    }


    [ServerRpc (RequireOwnership = false)]
    private void ReturnOwnershipServerRpc(ServerRpcParams rpcParams = default) {
        // Set ownership to server
        isGrabbed.Value = false;
        isGrabbedLocal = false;
        networkObject.ChangeOwnership(0);
        ownerClientId.Value = 0;
    }


    void Update() {
        UpdateGrabbableTransform();
    }


    private void UpdateGrabbableTransform()
    {
        if (isGrabbedLocal != isGrabbed.Value) {
            // don't doo anything
            Debug.Log("Grabbed local not the same" + isGrabbedLocal + " " + isGrabbed.Value);
        }
        else if (isGrabbed.Value && isGrabbedLocal && ownerClientId.Value == NetworkManager.Singleton.LocalClientId) {
            // update position and rotation
            //Vector3 newPostion = lastHand.transform.position + relativeHandPosition;
            //newPostion.y = transform.position.y;
            //transform.position = new Vector3(newPostion.x, transform.position.y, newPostion.z);

            //Vector3 newRotation = new Vector3(0, lastHand.transform.rotation.eulerAngles.y + relativeHandRotationY, 0);
            //transform.rotation = Quaternion.Euler(newRotation);


            // make new transform in zero
            
            gameObject.transform.position = originalPosition;
            gameObject.transform.rotation = originalRotation;

            // difference in rotation
            float diff = lastHand.transform.rotation.eulerAngles.y - originalHandRotation.eulerAngles.y;

            // rotate around original hand position
            gameObject.transform.RotateAround(originalHandPosition, Vector3.up, diff);

            // move to hand
            gameObject.transform.Translate(lastHand.transform.position - originalHandPosition, Space.World);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
        }
    }

    public void SetGrabEnabled() {
        // called on server GameOperator
        grabEnabled.Value = true;
        isGrabbed.Value = false;
        isGrabbedLocal = false;
    }

    public void SetGrabDisabled() {
        // called on server GameOperator
        grabEnabled.Value = false;
        isGrabbed.Value = false;
        isGrabbedLocal = false;

        if (isGrabbed.Value) {
            ReturnOwnershipServerRpc();
        }
    }

    public override void OnSecondaryAction() {
        if (!grabEnabled.Value || !isGrabbed.Value || !isGrabbedLocal || ownerClientId.Value != NetworkManager.Singleton.LocalClientId) {
            return; // check if isEnabled in parent class
        }

        if (EnableDelete) {
            DeleteObjectServerRpc();
        }
    }

    [ServerRpc]
    private void DeleteObjectServerRpc() {
        // Set ownership to server
        networkObject.ChangeOwnership(0);
        ownerClientId.Value = 0;
        isGrabbed.Value = false;
        isGrabbedLocal = false;
        networkObject.Despawn(true);
    }


}
