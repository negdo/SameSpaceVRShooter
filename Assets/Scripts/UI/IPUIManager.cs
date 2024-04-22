using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class IPUIManager : MonoBehaviour
{
    protected string MyIPAddress = "";

    [SerializeField] protected TMPro.TMP_InputField inputServerIP;

    public void UpdateSceneLoaderIP() {
        SceneLoader.host_ip = inputServerIP.text;
    }
}
