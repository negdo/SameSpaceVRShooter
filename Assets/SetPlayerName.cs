using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerName : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    public void SetPlayerNameFun()
    {
        SceneLoader.player_name = nameText.text;
    }
}
