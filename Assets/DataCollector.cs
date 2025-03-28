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
    GameOperator gameOperator;
    private GameDataWrapper gameDataWrapper;
    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/gamedata-" +System.DateTime.Now.ToString("MM-dd-HH-mm-ss") + ".json";
        gameDataWrapper = new GameDataWrapper();
        gameOperator = FindObjectOfType<GameOperator>();
        StartCoroutine(SaveDataRoutine());

    }

    IEnumerator SaveDataRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            SaveData();
        }
    }

     void SaveData()
    {
        NetworkPlayer[] players = FindObjectsOfType<NetworkPlayer>();
        // Update data (example: increasing score)
        gameData = new GameData();
        gameData.players = new PlayerData[players.Length];
        gameData.time = Time.time;
        gameData.isAr = ! SkyPanels.activeSelf;
        gameData.gamestate = gameOperator.gameState.Value;
        

        for (int i = 0; i < players.Length; i++) {   
            NetworkPlayer player = players[i];
            // get child component named "Body" from player
            GameObject body = player.transform.Find("Body").gameObject;
            playerData = new PlayerData
            {
                playerName = player.playerName.Value.ToString(),
                BodyPosition = body.transform.position,
                BodyRotation = body.transform.rotation.eulerAngles,
                playerState = player.state.Value
            };
            gameData.players[i] = playerData;
        }

        gameDataWrapper.gameDataList.Add(gameData);

        // Serialize to JSON
        string json = JsonUtility.ToJson(gameDataWrapper, true);

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

[System.Serializable]
public class GameDataWrapper {
    public List<GameData> gameDataList;

    public GameDataWrapper() {
        gameDataList = new List<GameData>();
    }
}


[System.Serializable]
public class ActivitiyData
{
    public string playerName;
    public string action;
    public float time;
}

[System.Serializable]
public class ActivityDataWrapper {
    public List<ActivitiyData> activityDataList;

    public ActivityDataWrapper() {
        activityDataList = new List<ActivitiyData>();
    }
}