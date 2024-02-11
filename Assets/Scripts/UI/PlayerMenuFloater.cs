using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlayerMenuFloater : MonoBehaviour
{
    [SerializeField] Transform head;
    private UnityEngine.Vector3 LastPosition;
    private float MovementFactor = 0.1f;


    private void OnEnable() {
        transform.position = head.position + head.forward * 0.5f;
    }

    
    void Update() {
        UnityEngine.Vector3 currentHeadPosition = head.position + head.forward * 0.5f;
        currentHeadPosition.y = Math.Max(currentHeadPosition.y, 1);
        LastPosition = LastPosition*(1-MovementFactor) + MovementFactor*currentHeadPosition;
        transform.position = LastPosition;
        transform.rotation = UnityEngine.Quaternion.LookRotation(transform.position - head.position);
    }
}
