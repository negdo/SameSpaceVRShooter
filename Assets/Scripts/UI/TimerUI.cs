using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI killsTeam0Text;
    [SerializeField] TextMeshProUGUI killsTeam1Text;
    private int killsTeam0 = 0;
    private int killsTeam1 = 0;

    void Update() {

        if (GameOperator.Singleton.gameState.Value == State.Game) {
            timerText.text = toClockString(GameOperator.Singleton.timeLeft.Value);
            setKillsText();
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

    private void setKillsText() {
        int[] kills = GameOperator.Singleton.getKills();
        if (kills[0] != killsTeam0) {
            killsTeam0 = kills[0];
            killsTeam0Text.text = kills[0].ToString();
        }
        if (kills[1] != killsTeam1) {
            killsTeam1 = kills[1];
            killsTeam1Text.text = kills[1].ToString();
        }
    }
}
