using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerHandler : MonoBehaviour
{
    [SerializeField] private UnityTransport transport;
    [SerializeField] private TextMeshProUGUI hintstext;

    void Start() {
        if (SceneLoader.isHost) {
            transport.ConnectionData.Address = "0.0.0.0";

            if (SceneLoader.isServer) {
                // Dedicated server
                Debug.Log("Starting server");

                // wait until server is started
                NetworkManager.Singleton.StartServer();
            }
            else {
                // Host on a player
                Debug.Log("Starting host");
                NetworkManager.Singleton.StartHost();
            }
        }
        else {
            // Client
            transport.ConnectionData.Address = SceneLoader.host_ip;
            Debug.Log("Starting client");
            NetworkManager.Singleton.StartClient();

            // check if client is connected after 3 seconds
            StartCoroutine(CheckIfConnected());
        }
    }

    private IEnumerator CheckIfConnected() {
        yield return new WaitForSeconds(2);
        if (!NetworkManager.Singleton.IsConnectedClient) {
            Debug.Log("Failed to connect to server");
            hintstext.text = "Going back to main menu";

            // after 3 seconds go back to main menu
            yield return new WaitForSeconds(1);

            // destroy the network manager
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);

            SceneLoader.LoadMainMenu();
        }
    }
}
