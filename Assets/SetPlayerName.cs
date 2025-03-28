using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SetPlayerName : NetworkBehaviour
{
    [SerializeField] private TMPro.TextMeshPro nameText;
    [SerializeField] private NetworkPlayer networkPlayer;
    
    // on network spawn, set the player name
    public override void OnNetworkSpawn()
    {
        // delay 1 second to make sure the player name is set
        StartCoroutine(SetPlayerNameDelayed());
    }

    private IEnumerator SetPlayerNameDelayed()
    {
        yield return new WaitForSeconds(1);
        nameText.text = networkPlayer.playerName.Value.ToString();
    }
}
