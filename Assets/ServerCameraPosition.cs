using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerCameraPosition : NetworkBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    public override void OnNetworkSpawn()
    {
        if (IsServer && !NetworkManager.Singleton.IsHost)
        {
            playerCamera.SetPositionAndRotation(position, Quaternion.Euler(rotation));
        }
    }
}
