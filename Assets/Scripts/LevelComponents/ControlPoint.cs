using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ControlPoint : NetworkBehaviour {
    [SerializeField] private Collider pointCollider;

    private int GetTeamOnPoint() {
        Collider[] colliders = Physics.OverlapBox(pointCollider.bounds.center, pointCollider.bounds.extents, pointCollider.transform.rotation, LayerMask.GetMask("Default"));
        HashSet<NetworkPlayer> players = new HashSet<NetworkPlayer>();
        
        foreach (Collider collider in colliders) {
            if (collider.gameObject.CompareTag("Player")) {
                NetworkPlayer player = collider.gameObject.GetComponent<NetworkPlayer>();
                if (player != null) {
                    players.Add(player);
                }
            }
        }

        int team1Players = 0;
        int team2Players = 0;
        foreach (NetworkPlayer player in players) {
            if (player.state.Value == PlayerState.Alive) {
                if (player.team.Value == 0) {
                    team1Players++;
                } else {
                    team2Players++;
                }
            }
        }

        if (team1Players > team2Players) {
            return 0;
        } else if (team2Players > team1Players) {
            return 1;
        } else {
            return -1;
        }
    }
    
}
