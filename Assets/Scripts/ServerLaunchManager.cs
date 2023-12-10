using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class ServerLaunchManager : MonoBehaviour
{
    [SerializeField] bool isServer = false;

    void Start()
    {
        if (isServer)
        {
            SceneLoader.LoadMultiplayerSceneHost();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
    }
}
