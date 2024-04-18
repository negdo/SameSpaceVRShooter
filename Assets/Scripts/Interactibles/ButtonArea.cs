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
        } else if (other.gameObject.tag == "Interactor") {
            OnButtonActionHand();
        }
    }

    protected abstract void OnButtonAction();
    protected abstract void OnButtonActionHand();
}
