using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ButtonArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Button Pressed" + other.gameObject);
        if (other.gameObject.tag == "Interactor") {
            OnButtonAction();
        }
    }

    protected abstract void OnButtonAction();
}
