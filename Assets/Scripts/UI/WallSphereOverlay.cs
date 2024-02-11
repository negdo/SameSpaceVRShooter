using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallSphereOverlay : MonoBehaviour
{
    private float currentAlpha = 0;
    private Boolean isFadeIn = false;
    private Boolean isFadeOut = false;
    private Boolean isPlayerInsideWall = false;
    private Boolean isOverlayActive = false;
    private GameObject collidedWall;
    private Vector3 referenceExitPoint;
    private float referenceExitPointDistance = 0.5f;
    private float referenceExitPointMargin = 0.2f;
    private float referenceExitPointEndThreshold = 0.4f;
    [SerializeField] Material SphereOverlayMaterial;

    private void OnTriggerEnter(Collider other){
        // if entering wall
        if (!isPlayerInsideWall && other.tag == "Wall") {

            if (PlayerLocalState.Singleton.GetPlayerState() == PlayerState.Alive) {
                isPlayerInsideWall = true;
                isOverlayActive = true;
                isFadeIn = true;
                isFadeOut = false;
                collidedWall = other.gameObject;
                
                referenceExitPoint = GetReferenceExitPoint(other);
            }
        }
    }

    private void OnTriggerExit(Collider other){
        // if exiting wall
        if (isPlayerInsideWall && other.gameObject == collidedWall) {
            if (Vector3.Distance(transform.position, referenceExitPoint) < referenceExitPointMargin + referenceExitPointDistance) {
                isPlayerInsideWall = false;
                isFadeIn = false;
                isFadeOut = true;
                collidedWall.GetComponent<Wall>().showWall();
            }
        }
    }

    private Vector3 GetReferenceExitPoint(Collider wallCollider) {
        // calculate point 0.5m away from the wall in the direction of the player
        Vector3 collisionPoint = wallCollider.ClosestPoint(transform.position);
        Vector3 normalizedDirection = (transform.position - collisionPoint).normalized;
        return collisionPoint + normalizedDirection * referenceExitPointDistance;
    }

    private void SetOverlay(float alpha) {
        // set material alpha value
        SphereOverlayMaterial.SetFloat("_AlphaValue", alpha);
    }

    void Update() {
        if (isOverlayActive) {
            // fade in overlay
            if (isFadeIn) {
                currentAlpha += Time.deltaTime * 2;
                if (currentAlpha >= 1) {
                    currentAlpha = 1;
                    isFadeIn = false;
                    collidedWall.GetComponent<Wall>().hideWall();
                }
                SetOverlay(currentAlpha);

            // fade out overlay
            } else if (isFadeOut) {
                currentAlpha -= Time.deltaTime * 2;
                if (currentAlpha <= 0) {
                    currentAlpha = 0;
                    isFadeOut = false;
                    isOverlayActive = false;
                }
                SetOverlay(currentAlpha);
            }
            // rotate overlay to face the reference exit point
            transform.LookAt(referenceExitPoint);

            // exit wall if player is close enough to the reference exit point
            if (Vector3.Distance(transform.position, referenceExitPoint) < referenceExitPointEndThreshold) {
                isPlayerInsideWall = false;
                isFadeIn = false;
                isFadeOut = true;
                collidedWall.GetComponent<Wall>().showWall();
            }
        }

    }
}
