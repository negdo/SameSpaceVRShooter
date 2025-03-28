using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour {
    [SerializeField] private GameObject nameScreen;
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject JoinScreen;
    [SerializeField] private GameObject HostScreen;
    [SerializeField] private GameObject TestScreen;

// text mesh pro ui input
    [SerializeField] private TMPro.TMP_InputField nameInput;

    public void OnJoinButtonClicked() {
        mainScreen.SetActive(false);
        JoinScreen.SetActive(true);
    }

    public void OnHostButtonClicked() {
        mainScreen.SetActive(false);
        HostScreen.SetActive(true);
    }

    public void OnBackButtonClicked() {
        mainScreen.SetActive(true);
        JoinScreen.SetActive(false);
        HostScreen.SetActive(false);
    }

    public void OnSpectateButtonClicked() {
        mainScreen.SetActive(false);
        JoinScreen.SetActive(true);
        SceneLoader.isSpectator = true;
    }

    public void OnTestButtonClicked() {
        mainScreen.SetActive(false);
        TestScreen.SetActive(true);
    }

    public void OnNameButtonClicked() {
        SceneLoader.player_name = nameInput.text;
        Debug.Log("Player name set to: " + SceneLoader.player_name);
        nameScreen.SetActive(false);
        mainScreen.SetActive(true);
    }
}
