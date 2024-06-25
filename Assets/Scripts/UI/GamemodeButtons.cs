using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeButtons : MonoBehaviour {
    // list of buttons
    [SerializeField] private GameObject KingOfTheHillButton;
    [SerializeField] private GameObject TeamDeathmatchButton;

    [SerializeField] private Color SelectedColor;
    [SerializeField] private Color UnselectedColor;



    public void ButtonClickKingOfTheHill() {
        KingOfTheHillButton.GetComponent<UnityEngine.UI.Image>().color = SelectedColor;
        TeamDeathmatchButton.GetComponent<UnityEngine.UI.Image>().color = UnselectedColor;
        SetGamemode(GameMode.KingOfTheHill);
    }

    public void ButtonClickTeamDeathmatch() {
        KingOfTheHillButton.GetComponent<UnityEngine.UI.Image>().color = UnselectedColor;
        TeamDeathmatchButton.GetComponent<UnityEngine.UI.Image>().color = SelectedColor;
        SetGamemode(GameMode.TeamDeathmatch);
    }

    private void SetGamemode(int gamemode) {
        SceneLoader.gameMode = gamemode;
    }
    
}