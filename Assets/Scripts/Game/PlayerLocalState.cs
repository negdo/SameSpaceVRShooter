using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerLocalState : MonoBehaviour
{
    public static PlayerLocalState Singleton { get; private set; }

    [SerializeField] private PlayerStateSphereOverlay playerStateSphereOverlay;

    private int localPlayerState;

    void Start() {
        Singleton = this;
        UpdateState();
    }

    public void SetPlayerState(int state) {
        if (localPlayerState != state) {
            CallUIOverlay(state);
        }


        localPlayerState = state;
    }

    public int GetPlayerState() {
        return localPlayerState;
    }

    public void UpdateState() {
        // get network players
        NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();

        foreach (NetworkPlayer player in players) {
            int foundState = player.GetLocalPlayerState();
            if (foundState != -1) {
                SetPlayerState(foundState);
                break;
            }
        }
    }

    private void CallUIOverlay(int state) {
        if (state == PlayerState.Alive) {
            playerStateSphereOverlay.PlayerAlive();
        } else if (state == PlayerState.Dead) {
            playerStateSphereOverlay.PlayerDied();
        } else if (state == PlayerState.Respawning) {
            playerStateSphereOverlay.PlayerRespawned();
        }
    }
}