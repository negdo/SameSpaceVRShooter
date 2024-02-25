using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class NetworkTransformClient : NetworkTransform
{
    private bool forceUpdate = false;
    protected override bool OnIsServerAuthoritative() {
        return false;
    }


    [ServerRpc(RequireOwnership = false)]
    public void forceUpdateServerRpc(Vector3 position, Quaternion rotation) {
        // set dirty bits to force update on clients
        
    }


}
