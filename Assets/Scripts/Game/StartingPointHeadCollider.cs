using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPointHeadCollider : MonoBehaviour
{
    // collision trigger only on Water layer to collide with starting point
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("StartingPoint")) {
            NetworkPlayer player = gameObject.transform.parent.GetComponent<NetworkPlayer>();
            TeamColorSetter teamColorSetter = other.gameObject.GetComponent<TeamColorSetter>();
            player.StartingPointColided(teamColorSetter.GetTeam());
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("StartingPoint")) {
            NetworkPlayer player = gameObject.transform.parent.GetComponent<NetworkPlayer>();
            TeamColorSetter teamColorSetter = other.gameObject.GetComponent<TeamColorSetter>();
            player.StartingPointLeft(teamColorSetter.GetTeam());
        }
    }

}