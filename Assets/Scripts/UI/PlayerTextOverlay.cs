using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTextOverlay : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;

    private bool CountdownActive = false;
    private float CountDownTimer = 5;

    private void SetText(string text) {
        
        textMesh.text = text;
    }

    public void ClickReadyText() {
        SetText("Click ready button");
    }

    public void GetToStartingZone() {
        SetText("Get to your starting zone");
    }

    public void Countdown() {
        CountdownActive = true;
        CountDownTimer = 5f;
        StartCoroutine(CountdownCoroutine());
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


}