using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingUI : MonoBehaviour {
    [SerializeField] private Transform followObject;
    [SerializeField] private float smoothFactor = 5;
    [SerializeField] private float yOffset = 0.5f;

    void Update() {
        // move towards the follow object with smooth movement dependent on time
        transform.position = Vector3.Lerp(transform.position, followObject.position, Time.deltaTime * smoothFactor);
        // set y position to 0 to prevent the UI from moving up and down
        transform.position = new Vector3(transform.position.x, yOffset, transform.position.z);
        // set rotation to the follow object rotation smoothly
        transform.rotation = Quaternion.Lerp(transform.rotation, followObject.rotation, Time.deltaTime * smoothFactor);
        // set rotation z to 0 to prevent the UI from rotating
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);



        
    }
}
