using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LevelComponentSpawnerSecond : NetworkBehaviour {
    [SerializeField] private GameObject object1Prefab;
    [SerializeField] private GameObject object2Prefab;
    [SerializeField] private GameObject object3Prefab;
    [SerializeField] private GameObject object4Prefab;

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
        GameObject spawnedObject = Instantiate(object1Prefab, position, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObject2ServerRpc(Vector3 position) {
        GameObject spawnedObject = Instantiate(object2Prefab, position, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObject3ServerRpc(Vector3 position1, Vector3 position2) {
        GameObject spawnedObject = Instantiate(object3Prefab, position1, Quaternion.identity);
        spawnedObject.transform.LookAt(position2);
        spawnedObject.transform.Rotate(0, -90, 0);
        spawnedObject.transform.localScale = new Vector3(Vector3.Distance(position1, position2), 1, 1);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnObject4ServerRpc(Vector3 position) {
        GameObject spawnedObject = Instantiate(object4Prefab, position, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }


}
