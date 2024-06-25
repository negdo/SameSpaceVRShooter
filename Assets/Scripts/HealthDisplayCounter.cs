using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplayCounter : MonoBehaviour
{
    private NetworkPlayer networkPlayer;
    private TextMeshPro displayText;

    void Start() {
        networkPlayer = GetComponentInParent<NetworkPlayer>();
        displayText = GetComponent<TextMeshPro>();
    }

    void Update() {
        displayText.text = Math.Round(networkPlayer.GetHealth()).ToString();
    }
}
