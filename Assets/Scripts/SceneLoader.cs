using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public static string host_ip = "";
    public static bool isHost = false;

    private Vector3 playerPositionCalibration = new Vector3(0, 0, 0);
    private Quaternion playerRotationCalibration = new Quaternion(0, 0, 0, 0);

    public void LoadMultiplayerSceneHost()
    {
        Debug.Log("Loading Multiplayer Scene as Host");
        isHost = true;

        CalibrationPositionHolder calibrationPositionHolder = FindObjectOfType<CalibrationPositionHolder>();
        calibrationPositionHolder.updatePlayerPosition();

        SceneManager.LoadScene("MultiplayerScene");
    }

    public void LoadMultiplayerSceneClient()
    {
        Debug.Log("Loading Multiplayer Scene as Client");
        isHost = false;

        CalibrationPositionHolder calibrationPositionHolder = FindObjectOfType<CalibrationPositionHolder>();
        calibrationPositionHolder.updatePlayerPosition();

        SceneManager.LoadScene("MultiplayerScene");
    }
}
