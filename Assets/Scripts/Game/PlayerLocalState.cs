using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerLocalState : MonoBehaviour
{
    public static PlayerLocalState Singleton { get; private set; }

    [SerializeField] private PlayerStateSphereOverlay playerStateSphereOverlay;
    [SerializeField] private TextMeshProUGUI playerHintText;

    private NetworkPlayer localNetworkPlayer = null;

    private int localPlayerState;

    private NetworkPlayer GetLocalNetworkPlayer() {
        if (localNetworkPlayer == null) {
            NetworkPlayer[] networkPlayers = FindObjectsOfType<NetworkPlayer>();
            foreach (NetworkPlayer networkPlayer in networkPlayers) {
                if (networkPlayer.IsLocalPlayer) {
                    localNetworkPlayer = networkPlayer;
                    break;
                }
            }

            if (localNetworkPlayer == null) {
                Debug.LogError("Local player not found");
            }
        }

        return localNetworkPlayer;
    }


    void Start() {
        Singleton = this;
        localPlayerState = PlayerState.NotReady;
    }

    public void SetPlayerState(int state) {
        CallUIOverlay(state);
        CallOtherUpdates(state);
        localPlayerState = state;
    }

    public int GetPlayerState() {
        return localPlayerState;
    }


    public void SetPlayerReadyGlobal() {
        NetworkPlayer p = GetLocalNetworkPlayer();
        p.SetReady();
    }

    public void SetPlayerNotReadyGlobal() {
        NetworkPlayer p = GetLocalNetworkPlayer();
        p.SetNotReady();
    }



    private void CallUIOverlay(int state) {
        if (state == PlayerState.Alive) {
            playerStateSphereOverlay.PlayerAlive();
            playerHintText.text = "";


        } else if (state == PlayerState.Dead) {
            playerStateSphereOverlay.PlayerDied();
            playerHintText.text = "You Died";
            playerHintText.color = new Color(0.8f, 0.2f, 0, 1);


        } else if (state == PlayerState.Respawning) {
            playerStateSphereOverlay.PlayerRespawned();
            playerHintText.text = "Respawning";
            playerHintText.color = new Color(1, 1, 1, 1);

        } else if (state == PlayerState.NotReady) {
            playerHintText.text = "Press the button to ready up";
            playerHintText.color = new Color(1, 1, 1, 1);

        
        } else if (state == PlayerState.Ready) {
            playerHintText.text = "Waiting for other players";
            playerHintText.color = new Color(1, 1, 1, 1);


        } else if (state == PlayerState.Spectating) {
            


        }
    }


    private void CallOtherUpdates(int state) {
        if (state == PlayerState.Alive) {
            Wall[] walls = FindObjectsOfType<Wall>();
            foreach (Wall wall in walls) {
                wall.riseWall();
            }


        } else if (state == PlayerState.Dead) {
            // hide all walls
            Wall[] walls = FindObjectsOfType<Wall>();
            foreach (Wall wall in walls) {
                wall.lowerWall();
            }

            // TODO: drop everything from hands
            

        } else if (state == PlayerState.Respawning) {
            Wall[] walls = FindObjectsOfType<Wall>();
            foreach (Wall wall in walls) {
                wall.riseWall();
            }
            

        } else if (state == PlayerState.NotReady) {
                
    
        } else if (state == PlayerState.Ready) {
            

        } else if (state == PlayerState.Spectating) {


        }
    }
}