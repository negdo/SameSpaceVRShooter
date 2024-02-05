using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ButtonArea : MonoBehaviour
{
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Button Pressed" + other.gameObject);
        if (other.gameObject == leftController || other.gameObject == rightController)
        {
            
            OnButtonAction();
        }
    }

    protected abstract void OnButtonAction();
}
