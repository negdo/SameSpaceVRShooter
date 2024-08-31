using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CalibrationTutorialWindow : MonoBehaviour {

    [SerializeField] private GameObject Window;
    [SerializeField] private InputActionReference calibrationAction;

    private void OnEnable() {
        calibrationAction.action.performed += _ => CloseWindow();
    }

    public void CloseWindow() {
        Window.SetActive(false);
        calibrationAction.action.performed -= _ => CloseWindow();
    }




}
