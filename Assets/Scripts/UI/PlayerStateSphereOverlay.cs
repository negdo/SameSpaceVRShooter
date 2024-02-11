using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateSphereOverlay : MonoBehaviour
{
    public static PlayerStateSphereOverlay Singleton { get; private set; }
    private float currentAlpha = 0;
    private Boolean isFadeIn = false;
    private Boolean isFadeOut = false;
    private Boolean isOverlayActive = false;
    private Boolean isOverlayStatic = false;
    [SerializeField] Material SphereOverlayMaterial;

    [SerializeField] private Color redColor = new Color(113f/255f, 9f/255f, 9f/255f, 1);
    [SerializeField] private Color blueColor = new Color(0f/255f, 255f/255f, 255f/255f, 1);

    private Color currentColor;

    void Start() {
        Singleton = this;
        currentColor = blueColor;
    }


    private void SetOverlay(float alpha) {
        // set material alpha value
        SphereOverlayMaterial.SetFloat("_AlphaValue", alpha);
        SphereOverlayMaterial.SetColor("_Color", currentColor);
    }

    public void PlayerDied() {
        currentColor = redColor;
        isOverlayActive = true;
        isOverlayStatic = true;
        isFadeIn = true;
        isFadeOut = false;
    }

    public void PlayerRespawned() {
        currentColor = blueColor;
        isFadeIn = true;
        isFadeOut = false;
        isOverlayActive = true;
        isOverlayStatic = true;
    }

    public void PlayerAlive() {
        isFadeIn = false;
        isFadeOut = true;
    }

    public void PlayerDamage() {
        if (!isOverlayStatic) {
            currentColor = redColor;
            currentAlpha = 1.3f;
            isOverlayActive = true;
            isFadeOut = true;
            isOverlayActive = true;
        }
    }



    void Update() {
        if (isOverlayActive) {
            // fade in overlay
            if (isFadeIn) {
                currentAlpha += Time.deltaTime * 2;
                if (currentAlpha >= 1) {
                    currentAlpha = 1;
                    isFadeIn = false;
                }
                SetOverlay(currentAlpha);

            // fade out overlay
            } else if (isFadeOut) {
                currentAlpha -= Time.deltaTime * 2;
                if (currentAlpha <= 0) {
                    currentAlpha = 0;
                    isFadeOut = false;
                    isOverlayActive = false;
                    isOverlayStatic = false;
                }
                SetOverlay(currentAlpha);
            }
        }
    }
}
