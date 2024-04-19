using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateSphereOverlay : MonoBehaviour
{
    public static PlayerStateSphereOverlay Singleton { get; private set; }
    private float currentAlpha = 0;
    private Boolean isFadeIn = false;
    private Boolean isFadeOut = false;
    private Boolean isOverlayActive = false;
    private Boolean isOverlayStatic = false;
    private Boolean isPaintOverlayFadeOut = false;
    private Boolean isPaintCountdown = false;
    private float paintCountdown = 0;
    [SerializeField] Material SphereOverlayMaterial;

    [SerializeField] private Color redColor = new Color(113f/255f, 9f/255f, 9f/255f, 1);
    [SerializeField] private Color blueColor = new Color(0f/255f, 255f/255f, 255f/255f, 1);
    [SerializeField] private Color greenColor = new Color(100f/255f, 255f/255f, 0f/255f, 1);
    [SerializeField] private Slider HealthBarSlider;

    [Header("Paint")]
    [SerializeField] private Material paintMaterial;

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
        PlayerHealthUpdate(0);

        paintCountdown = 0;
    }

    public void PlayerRespawned() {
        currentColor = blueColor;
        isFadeIn = true;
        isFadeOut = false;
        isOverlayActive = true;
        isOverlayStatic = true;
        PlayerHealthUpdate(100);
    }

    public void PlayerAlive() {
        isFadeIn = false;
        isFadeOut = true;
        PlayerHealthUpdate(100);
    }

    public void PlayerDamage(float health) {
        if (!isOverlayStatic) {
            currentColor = redColor;
            currentAlpha = 1.3f;
            isOverlayActive = true;
            isFadeOut = true;
            isOverlayActive = true;
        }

        PlayerHealthUpdate(health);
    }

    public void PlayerHeal(float health) {
        if (!isOverlayStatic) {
            currentColor = greenColor;
            currentAlpha = 1.3f;
            isOverlayActive = true;
            isFadeOut = true;
            isOverlayActive = true;
        }

        PlayerHealthUpdate(health);
    }

    public void PlayerHealthUpdate(float health) {
        HealthBarSlider.value = health/100f;
    }

    public void PlayerPaint(float time) {
        // set _alphamul material property to 1
        paintMaterial.SetFloat("_alphaMult", 1);
        isPaintCountdown = true;
        paintCountdown = time;
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

        if (isPaintCountdown) {
            paintCountdown -= Time.deltaTime;
            if (paintCountdown <= 0) {
                isPaintCountdown = false;
                isPaintOverlayFadeOut = true;
            }
        }


        if (isPaintOverlayFadeOut) {
            paintMaterial.SetFloat("_alphaMult", Mathf.Max(0, paintMaterial.GetFloat("_alphaMult") - Time.deltaTime * 0.5f));
            if (paintMaterial.GetFloat("_alphaMult") <= 0) {
                isPaintOverlayFadeOut = false;
            }
        }
    }
}
