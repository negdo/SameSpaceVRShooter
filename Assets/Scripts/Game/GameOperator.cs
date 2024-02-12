using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameOperator : NetworkBehaviour
{
    public static GameOperator instance;


    public NetworkVariable<int> gameState = new NetworkVariable<int>(State.Lobby);


    private NetworkPlayer[] players;
    public GameObject[] startingPoints;
    


    private void Awake() {
        instance = this;
    }

    public void CheckStartGame() {
        Debug.Log("Checking if game can start");
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

    private void StartGame()
    {
        Debug.Log("Starting game");

        // get all LevelComponentGrabable objects in the scene
        LevelComponentGrabable[] grabables = FindObjectsOfType<LevelComponentGrabable>();

        // disable grabable objects
        foreach (LevelComponentGrabable grabable in grabables)
        {
            grabable.SetGrabDisabled();
        }

        // get all spawn points in the scene and assign them to a team
        startingPoints = GameObject.FindGameObjectsWithTag("StartingPoint");
        for (int i = 0; i < startingPoints.Length; i++) {
            startingPoints[i].GetComponent<TeamColorSetter>().AssignTeam(i);
        }


        // assign each player to a spawn point
        for (int i = 0; i < players.Length; i++)
        {
            int team = 0;
            float closestPointDistance = Mathf.Infinity;

            for (int j = 0; j < startingPoints.Length; j++)
            {
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


    }

}



public static class State {
    public const int Lobby = 0;
    public const int Game = 1;
    public const int End = 2;
}

