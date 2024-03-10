using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponSpawnPoint : NetworkBehaviour {
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private float spawnTime = 5f;
    [SerializeField] private float speed = 2f;
    private float RandomSpeed = 0.3f;
    private float RandomAngularVelocity = 10f;
    private float RandomSpawnTime = 1f;
    private float timeToSpawn = 0f;

    private void Start() {
        resetTimer();
    }

    private void resetTimer() {
        timeToSpawn = spawnTime + Random.Range(-RandomSpawnTime, RandomSpawnTime);
    }

    private void Update() {
        if (!IsServer) {
            return;
        }

        if (GameOperator.Singleton.gameState.Value != State.Game) {
            return;
        }

        timeToSpawn -= Time.deltaTime;
        if (timeToSpawn <= 0) {
            SpawnWeapon();
            resetTimer();
        }
    }


    private void SpawnWeapon() {
        if (IsServer) {
            GameObject weaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
            GameObject spawnedObject = NetworkObjectPool.Singleton.GetNetworkObject(weaponPrefab, spawnPoint.position, Random.rotation).gameObject;

            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(Random.Range(-RandomSpeed, RandomSpeed), speed, Random.Range(-RandomSpeed, RandomSpeed));
            rb.angularVelocity = new Vector3(Random.Range(-RandomAngularVelocity, RandomAngularVelocity), Random.Range(-RandomAngularVelocity, RandomAngularVelocity), Random.Range(-RandomAngularVelocity, RandomAngularVelocity));
        }

    }

}


