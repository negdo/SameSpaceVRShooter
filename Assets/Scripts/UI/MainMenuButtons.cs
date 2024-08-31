using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour {
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject JoinScreen;
    [SerializeField] private GameObject HostScreen;

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
}
