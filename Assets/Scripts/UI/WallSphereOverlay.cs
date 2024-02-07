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
    [SerializeField] Material SphereOverlayMaterial;

    private void OnTriggerEnter(Collider other){
        if (!isPlayerInsideWall && other.tag == "Wall") {
            isPlayerInsideWall = true;
            isOverlayActive = true;
            isFadeIn = true;
            isFadeOut = false;
            collidedWall = other.gameObject;
            

            referenceExitPoint = GetReferenceExitPoint(other);
        }
    }

    private void OnTriggerExit(Collider other){
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
        Vector3 collisionPoint = wallCollider.ClosestPoint(transform.position);
        Vector3 normalizedDirection = (transform.position - collisionPoint).normalized;
        return collisionPoint + normalizedDirection * referenceExitPointDistance;
    }

    private void SetOverlay(float alpha) {
        SphereOverlayMaterial.SetFloat("_AlphaValue", alpha);
    }

    void Update()
    {
        if (isOverlayActive) {
            if (isFadeIn) {
                currentAlpha += Time.deltaTime * 2;
                if (currentAlpha >= 1) {
                    currentAlpha = 1;
                    isFadeIn = false;
                    collidedWall.GetComponent<Wall>().hideWall();
                }
                SetOverlay(currentAlpha);
            } else if (isFadeOut) {
                currentAlpha -= Time.deltaTime * 2;
                if (currentAlpha <= 0) {
                    currentAlpha = 0;
                    isFadeOut = false;
                    isOverlayActive = false;
                }
                SetOverlay(currentAlpha);
            }
            transform.LookAt(referenceExitPoint);
        }

    }
}
