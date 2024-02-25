using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandPresencePhysics : MonoBehaviour {
    [SerializeField] Transform target;
    private Rigidbody rigidbodyComponent;

    private void Start() {
        rigidbodyComponent = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        rigidbodyComponent.velocity = (target.position - transform.position) / Time.fixedDeltaTime;
        // set rotation
        rigidbodyComponent.MoveRotation(target.rotation);
        
        /*Quaternion rotationDiff = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDiff.ToAngleAxis(out float angle, out Vector3 axis);
        Vector3 rotationDiffInDegrees = angle * axis;
        rigidbodyComponent.angularVelocity = rotationDiffInDegrees * Mathf.Deg2Rad / Time.fixedDeltaTime;*/
        
        /* 
        Quaternion rotationDiff = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDiff.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180) {
            angle -= 360;
        }
        angle = angle * Mathf.Deg2Rad; // Convert angle to radians
        Vector3 rotationDiffInRadians = angle * axis;
        rigidbodyComponent.angularVelocity = rotationDiffInRadians / Time.fixedDeltaTime;
        */
    }
}
