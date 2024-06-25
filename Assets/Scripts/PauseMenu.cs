using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    [SerializeField] ButtonAreaCalibrate buttonAreaCalibrate;
    [SerializeField] InputActionReference SettingsButtonAction;
    [SerializeField] GameObject leftRay;
    [SerializeField] GameObject rightRay;
    private bool showMenu = false;


    private void Start() {
        Canvas.SetActive(false);
        showMenu = false;
    }

    private void Awake() {
        SettingsButtonAction.action.performed += _ => OnSettingsButton();
    }

    private void OnDestroy() {
        SettingsButtonAction.action.performed -= _ => OnSettingsButton();
    }

    public void OnSettingsButton() {
        if (showMenu) {
            buttonAreaCalibrate.StopCalibration();
            Canvas.SetActive(false);
            showMenu = false;
            leftRay.SetActive(false);
            rightRay.SetActive(false);
        } else {
            Canvas.SetActive(true);
            showMenu = true;
            leftRay.SetActive(true);
            rightRay.SetActive(true);
        }
    }


    public void OnResumeButton() {
        OnSettingsButton();
    }

    public void OnCalibrateButton() {
        buttonAreaCalibrate.OnButtonClick();
    }

    public void OnMenuButton() {
        Debug.Log("Menu button pressed");
        
        // destroy the network manager
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);

        SceneLoader.LoadMainMenu();
    }
}
