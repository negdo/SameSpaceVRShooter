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
            transport.ConnectionData.Address = "0.0.0.0";

            if (SceneLoader.isServer)
            {
                // Dedicated server
                Debug.Log("Starting server");

                // wait until server is started
                NetworkManager.Singleton.StartServer();
            }
            else
            {
                // Host on a player
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
