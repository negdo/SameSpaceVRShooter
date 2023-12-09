using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class IPUIManager : MonoBehaviour
{
    protected string MyIPAddress = "";

    [SerializeField] protected TMPro.TextMeshProUGUI displayMyIPTextMesh;
    [SerializeField] protected TMPro.TMP_InputField inputServerIP;

    void Start()
    {
        MyIPAddress = GetLocalIPAddress();
        displayMyIPTextMesh.text = "Your IP Address:\n" + MyIPAddress;
    }

    private string GetLocalIPAddress()
    {
        string all_interfaces_ip = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
            {
                all_interfaces_ip += ip.ToString() + "\n";
                Debug.Log(ip.Address.ToString());

                // if ip address is of shape xxxx.xxxx.xxxx.xxxx regex
                if (System.Text.RegularExpressions.Regex.IsMatch(ip.Address.ToString(), @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"))
                {
                    // if ip does not start with 127
                    if (!ip.Address.ToString().StartsWith("127"))
                    {
                        if (ip.Address.ToString().StartsWith("192"))
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return all_interfaces_ip;
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public void UpdateSceneLoaderIP()
    {
        SceneLoader.host_ip = "192.168.1." + inputServerIP.text;
    }
}
