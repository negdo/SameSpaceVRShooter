using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalibrationPositionHolder : MonoBehaviour
{
    public static CalibrationPositionHolder Instance;

    private Vector3 playerPositionCalibration = new Vector3(0, 0, 0);
    private Quaternion playerRotationCalibration = new Quaternion(0, 0, 0, 0);

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MultiplayerScene")
        {
            GameObject NewPlayer = GameObject.Find("XR Origin (XR Rig)");
            if (NewPlayer != null)
            {
                NewPlayer.transform.position = playerPositionCalibration;
                NewPlayer.transform.rotation = playerRotationCalibration;
                Debug.Log("Calibration Position Loaded");
            }
        }
    }

    public void updatePlayerPosition()
    {
        GameObject OldPlayer = GameObject.Find("XR Origin (XR Rig)");
        if (OldPlayer != null)
        {
            playerPositionCalibration = OldPlayer.transform.position;
            playerRotationCalibration = OldPlayer.transform.rotation;
            Debug.Log("Calibration Position Updated");
        }
    }




}
