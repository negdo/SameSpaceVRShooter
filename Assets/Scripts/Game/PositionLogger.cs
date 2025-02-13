using UnityEngine;
using System.Text;
using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;

public class PositionLogger : MonoBehaviour {
    private StringBuilder positions = new StringBuilder();
    private float start_time;
    private float last_time = 0;
    private float interval = 0.1f;


    public void writeToFile() {
        Debug.Log("Writing to file");
        Debug.Log(positions.ToString());
        System.IO.File.WriteAllText("positions-" + Time.time.ToString() + ".txt", positions.ToString());
    }

    public void logPosition(int playerid, Vector3 position) {
        positions.Append(playerid.ToString() + ":   " + position.x + " " + position.y + " " + position.z + " " + (Time.time - start_time) + "\n");
    }

    public void StartLogger() {
        // clear positions
        positions = new StringBuilder();
        // start time
        start_time = Time.time;
    }

    public void EndLogger() {
        writeToFile();
    }

    public void RunLogger() {
        if (Time.time - last_time > interval) {
            NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();
            int i = 0;
            foreach (NetworkPlayer player in players) {
                // get child component named body
                Transform body = player.transform.Find("Body");
                logPosition(i, body.position);
                i++;
            }
            last_time = Time.time;
        }
    }
}