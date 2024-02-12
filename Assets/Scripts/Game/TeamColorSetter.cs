using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeamColorSetter : NetworkBehaviour
{
    private NetworkVariable<int> team = new NetworkVariable<int>(0);
    public void AssignTeam(int teamnumeber) {
        // called from server
        team.Value = teamnumeber;
        SetTeamColor(teamnumeber);
        AssignTeamClientRpc(teamnumeber);
    }


    [ClientRpc]
    public void AssignTeamClientRpc(int teamnumeber) {
        SetTeamColor(teamnumeber);
    }


    private void SetTeamColor(int teamnumeber) {
        Debug.Log("Setting team color");
        // set material _Color property to the team color
        GetComponent<Renderer>().material.SetColor("_TeamColor", TeamColor.teamColors[teamnumeber]);
    }

    public int GetTeam() {
        return team.Value;
    }
}
