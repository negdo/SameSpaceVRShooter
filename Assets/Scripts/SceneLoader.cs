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
    public static bool isSpectator = false;
    public static int gameMode = GameMode.TeamDeathmatch;
    public static int testType = 0;

    public static string player_name = "Missing Player Name in Scene Loader";

    public static void LoadMultiplayerSceneHost() {
        Debug.Log("Loading Multiplayer Scene as Host");
        isHost = true;

        CalibrationPositionHolder calibrationPositionHolder = FindObjectOfType<CalibrationPositionHolder>();
        calibrationPositionHolder.updatePlayerPosition();

        SceneManager.LoadScene("MultiplayerScene");
    }

    public static void LoadMultiplayerSceneClient() {
        Debug.Log("Loading Multiplayer Scene as Client");
        isHost = false;

        CalibrationPositionHolder calibrationPositionHolder = FindObjectOfType<CalibrationPositionHolder>();
        calibrationPositionHolder.updatePlayerPosition();

        SceneManager.LoadScene("MultiplayerScene");
    }

    public static void LoadMultiplayerSceneServer() {
        Debug.Log("Loading Multiplayer Scene as Server");
        isHost = true;
        isServer = true;
        SceneManager.LoadScene("MultiplayerScene");
    }


    public static void LoadMainMenu() {
        Debug.Log("Loading Main Menu");

        CalibrationPositionHolder calibrationPositionHolder = FindObjectOfType<CalibrationPositionHolder>();
        if (calibrationPositionHolder != null) {
            calibrationPositionHolder.updatePlayerPosition();
        }

        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadTargetTest() {
        Debug.Log("Loading Target Test");

        SceneManager.LoadScene("TargetTestScene");
    }


}
