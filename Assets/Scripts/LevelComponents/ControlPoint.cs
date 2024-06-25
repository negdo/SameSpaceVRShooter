using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class ControlPoint : NetworkBehaviour {
    [SerializeField] private Collider pointCollider;
    [SerializeField] private float captureTime = 10f;
    [SerializeField] private Material circleMaterial;

    private NetworkVariable<float> captureProgress = new NetworkVariable<float>(0f);
    private bool capturePaused = false;
    private NetworkVariable<int> teamOnPoint = new NetworkVariable<int>(-1);
    private NetworkVariable<float> team1Points = new NetworkVariable<float>(0);
    private NetworkVariable<float> team2Points = new NetworkVariable<float>(0);

    private int captureSpeedMultiplier = 1;



    private int GetTeamOnPoint() {
        // called on server
        Collider[] colliders = Physics.OverlapBox(pointCollider.bounds.center, pointCollider.bounds.extents, pointCollider.transform.rotation, LayerMask.GetMask("Default"));
        HashSet<NetworkPlayer> players = new HashSet<NetworkPlayer>();
        
        foreach (Collider collider in colliders) {
            if (collider.gameObject.CompareTag("Player")) {
                NetworkPlayer player = collider.gameObject.GetComponentInParent<NetworkPlayer>();
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

        captureSpeedMultiplier = Math.Abs(team1Players - team2Players);

        if (team1Players > team2Players) {
            return 0;
        } else if (team2Players > team1Players) {
            return 1;
        } else if (team1Players > 0 && team2Players > 0) {
            return -1;
        } else {
            return -2;
        }
    }

    private void UpdateTeamOnPoint() {
        // called on server
        if (IsServer) {
            if (GameOperator.Singleton.gameState.Value != State.Game) {
                captureProgress.Value = 0;
                return;
            }

            int team = GetTeamOnPoint();

            if (team == -2) {
                // no players on point
                captureProgress.Value -= 0.5f * Time.deltaTime / captureTime;
            } else if(team == -1) {
                // same number of both: keep the current progess
            } else {
                if (team != teamOnPoint.Value) {
                    captureProgress.Value -= captureSpeedMultiplier * Time.deltaTime / captureTime;
                    teamOnPoint.Value = team;
                } else {
                    captureProgress.Value += captureSpeedMultiplier * Time.deltaTime / captureTime;

                    if (captureProgress.Value >= 1) {
                        if (teamOnPoint.Value == 0) {
                            team1Points.Value += Time.deltaTime * captureSpeedMultiplier;
                        } else {
                            team2Points.Value += Time.deltaTime * captureSpeedMultiplier;
                        }
                    }
                }
            }
        }
    }

    public int[] getPoints() {
        Debug.Log("Getting points: " + team1Points.Value + " " + team2Points.Value);
        // int version
        Debug.Log("Getting points: " + (int) team1Points.Value + " " + (int) team2Points.Value);
        return new int[] { (int) team1Points.Value, (int) team2Points.Value };
    }




    private void Update() {
        if (IsServer) {
            UpdateTeamOnPoint();
        }
        circleMaterial.SetFloat("_fill", captureProgress.Value);
        if (teamOnPoint.Value == 0) {
            circleMaterial.SetFloat("_team", 0);
        } else if (teamOnPoint.Value == 1) {
            circleMaterial.SetFloat("_team", 1);
        }
    }   
}
