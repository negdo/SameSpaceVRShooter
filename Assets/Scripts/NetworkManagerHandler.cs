using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkManagerHandler : MonoBehaviour
{
    [SerializeField] private UnityTransport transport;

    void Start()
    {
        

        if (SceneLoader.isHost)
        {
            if (SceneLoader.isServer)
            {
                // Dedicated server
                transport.ConnectionData.Address = "0.0.0.0";
                Debug.Log("Starting server");
                NetworkManager.Singleton.StartServer();
            }
            else
            {
                // Host on a player
                transport.ConnectionData.Address = "0.0.0.0";
                Debug.Log("Starting host");
                NetworkManager.Singleton.StartHost();
            }
        }
        else
        {
            // Client
            transport.ConnectionData.Address = SceneLoader.host_ip;
            Debug.Log("Starting client");
            NetworkManager.Singleton.StartClient();
        }
    }
}
