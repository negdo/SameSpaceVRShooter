using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class LevelComponentSpawnerSecond : NetworkBehaviour {
    [SerializeField] private GameObject object1Prefab;
    [SerializeField] private int object1MaxCount = 100;
    [SerializeField] private GameObject object2Prefab;
    [SerializeField] private int object2MaxCount = 100;
    [SerializeField] private GameObject object3Prefab;
    [SerializeField] private int object3MaxCount = 100;
    [SerializeField] private GameObject object4Prefab;
    [SerializeField] private int object4MaxCount = 100;

    private int count(GameObject objectPrefab) {
        string prefix = objectPrefab.name;
        GameObject[] objects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        int count = 0;
        foreach (GameObject obj in objects) {
            if (obj.name.StartsWith(prefix)) {
                count++;
            }
        }
        return count;
    }

    public void SpawnObject1(Vector3 position) {
        SpawnObject1ServerRpc(position);
    }

    public void SpawnObject2(Vector3 position) {
        SpawnObject2ServerRpc(position);
    }

    public void SpawnObject3(Vector3 position1, Vector3 position2) {
        SpawnObject3ServerRpc(position1, position2);
    }

    public void SpawnObject4(Vector3 position) {
        SpawnObject4ServerRpc(position);
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnObject1ServerRpc(Vector3 position) {
        if (count(object1Prefab) >= object1MaxCount) {
            return;
        }
        GameObject spawnedObject = Instantiate(object1Prefab, position, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObject2ServerRpc(Vector3 position) {
        if (count(object2Prefab) >= object2MaxCount) {
            return;
        }
        GameObject spawnedObject = Instantiate(object2Prefab, position, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObject3ServerRpc(Vector3 position1, Vector3 position2) {
        if (count(object3Prefab) >= object3MaxCount) {
            return;
        }
        GameObject spawnedObject = Instantiate(object3Prefab, position1, Quaternion.identity);
        spawnedObject.transform.LookAt(position2);
        spawnedObject.transform.Rotate(0, -90, 0);
        spawnedObject.transform.localScale = new Vector3(Vector3.Distance(position1, position2), 1, 1);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObject4ServerRpc(Vector3 position) {
        if (count(object4Prefab) >= object4MaxCount) {
            return;
        }
        GameObject spawnedObject = Instantiate(object4Prefab, position, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }


}
