using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEditor.Rendering;
using UnityEngine;

public class NetworkTransformLinear : NetworkTransform
{
    // get Bullet component from gameobject
    private Bullet bullet;

    private void Start()
    {
        // get Bullet component from gameobject
        bullet = GetComponent<Bullet>();
    }
    
    

    
    /*protected override void Update()
    {
        if (IsOwner)
        {
            base.Update();
        }
        else
        {
            // move the object
            //transform.Translate(bullet.GetMovement(Time.deltaTime));
        }
    }*/
}