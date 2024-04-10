using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ButtonArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cursor") {
            OnButtonAction();
        }
    }

    protected abstract void OnButtonAction();
}
