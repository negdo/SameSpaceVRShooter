using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class ButtonAreaReady : ButtonArea
{
    [SerializeField] private NetworkPlayer networkPlayer;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    private float clickDelay = 1f;
    private float lastClickTime = 0.5f;


    private void UpdateText() {
        Debug.Log("UpdateText" + (networkPlayer.state.Value == PlayerState.Ready ? "Cancel" : "Ready"));
        textMeshPro.text = networkPlayer.state.Value == PlayerState.Ready ? "Cancel" : "Ready";
    }

    private void OnEnable() {
        UpdateText();
    }

    private void OnDisable() {
        networkPlayer.state.Value = PlayerState.NotReady;
        UpdateText();
    }

    protected override void OnButtonAction() {
        Debug.Log("Button Pressed");
        if (Time.time - lastClickTime < clickDelay) {
            UpdateText();
            return;
        }
        lastClickTime = Time.time;

        if (networkPlayer.state.Value == PlayerState.NotReady) {
            networkPlayer.SetReady();
        } else if (networkPlayer.state.Value == PlayerState.Ready) {
            networkPlayer.SetNotReady();
        }
        UpdateText();
    }
}
