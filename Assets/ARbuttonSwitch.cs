using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARbuttonSwitch : MonoBehaviour
{
    private bool isAR = false;
    public void ToggleAR() {
        isAR = !isAR;
        // get all NetwrkPlayers
        Debug.Log("Toggled AR");
        GameOperator gameOperator = FindObjectOfType<GameOperator>();
        gameOperator.SetAR(isAR);


    }

    public void ServerStartGame() {
        GameOperator gameOperator = FindObjectOfType<GameOperator>();
        gameOperator.ServerStartGame();
    }
}
