using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using Unity.VisualScripting;
using TMPro;

public class AccuracyTest : MonoBehaviour {
    [SerializeField] private InputActionReference addMeasurementAction;
    [SerializeField] private InputActionReference resetMeasurementsAction;
    [SerializeField] private InputActionReference saveAction;
    // TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI text;
    // audio source
    [SerializeField] private AudioSource beepSound;
    [SerializeField] private AudioSource resetSound;


    private MeasurementDataList measurements = new MeasurementDataList();

    private void OnEnable() {
        addMeasurementAction.action.performed += AddMeasurement;
        saveAction.action.performed += Save;
        resetMeasurementsAction.action.performed += ResetMeasurements;
        measurements.Measurements = new List<MeasurementData>();
    }

    private void OnDisable() {
        addMeasurementAction.action.performed -= AddMeasurement;
        saveAction.action.performed -= Save;
    }

    private void AddMeasurement(InputAction.CallbackContext context) {
        var measurement = new MeasurementData {
            Position = transform.position,
            Rotation = transform.eulerAngles,
            time = Time.time
        };

        Debug.Log(measurement.Position);
        Debug.Log(measurement.Rotation);
        Debug.Log(measurements.ToString());
        measurements.Measurements.Add(measurement);
        Debug.Log("Added measurement: " + measurement.Position + " " + measurement.Rotation);
        text.text = measurements.Measurements.ToArray().Length.ToString() + "\n" + 
                    measurement.Position + "\n" + 
                    measurement.Rotation;

        // send beep sound
        beepSound.Play();
    }

    private void Save(InputAction.CallbackContext context) {
        string time_data = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string file_name = "measurements_" + time_data + ".json";
        string filePath = Path.Combine(Application.persistentDataPath, file_name);
        string json = JsonUtility.ToJson(new MeasurementDataList { Measurements = measurements.Measurements }, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Saved to " + filePath);
        beepSound.Play();
    }

    private void ResetMeasurements(InputAction.CallbackContext context) {
        measurements.Measurements.Clear();
        text.text = "0";


        resetSound.Play();
    }
}




[System.Serializable]
public class MeasurementData {
    public Vector3 Position;
    public Vector3 Rotation;
    public float time;
}

[System.Serializable]
public class MeasurementDataList {
    public List<MeasurementData> Measurements;
}