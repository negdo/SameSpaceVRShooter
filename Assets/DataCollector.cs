using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using System.IO;


public class DataCollector : NetworkBehaviour
{
    [SerializeField] GameObject SkyPanels;
    string filePath = "";
    GameData gameData;
    PlayerData playerData;
    private List<GameData> gameDataList;
    // Start is called before the first frame update
    void Start()
    {
        // get current time to use it as filename
        if (IsServer) {
            filePath = Application.persistentDataPath + "/gamedata-" +System.DateTime.Now.ToString("-MM-dd-HH-mm-ss") + ".json";
            gameData.players = new PlayerData[4];
            StartCoroutine(SaveDataRoutine());
        }
    }

    IEnumerator SaveDataRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            SaveData();
        }
    }

     void SaveData()
    {
        // Update data (example: increasing score)
        gameData = new GameData();
        gameData.time = Time.time;
        gameData.isAr = ! SkyPanels.activeSelf;
        gameData.gamestate = 0;

        foreach (NetworkPlayer player in FindObjectsOfType<NetworkPlayer>()) {
            playerData = new PlayerData();
            playerData.playerName = player.playerName.Value.ToString();
            playerData.BodyPosition = player.transform.position;
            playerData.BodyRotation = player.transform.rotation.eulerAngles;
            gameData.players[0] = playerData;
        }

        gameDataList.Add(gameData);

        // Serialize to JSON
        string json = JsonUtility.ToJson(gameData, true);

        // Write to file
        File.WriteAllText(filePath, json);

        Debug.Log("Data saved to: " + filePath);
    }


}


[System.Serializable]
public class PlayerData
{
    public string playerName;
    public Vector3 BodyPosition;
    public Vector3 BodyRotation;
    public int playerState;
}

[System.Serializable]
public class GameData
{
    public float time;
    public bool isAr;
    public int gamestate;
    public PlayerData[] players;
}

