using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkDisableHands : NetworkBehaviour
{
    [SerializeField] Animator animator;


    void Start() {
        if (IsOwner) {
            Debug.Log("Is owner of hand");
        } else {
            // disable animator
            animator.enabled = false;
        }
    }

}
