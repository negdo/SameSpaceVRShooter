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
        rigidbodyComponent.MoveRotation(target.rotation * Quaternion.Euler(45, 0, 0));
    }
}
