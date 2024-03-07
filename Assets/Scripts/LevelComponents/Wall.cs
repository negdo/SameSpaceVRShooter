using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    [SerializeField] Animator WallAnimator;
    [SerializeField] Collider WallCollider;
    [SerializeField] GameObject WallMesh;


    public void showWall() {
        WallMesh.SetActive(true);
    }

    public void hideWall() {
        WallMesh.SetActive(false);
    }



    public void riseWall() {
        WallAnimator.SetBool("IsUp", true);
        WallCollider.enabled = true;
    }

    public void lowerWall() {
        WallAnimator.SetBool("IsUp", false);
        WallCollider.enabled = false;
    }
}
