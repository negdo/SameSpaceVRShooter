using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplayCounter : MonoBehaviour
{
    private NetworkPlayer networkPlayer;
    private TextMeshPro displayText;
    // Start is called before the first frame update
    void Start()
    {
        networkPlayer = GetComponentInParent<NetworkPlayer>();
        displayText = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = networkPlayer.GetHealth().ToString();
    }
}
