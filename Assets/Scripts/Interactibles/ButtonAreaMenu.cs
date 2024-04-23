using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ButtonAreaMenu : ButtonArea
{
    protected override void OnButtonAction() {}

    protected override void OnButtonActionHand() {
        Debug.Log("Menu button pressed");
        
        // destroy the network manager
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);

        SceneLoader.LoadMainMenu();
    }
}
