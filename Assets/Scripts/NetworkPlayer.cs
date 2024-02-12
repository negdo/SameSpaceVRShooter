using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] Transform head;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform body;
    [SerializeField] GameObject readyButton;
    [SerializeField] PlayerTextOverlay OverlayText;

    // queue of last 10 head positions
    private Queue<Vector3> recentHeadPositions = new Queue<Vector3>();


    public NetworkVariable<int> team = new NetworkVariable<int>(0);
    public NetworkVariable<int> state = new NetworkVariable<int>(PlayerState.NotReady);
    private float maxHealth = 100;
    public NetworkVariable<float> health = new NetworkVariable<float>();
    public NetworkVariable<int> kills = new NetworkVariable<int>();
    public NetworkVariable<int> deaths = new NetworkVariable<int>();




    private void Start() {
        if (IsServer) {
            health.Value = maxHealth;
            SetPlayerState(PlayerState.NotReady);
        }

        if (IsOwner) {
            OverlayText.GetToStartingZone();
        }
    }

    void Update() {
        if (IsOwner) {
            root.position = VRRigReferences.Singelton.root.position;
            root.rotation = VRRigReferences.Singelton.root.rotation;

            head.position = VRRigReferences.Singelton.head.position;
            head.rotation = VRRigReferences.Singelton.head.rotation;

            leftHand.position = VRRigReferences.Singelton.leftHand.position;
            leftHand.rotation = VRRigReferences.Singelton.leftHand.rotation * Quaternion.Euler(45, 0, 0);

            rightHand.position = VRRigReferences.Singelton.rightHand.position;
            rightHand.rotation = VRRigReferences.Singelton.rightHand.rotation * Quaternion.Euler(45, 0, 0);

            recentHeadPositions.Enqueue(VRRigReferences.Singelton.head.position);
            if (recentHeadPositions.Count > 20) {
                recentHeadPositions.Dequeue();
            }

            
            // average head position
            Vector3 averageHeadPosition = Vector3.zero;
            foreach (Vector3 position in recentHeadPositions) {
                averageHeadPosition += position;
            }
            averageHeadPosition /= recentHeadPositions.Count;


            body.position = averageHeadPosition - new Vector3(0, 0.2f, 0);
            body.rotation = Quaternion.Euler(0, VRRigReferences.Singelton.head.rotation.eulerAngles.y, 0);

            if (state.Value == PlayerState.Alive) {
                CheckDead();
            }
        }
    }

    private void CheckDead() {
        if (health.Value <= 0) {
            DieServerRpc();

            // hide all walls on the owner client
            Wall[] walls = FindObjectsOfType<Wall>();
            foreach (Wall wall in walls) {
                wall.hideWall();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    protected void DieServerRpc() {
        SetPlayerState(PlayerState.Dead);
        deaths.Value++;
    }

    [ServerRpc(RequireOwnership = false)]
    protected void RespawnServerRpc() {
        SetPlayerState(PlayerState.Respawning);
        health.Value = maxHealth;
    }

    [ServerRpc(RequireOwnership = false)]
    protected void AliveServerRpc() {
        SetPlayerState(PlayerState.Alive);
    }

    public void BulletHit(Transform transform, float damage) {
        if (GameOperator.instance.gameState.Value != State.Game || state.Value != PlayerState.Alive) {
            return; // do nothing if not in game or not alive
        }
        SubtractHealthServerRpc(damage);
        DamageClientRpc();
    }

    [ClientRpc]
    public void DamageClientRpc() { 
        if (IsOwner) PlayerStateSphereOverlay.Singleton.PlayerDamage(); 
    }

    [ServerRpc (RequireOwnership = false)]
    public void SubtractHealthServerRpc(float damage) {
        if (IsOwner) {
            PlayerStateSphereOverlay.Singleton.PlayerDamage();
        }
        health.Value -= damage;
    }

    public float GetHealth() { return health.Value; }

    public void StartingPointColided(int StartPointTeam) {
        if (IsOwner) {
            // show ready button when in lobby
            if (GameOperator.instance.gameState.Value == State.Lobby) {
                    readyButton.gameObject.SetActive(true);
                    OverlayText.ClickReadyText();
                
            // respawn when game is running and player is dead
            } else if (GameOperator.instance.gameState.Value == State.Game && state.Value == PlayerState.Dead) {
                if (team.Value == StartPointTeam) {
                    RespawnServerRpc();

                    // show all walls on the owner client
                    Wall[] walls = FindObjectsOfType<Wall>();
                    foreach (Wall wall in walls) {
                        wall.showWall();
                    }
                }
            } else if (PlayerLocalState.Singleton.GetPlayerState() == PlayerState.Dead) {
                Debug.Log("Player is dead, but not weirdly dead " + GameOperator.instance.gameState.Value + state.Value + PlayerLocalState.Singleton.GetPlayerState());
            
            }
        }
    }

    public void StartingPointLeft(int StartPointTeam) {
        if (IsOwner) {
            if (GameOperator.instance.gameState.Value == State.Lobby) {
                readyButton.gameObject.SetActive(false);
                SetNotReady();

            // make player alive when game is running and player is respawning state inside starting point
            } else if (GameOperator.instance.gameState.Value == State.Game && state.Value == PlayerState.Respawning) {
                if (team.Value == StartPointTeam)
                    AliveServerRpc();
            }
        }
    }

    public void AssignTeam(int team)
    {
        // find children components with TeamColorSetter script
        this.team.Value = team;
        TeamColorSetter[] teamColorSetters = GetComponentsInChildren<TeamColorSetter>();
        foreach (TeamColorSetter teamColorSetter in teamColorSetters)
        {
            teamColorSetter.AssignTeam(team);
        }
    }

    public void StartGame() {
        // called on server
        readyButton.gameObject.SetActive(false);
        if (IsOwner) {
            OverlayText.Countdown();
        }
        // delay start game for 5 seconds
        StartCoroutine(StartGameDelayedCoroutine());
    }

    private IEnumerator StartGameDelayedCoroutine() {
        yield return new WaitForSeconds(5);
        StartGameDelayed();
    }

    private void StartGameDelayed() {
        // called on server
        SetPlayerState(PlayerState.Alive);
        health.Value = maxHealth;
        StartGameClientRpc();
    }

    [ClientRpc]
    public void StartGameClientRpc() { 
        readyButton.gameObject.SetActive(false);
        if (IsOwner) {
            OverlayText.Countdown();
        }
    }

    public void SetReady() { SetReadyServerRpc(); }
    public void SetNotReady() { SetNotReadyServerRpc(); }

    [ServerRpc (RequireOwnership = false)]
    public void SetReadyServerRpc() {
        SetPlayerState(PlayerState.Ready);
        GameOperator.instance.CheckStartGame();
    }

    [ServerRpc (RequireOwnership = false)]
    public void SetNotReadyServerRpc() { SetPlayerState(PlayerState.NotReady); }

    public int GetLocalPlayerState() {
        if (IsOwner) {
            return state.Value;
        }
        return -1;
    }

    private void SetPlayerState(int state) {
        // called on server
        this.state.Value = state;
        if (IsOwner) {
            PlayerLocalState.Singleton.SetPlayerState(state);
        } else {
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

