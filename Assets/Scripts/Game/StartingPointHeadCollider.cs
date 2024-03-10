using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPointHeadCollider : MonoBehaviour {
    
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


    public bool CheckIfCollidesTeam(int team) {
        // get colliders that collide with this collider
        Collider selfCollider = gameObject.GetComponent<Collider>();
        Collider[] colliders = Physics.OverlapBox(selfCollider.bounds.center, selfCollider.bounds.extents, selfCollider.transform.rotation, LayerMask.GetMask("Player"));
        
        foreach (Collider collider in colliders) {
            if (collider.gameObject.CompareTag("StartingPoint")) {
                TeamColorSetter teamColorSetter = collider.gameObject.GetComponent<TeamColorSetter>();
                if (teamColorSetter.GetTeam() == team) {
                    return true;
                }
            }
        }
        return false;
    }

}