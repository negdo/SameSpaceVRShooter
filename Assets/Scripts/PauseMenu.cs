using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject CameraObject;
    [SerializeField] InputActionReference SettingsButtonAction;
    [SerializeField] float distance = 0.5f;
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
            Canvas.SetActive(false);
            showMenu = false;
        } else {
            Canvas.SetActive(true);
            showMenu = true;

            Vector3 newPostion = CameraObject.transform.position;
            newPostion += CameraObject.transform.forward * distance;

            transform.position = newPostion;
            transform.rotation = CameraObject.transform.rotation;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
    }
}
