using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeButtonsUI : MonoBehaviour
{
    [SerializeField] private GamemodeButtons gamomodeButtonsManager;
    public void ButtonClickKingOfTheHill() {
        gamomodeButtonsManager.ButtonClickKingOfTheHill();
    }

    public void ButtonClickTeamDeathmatch() {
        gamomodeButtonsManager.ButtonClickTeamDeathmatch();
    }
}
