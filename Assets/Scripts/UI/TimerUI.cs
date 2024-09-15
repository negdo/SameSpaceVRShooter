using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI pointsTeam0Text;
    [SerializeField] TextMeshProUGUI pointsTeam1Text;
    private int pointsTeam0 = 0;
    private int pointsTeam1 = 0;

    void Update() {

        if (GameOperator.Singleton.gameState.Value == State.Game) {
            timerText.text = toClockString(GameOperator.Singleton.timeLeft.Value);
            setPointsText();
        } else {
            timerText.text = "00:00";
        }
    } 

    string toClockString(float time) {
        int minutes = (int) time / 60;
        int seconds = (int) time % 60;
        // format to 00:00
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void setPointsText() {
        
        int[] kills = GameOperator.Singleton.getPoints();
        if (kills[0] != pointsTeam0) {
            pointsTeam0 = kills[0];
            pointsTeam0Text.text = kills[0].ToString();
        }
        if (kills[1] != pointsTeam1) {
            pointsTeam1 = kills[1];
            pointsTeam1Text.text = kills[1].ToString();
        }
    }
}
