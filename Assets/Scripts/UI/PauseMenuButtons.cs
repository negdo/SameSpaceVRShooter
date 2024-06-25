using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtons : MonoBehaviour {
    [SerializeField] private PauseMenu pauseMenu;

    public void OnResumeButton() {
        pauseMenu.OnResumeButton();
    }

    public void OnCalibrateButton() {
        pauseMenu.OnCalibrateButton();
    }

    public void OnMenuButton() {
        pauseMenu.OnMenuButton();
    }
}
