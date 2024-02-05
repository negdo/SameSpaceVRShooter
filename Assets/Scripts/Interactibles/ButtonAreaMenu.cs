using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ButtonAreaMenu : ButtonArea
{
    protected override void OnButtonAction()
    {
        // turn off the network manager
        NetworkManager.Singleton.Shutdown();

        // Switch scene to "MainMenu"
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
