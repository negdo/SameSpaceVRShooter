using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ButtonAreaCalibrate : ButtonArea {

    [SerializeField] private GameObject calibrationManager;
    [SerializeField] private TextMeshProUGUI text;
    private bool calibrationEnabled = false;

    protected override void OnButtonActionHand() {
        if (calibrationEnabled) {
            StopCalibration();
        } else {
            StartCalibration();
        }
    }

    public void StartCalibration() {
        calibrationManager.SetActive(true);
        calibrationEnabled = true;
        text.text = "Stop Calibration";
    }

    public void StopCalibration() {
        calibrationManager.SetActive(false);
        calibrationEnabled = false;
        text.text = "Calibrate";
    }




    protected override void OnButtonAction() {}
}
