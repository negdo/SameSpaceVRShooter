using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPoint : MonoBehaviour {
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private Collider buttonCollider;
    [SerializeField] private ButtonPhysicalReady buttonPhysicalReady;
    

    public void SetStateLoby() {
        buttonAnimator.SetBool("isUp", true);
        buttonPhysicalReady.resetButton();
        StartCoroutine(EnableCollider());
    }

    public void SetStateGame() {
        buttonAnimator.SetBool("isUp", false);
        buttonCollider.enabled = false;
    }

    IEnumerator EnableCollider() {
        yield return new WaitForSeconds(2);
        buttonCollider.enabled = true;
    }
}
