using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentSpawnerPositionSetter : MonoBehaviour
{
    [SerializeField] private GameObject SpawnPointObject;
    float maxDistance = 5f;
    public bool IsAvaliable = true;

    void Update() {
        // check if transform.forward vector will hit y=0 plane in less than 5 units

        // calculate the the intersection point
        Vector3 rayEnd = transform.position + transform.forward * maxDistance;

        if (rayEnd.y < 0 && transform.position.y > 0) {
            // ray hits the ground

            // find intersection point
            float t = -transform.position.y / (rayEnd.y - transform.position.y);
            Vector3 intersection = transform.position + t * (rayEnd - transform.position);

            // set position to intersection point
            SpawnPointObject.transform.position = new Vector3(intersection.x, 0, intersection.z);
            IsAvaliable = true;
        } else {
            // ray did not hit the ground
            IsAvaliable = false;
        }


        
    }


}
