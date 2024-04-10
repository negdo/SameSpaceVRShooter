using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour {
    // Start is called before the first frame update
    private Camera mainCamera;
    void Start() {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
