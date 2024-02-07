using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WallDistanceDarker : MonoBehaviour
{
    [SerializeField] Volume volume;
    [SerializeField] Transform playerCamera;

    private GameObject[] walls;

    float max_distance = 0.5f;
    float min_distance = 0.1f;
    float center_distance = 1.5f;

    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    void Update()
    {
        float min_distance = 1000;
        float distance;
        foreach (GameObject wall in walls) {
            if (Vector3.Distance(wall.transform.position, playerCamera.position) < center_distance) {
                Collider targetCollider = wall.GetComponent<Collider>();
                distance = Vector3.Distance(playerCamera.position, targetCollider.ClosestPoint(playerCamera.position));
                if (distance < min_distance) {
                    min_distance = distance;
                }
            }
        }
        //SetOverlay(min_distance);
    }






    private void SetOverlay(float distance) {
        distance -= min_distance;
        float weight = 1 - Math.Clamp(distance / max_distance, 0, 1);

        volume.weight = weight;
    }

}
