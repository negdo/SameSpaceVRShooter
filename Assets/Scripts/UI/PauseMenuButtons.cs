using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons : MonoBehaviour {
    [SerializeField] private PauseMenu pauseMenu;

    public void OnResumeButton() {
        Debug.Log("Resume button pressed");
        pauseMenu.OnResumeButton();
    }

    public void OnCalibrateButton() {
        Debug.Log("Calibrate button pressed");
        pauseMenu.OnCalibrateButton();
    }

    public void OnMenuButton() {
        Debug.Log("Menu button pressed");
        pauseMenu.OnMenuButton();
    }
}
