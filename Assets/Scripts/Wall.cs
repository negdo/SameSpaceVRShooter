using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public void hideWall() {
        // turn off mesh renderer in children
        foreach (Transform child in transform) {
            child.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void showWall() {
        foreach (Transform child in transform) {
            child.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
