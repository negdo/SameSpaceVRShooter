using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ButtonAreaMenu : ButtonArea
{
    protected override void OnButtonAction()
    {
        NetworkManager.Singleton.Shutdown();
        // destroy the network manager
        Destroy(NetworkManager.Singleton.gameObject);
        // destroy calibration position holder
        try {
            Destroy(CalibrationPositionHolder.Instance.gameObject);
        } catch {
            Debug.Log("CalibrationPositionHolder not found");
        }

        SceneManager.LoadScene("MainMenu");
        // SceneManager.UnloadSceneAsync("MultiplayerScene");
    }
}
