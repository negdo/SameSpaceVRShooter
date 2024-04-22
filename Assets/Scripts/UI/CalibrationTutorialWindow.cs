using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class CalibrationTutorialWindow : MonoBehaviour {

    [SerializeField] private GameObject Window;

    public void CloseWindow() {
        Window.SetActive(false);
    }


}
