using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    public NetworkVariable<int> team = new NetworkVariable<int>(0);
    public NetworkVariable<int> state = new NetworkVariable<int>(PlayerState.NotReady);
    private float maxHealth = 100;
    public NetworkVariable<float> health = new NetworkVariable<float>();
    public NetworkVariable<int> kills = new NetworkVariable<int>();
    public NetworkVariable<int> deaths = new NetworkVariable<int>();

    [SerializeField] private StartingPointHeadCollider startingPointHeadColliders;


    public float GetHealth() { return health.Value; }

    private void Start() {
        // new player joins the game
        // set max health and state not-ready
        if (IsServer) {
            health.Value = maxHealth;

            SetPlayerState(PlayerState.NotReady);
        }
    }



    private void CheckDead() {
        // called on server to check if player is dead
        if (IsServer) {
            if (health.Value <= 0) {
                Die();
            }
        }
    }


    protected void Die() {
        // called on server when player dies
        Debug.Log("Die");
        SetPlayerState(PlayerState.Dead);
        deaths.Value++;

        // add kill to the other team
        GameOperator.Singleton.AddKillToOtherTeam(team.Value);

        // inside starting point -> respawn
        if (startingPointHeadColliders.CheckIfCollidesTeam(team.Value)) {
            Respawn();
        }
    }


    public void StartingPointColided(int StartPointTeam) {
        if (IsServer) {
            // game is running and player is dead -> respawn
            if (GameOperator.Singleton.gameState.Value == State.Game && state.Value == PlayerState.Dead) {
                // check if point is for the same team
                if (team.Value == StartPointTeam) {
                    Respawn();
                }
            }
        }
    }

    private void Respawn() {
        // called on server
        health.Value = maxHealth;
        SetPlayerState(PlayerState.Respawning);
    }

    public void StartingPointLeft(int StartPointTeam) {
        // called on server only
        // TODO call on server only
        if (IsServer) {
            // make player alive when game is running and player is respawning state inside starting point
            if (GameOperator.Singleton.gameState.Value == State.Game && state.Value == PlayerState.Respawning) {
                // check if point is for the same team
                if (team.Value == StartPointTeam) {
                    Alive();
                }
            } else if (GameOperator.Singleton.gameState.Value == State.Lobby && state.Value == PlayerState.Ready) {
                // make player not ready when leaving starting point
                SetNotReadyServerRpc();
            }
        }
    }

    private void Alive() {
        // called on server
        SetPlayerState(PlayerState.Alive);
    }

    public void BulletHit(Transform transform, float damage) {
        // called on server
        if (GameOperator.Singleton.gameState.Value == State.Game && state.Value == PlayerState.Alive) {
            health.Value = Mathf.Max(0, health.Value - damage);
            DamageClientRpc(health.Value);
            CheckDead();
        }
    }

    public void HealingHit(Transform transform, float healing) {
        // called on server
        if (GameOperator.Singleton.gameState.Value == State.Game && state.Value == PlayerState.Alive) {
            health.Value = Mathf.Min(maxHealth, health.Value + healing);
            HealClientRpc(health.Value);
        }
    }


    public void PaintHit(Transform transform, float damage) {
        // called on server
        if (GameOperator.Singleton.gameState.Value == State.Game && state.Value == PlayerState.Alive) {
            PaintOverlayClientRpc(damage);
        }
    }


    [ClientRpc]
    public void DamageClientRpc(float health) { 
        if (IsOwner) PlayerStateSphereOverlay.Singleton.PlayerDamage(health); 
    }

    [ClientRpc]
    public void HealClientRpc(float health) { 
        if (IsOwner) PlayerStateSphereOverlay.Singleton.PlayerHeal(health); 
    }

    [ClientRpc]
    public void PaintOverlayClientRpc(float time) { 
        if (IsOwner) PlayerStateSphereOverlay.Singleton.PlayerPaint(time);
    }

    public void AssignTeam(int team) {
        // find children components with TeamColorSetter script
        this.team.Value = team;
        TeamColorSetter[] teamColorSetters = GetComponentsInChildren<TeamColorSetter>();
        foreach (TeamColorSetter teamColorSetter in teamColorSetters) {
            teamColorSetter.AssignTeam(team);
        }
    }


    public void StartGame() {
        // called on server
        StartGameClientRpc();

        // delay start game for 5 seconds
        StartCoroutine(StartGameDelayedCoroutine());
    }

    private IEnumerator StartGameDelayedCoroutine() {
        yield return new WaitForSeconds(1);
        StartGameDelayed();
    }

    private void StartGameDelayed() {
        // called on server
        SetPlayerState(PlayerState.Alive);
        health.Value = maxHealth;
    }

    [ClientRpc]
    public void StartGameClientRpc() { 
        // TODO trigger countdown text on client
    }


    public void EndGame() {
        // called on server
        SetPlayerState(PlayerState.NotReady);
        health.Value = maxHealth;
    }


    public void SetReady() {
        // called on owner when clicking ready button
        if (IsOwner) {
            SetReadyServerRpc();
        }
    }

    public void SetNotReady() { 
        // called on owner when clicking on ready button again
        if (IsOwner) {
            SetNotReadyServerRpc();
        }
    }

    [ServerRpc (RequireOwnership = false)]
    public void SetReadyServerRpc() {
        SetPlayerState(PlayerState.Ready);
        GameOperator.Singleton.CheckStartGame();
    }

    [ServerRpc (RequireOwnership = false)]
    public void SetNotReadyServerRpc() {
        SetPlayerState(PlayerState.NotReady);
    }

    private void SetPlayerState(int state) {
        // called on server
        if (IsServer) {
            this.state.Value = state;
            SetPlayerStateClientRpc(state);
        } 
    }

    [ClientRpc]
    public void SetPlayerStateClientRpc(int state) {
        if (IsOwner) {
            PlayerLocalState.Singleton.SetPlayerState(state);
        }
    }

}

public static class PlayerState {
    public static int Alive = 0;
    public static int Dead = 1;
    public static int Spectating = 2;
    public static int NotReady = 3;
    public static int Ready = 4;
    public static int Respawning = 5;
}

