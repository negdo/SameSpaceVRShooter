using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveGridMaterialUpdater : MonoBehaviour
{
    // reference to gameobject
    [SerializeField] private GameObject trackedObject;
    // reference to material
    [SerializeField] private Material material;

    [SerializeField] private Material material2;


    // Update is called once per frame
    void Update() {
        material.SetVector("_ObjectPosition", trackedObject.transform.position);
        material2.SetVector("_ObjectPosition", trackedObject.transform.position);
    }
}
