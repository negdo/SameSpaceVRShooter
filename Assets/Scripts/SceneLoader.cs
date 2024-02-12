using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public static string host_ip = "";
    public static bool isHost = true;
    public static bool isServer = false;

    private Vector3 playerPositionCalibration = new Vector3(0, 0, 0);
    private Quaternion playerRotationCalibration = new Quaternion(0, 0, 0, 0);

    public static void LoadMultiplayerSceneHost()
    {
        Debug.Log("Loading Multiplayer Scene as Host");
        isHost = true;

        CalibrationPositionHolder calibrationPositionHolder = FindObjectOfType<CalibrationPositionHolder>();
        calibrationPositionHolder.updatePlayerPosition();

        SceneManager.LoadScene("MultiplayerScene");
    }

    public static void LoadMultiplayerSceneClient()
    {
        Debug.Log("Loading Multiplayer Scene as Client");
        isHost = false;

        CalibrationPositionHolder calibrationPositionHolder = FindObjectOfType<CalibrationPositionHolder>();
        calibrationPositionHolder.updatePlayerPosition();

        SceneManager.LoadScene("MultiplayerScene");
    }

    public static void LoadMultiplayerSceneServer()
    {
        Debug.Log("Loading Multiplayer Scene as Server");
        isHost = true;
        isServer = true;
        SceneManager.LoadScene("MultiplayerScene");
    }
}
