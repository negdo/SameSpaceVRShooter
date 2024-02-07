using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class NetworkTransformLinear : NetworkTransform
{
    // get Bullet component from gameobject
    IMovement bulletMovement;
    Vector3 lastPositionNetwork = Vector3.zero;
    Vector3 lastPositionLocal = Vector3.zero;



    private void Start() {
        // get Bullet Movement Interface
        bulletMovement = GetComponent<IMovement>();
    }

    protected override void Update() {
        base.Update();

        if (!IsOwner && lastPositionNetwork == transform.position)
        {
            //lastPositionLocal += bulletMovement.GetMovement(Time.deltaTime);
            // transform movement with rotation
            Vector3 localMovement = bulletMovement.GetMovement(Time.deltaTime);
            Vector3 GlobalMovement = transform.TransformDirection(localMovement);

            lastPositionLocal += GlobalMovement;
            transform.position = lastPositionLocal;

        } else if (!IsOwner) {
            lastPositionNetwork = transform.position;
            lastPositionLocal = transform.position;
        }
    }
}