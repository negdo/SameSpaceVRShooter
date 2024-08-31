using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowFloater : MonoBehaviour {

    [SerializeField] protected Transform cameraTransform;
    private Transform GetTransform() {
        Transform newTransform = new GameObject().transform;
        newTransform.position = cameraTransform.position + cameraTransform.forward * 1.5f;
        newTransform.rotation = Quaternion.LookRotation(newTransform.position - cameraTransform.position);

        newTransform.position = new Vector3(newTransform.position.x, cameraTransform.position.y, newTransform.position.z);
        newTransform.rotation = Quaternion.Euler(0, newTransform.rotation.eulerAngles.y, 0);

        return newTransform;
    }

    private void Update() {
        // Transform newTransform = GetTransform();

        Vector3 newPosition = cameraTransform.position + cameraTransform.forward * 1.5f;
        Quaternion newRotation = Quaternion.LookRotation(newPosition - cameraTransform.position);
        newPosition = new Vector3(newPosition.x, cameraTransform.position.y, newPosition.z);
        newRotation = Quaternion.Euler(0, newRotation.eulerAngles.y, 0);

        float distance = Vector3.Distance(gameObject.transform.position, newPosition);
        float angle = Quaternion.Angle(gameObject.transform.rotation, newRotation);

        if (distance > 0.5f || angle > 30.0f) {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, newPosition, 0.01f);
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, newRotation, 0.01f);
        }
    }
}
