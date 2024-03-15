using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class ButtonPhysicalReady : MonoBehaviour {
    // controlls physical button in ready point
    private bool isPressed = false;
    private GameObject presser;

    [SerializeField] private Animator buttonAnimator;

    private void OnTriggerEnter(Collider other) {
        Debug.Log("ButtonPhysicalReady: OnTriggerEnter " + other.gameObject.tag);
        if (other.gameObject.tag == "Interactor") {
            if (!isPressed) {
                isPressed = true;
                presser = other.gameObject;
                buttonAnimator.SetBool("isPressed", true);

                if (PlayerLocalState.Singleton.GetPlayerState() == PlayerState.NotReady) {
                    PlayerLocalState.Singleton.SetPlayerReadyGlobal();
                } else if (PlayerLocalState.Singleton.GetPlayerState() == PlayerState.Ready) {
                    PlayerLocalState.Singleton.SetPlayerNotReadyGlobal();
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject == presser) {
            isPressed = false;
            presser = null;
            buttonAnimator.SetBool("isPressed", false);
        }
    }

    public void resetButton() {
        isPressed = false;
        presser = null;
        buttonAnimator.SetBool("isPressed", false);
    }
}
