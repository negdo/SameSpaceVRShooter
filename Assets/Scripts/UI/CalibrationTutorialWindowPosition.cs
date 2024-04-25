using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CalibrationTutorialWindowPosition : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;


    private Transform GetTransform() {
        Transform newTransform = new GameObject().transform;
        newTransform.position = cameraTransform.position + cameraTransform.forward * 1.5f;
        newTransform.rotation = Quaternion.LookRotation(newTransform.position - cameraTransform.position);

        newTransform.position = new Vector3(newTransform.position.x, cameraTransform.position.y, newTransform.position.z);
        newTransform.rotation = Quaternion.Euler(0, newTransform.rotation.eulerAngles.y, 0);

        return newTransform;
    }

    private void Start() {
        cameraTransform = Camera.main.transform;
        
        if (JustLoaded.justLoaded) {
            JustLoaded.justLoaded = false;
        } else {
            gameObject.SetActive(false);
        }
    }

    private void Update() {
        Transform newTransform = GetTransform();
        float distance = Vector3.Distance(gameObject.transform.position, newTransform.position);
        float angle = Quaternion.Angle(gameObject.transform.rotation, newTransform.rotation);

        if (distance > 0.5f || angle > 30.0f) {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newTransform.position, 0.01f);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newTransform.rotation, 0.01f);
        }
    }
}

public static class JustLoaded {
    public static bool justLoaded = true;
}
