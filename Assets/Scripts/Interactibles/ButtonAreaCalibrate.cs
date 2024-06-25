using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ButtonAreaCalibrate : MonoBehaviour {

    [SerializeField] private GameObject calibrationManager;
    [SerializeField] private TextMeshProUGUI text;
    private bool calibrationEnabled = false;

    public void OnButtonClick() {
        if (calibrationEnabled) {
            StopCalibration();
        } else {
            StartCalibration();
        }
    }

    public void StartCalibration() {
        Debug.Log("Calibration started");
        calibrationManager.SetActive(true);
        calibrationEnabled = true;
        text.text = "Stop Calibration";
        text.fontSize = 20;
    }

    public void StopCalibration() {
        Debug.Log("Calibration stopped");
        calibrationManager.SetActive(false);
        calibrationEnabled = false;
        text.text = "Calibrate";
        text.fontSize = 30;
    }
}
