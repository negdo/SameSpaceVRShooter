using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class GameOperator : NetworkBehaviour
{
    public static GameOperator Singleton { get; private set; }
    public NetworkVariable<int> gameState = new NetworkVariable<int>(State.Lobby);
    private NetworkPlayer[] players;
    private NetworkVariable<int> killsTeam0 = new NetworkVariable<int>(0);
    private NetworkVariable<int> killsTeam1 = new NetworkVariable<int>(0);
    public GameObject[] startingPoints;
    public NetworkVariable<float> timeLeft = new NetworkVariable<float>(0);
    public NetworkVariable<int> gameMode = new NetworkVariable<int>(GameMode.TeamDeathmatch);
    private float timeGame = 180;


    void Start() {
        Singleton = this;
    }

    public void CheckStartGame() {
        // called on server
        if (IsServer) {
            
            // get all players in the scene
            players = FindObjectsOfType<NetworkPlayer>();

            // check if all players are ready
            bool allReady = true;
            foreach (NetworkPlayer player in players) {
                if (player.state.Value != PlayerState.Ready) {
                    allReady = false;
                    break;
                }
            }

            // if all players are ready, start the game
            if (allReady)
            {
                gameState.Value = State.Game;
                StartGame();
            } else {
                Debug.Log("Not all players are ready");
            }

        } else {
            Debug.Log("Not server");
        }
    }

    private void StartGame() {
        // called on server
        Debug.Log("Starting game");

        // get all LevelComponentGrabable objects in the scene
        LevelComponentGrabable[] grabables = FindObjectsOfType<LevelComponentGrabable>();

        // disable grabable objects
        foreach (LevelComponentGrabable grabable in grabables) {
            grabable.SetGrabDisabled();
        }

        // get all spawn points in the scene and assign them to a team
        startingPoints = GameObject.FindGameObjectsWithTag("StartingPoint");
        for (int i = 0; i < startingPoints.Length; i++) {
            startingPoints[i].GetComponent<TeamColorSetter>().AssignTeam(i);
        }


        // assign each player to a spawn point
        for (int i = 0; i < players.Length; i++) {
            int team = 0;
            float closestPointDistance = Mathf.Infinity;

            for (int j = 0; j < startingPoints.Length; j++) {
                // get child object called "Head"
                Transform Head = players[i].transform.Find("Head");

                float distance = Vector3.Distance(Head.position, startingPoints[j].transform.position);
                if (distance < closestPointDistance) {
                    closestPointDistance = distance;
                    team = j;
                }
            }

            players[i].AssignTeam(team);
            players[i].StartGame();
        }

        // set time left
        timeLeft.Value = timeGame;

        // reset kills
        killsTeam0.Value = 0;
        killsTeam1.Value = 0;

        StartGameClientRpc();
    }

    [ClientRpc]
    private void StartGameClientRpc() {
        Debug.Log("Starting game client rpc");

        // drop all starting point ready buttons
        StartingPoint[] startingPoints = FindObjectsOfType<StartingPoint>();
        foreach (StartingPoint startingPoint in startingPoints) {
            startingPoint.SetStateGame();
        }
    }


    void Update() {
        if (IsServer) {
            if (gameState.Value == State.Game) {
                timeLeft.Value = Mathf.Max(0, timeLeft.Value - Time.deltaTime);
                if (timeLeft.Value == 0) {
                    EndGame();
                }
            }
        }
    }

    private void EndGame() {
        // called on server
        Debug.Log("Ending game");
        gameState.Value = State.End;

        // get all LevelComponentGrabable objects in the scene
        LevelComponentGrabable[] grabables = FindObjectsOfType<LevelComponentGrabable>();

        // disable grabable objects
        foreach (LevelComponentGrabable grabable in grabables) {
            grabable.SetGrabEnabled();
        }

        // get all spawn points and reset teams
        startingPoints = GameObject.FindGameObjectsWithTag("StartingPoint");
        for (int i = 0; i < startingPoints.Length; i++) {
            startingPoints[i].GetComponent<TeamColorSetter>().AssignTeam(-1);
        }

        // reset teams of all players
        for (int i = 0; i < players.Length; i++) {
            players[i].AssignTeam(-1);
            players[i].EndGame();
        }

        EndGameClientRpc();
    }

    [ClientRpc]
    private void EndGameClientRpc() {
        Debug.Log("Ending game client rpc");

        // reset all starting points
        StartingPoint[] startingPoints = FindObjectsOfType<StartingPoint>();
        foreach (StartingPoint startingPoint in startingPoints) {
            startingPoint.SetStateLoby();
        }
    }

    public int[] getKills() {
        return new int[] {killsTeam0.Value, killsTeam1.Value};
    }

    public void AddKillToOtherTeam(int team) {
        // called on server
        if (team == 1) {
            killsTeam0.Value++;
        } else {
            killsTeam1.Value++;
        }
    }
        



}



public static class State {
    public const int Lobby = 0;
    public const int Game = 1;
    public const int End = 2;
}

public static class GameMode {
    public const int TeamDeathmatch = 1;
    public const int CaptureTheFlag = 2;
    public const int LastManStanding = 3;
    public const int KingOfTheHill = 4;
}

