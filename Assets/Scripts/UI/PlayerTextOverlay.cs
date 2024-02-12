using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTextOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    private bool CountdownActive = false;
    private float CountDownTimer = 5;

    private float DissapearTimer = 3;
    private float DissapearTimerMax = 3;

    private void SetText(string text) {    
        textMesh.text = text;
    }

    public void ClickReadyText() {
        SetText("Click Ready button");
        DissapearTimer = DissapearTimerMax;
    }

    public void GetToStartingZone() {
        SetText("Get to your starting zone");
        DissapearTimer = DissapearTimerMax;
    }

    public void Countdown() {
        CountdownActive = true;
        CountDownTimer = 5f;
        StartCoroutine(CountdownCoroutine());
    }

    public void hideText() {
        SetText("");
    }

    private IEnumerator CountdownCoroutine() {
        while (CountDownTimer > 0 && CountdownActive) {
            SetText(CountDownTimer.ToString());
            yield return new WaitForSeconds(1);
            CountDownTimer--;
        }
        if (CountdownActive) {
            SetText("GO!");
            yield return new WaitForSeconds(1);
            SetText("");
            CountdownActive = false;
        }
    }

    private void Update() {
        if (DissapearTimer > 0) {
            DissapearTimer -= Time.deltaTime;
            if (DissapearTimer <= 0) {
                SetText("");
            }
        }
    }




}