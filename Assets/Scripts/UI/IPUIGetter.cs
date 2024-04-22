using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Net.NetworkInformation;

public class IPUIGetter : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI displayMyIPTextMesh;

    void OnEnable() {

        displayMyIPTextMesh.text = GetLocalIPAddress();
    }


    private string GetLocalIPAddress() {
        string all_interfaces_ip = "";

        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces()) {
            foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses) {
                all_interfaces_ip += ip.ToString() + "\n";

                // if ip address is of shape xxxx.xxxx.xxxx.xxxx regex
                if (System.Text.RegularExpressions.Regex.IsMatch(ip.Address.ToString(), @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$")) {
                    // if ip does not start with 127
                    if (!ip.Address.ToString().StartsWith("127")) {
                        if (ip.Address.ToString().StartsWith("192")) {
                            Debug.Log("My IP Address: " + ip.Address.ToString());
                            return ip.Address.ToString();
                        }
                    }
                }
            }
        }
        return "No Internet Connection";
        // return all_interfaces_ip;
        // throw new Exception("No network adapters with an IPv4 address in the system!");
    }

}
