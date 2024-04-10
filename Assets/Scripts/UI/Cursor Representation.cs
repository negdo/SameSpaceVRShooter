using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRepresentation : MonoBehaviour {
    [SerializeField] private Material cursorMaterial;
    [SerializeField] private GameObject proximityObject;

    void OnEnable() {
        cursorMaterial.SetFloat("_alpha", 1);
    }

    void Update() {
        float distance = Vector3.Distance(transform.position, proximityObject.transform.position);
        // 0.3f -> alpha value 0
        // 0.1f -> alpha value 1
        float alpha = Math.Max(Math.Min((0.3f - distance) / (0.3f - 0.1f), 1), 0);
        cursorMaterial.SetFloat("_alpha", alpha);
    }
}




