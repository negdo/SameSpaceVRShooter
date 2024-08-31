using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CalibrationTutorialWindowPosition : WindowFloater {

    private void Start() {
        cameraTransform = Camera.main.transform;
        
        if (JustLoaded.justLoaded) {
            JustLoaded.justLoaded = false;
        } else {
            gameObject.SetActive(false);
        }
    }
}

public static class JustLoaded {
    public static bool justLoaded = true;
}
