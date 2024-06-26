using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Shotgun : PhysicsGrabable, IPooledObject {
    [SerializeField] private int numberOfBullets = 5;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float coneAngle = 15.0f;
    [SerializeField] private float timeBetweenShots = 0.7f;

    AudioPlayer audioPlayer;


    private float lastShotTime = 0f;

    void Start() {
        audioPlayer = GetComponent<AudioPlayer>();
    }


    public override void OnPrimaryAction() {
        if (!grabEnabled.Value || !isGrabbed.Value || !isGrabbedLocal || ownerClientId.Value != NetworkManager.Singleton.LocalClientId) {
            return; // check if isEnabled in parent class
        }

        if (PlayerLocalState.Singleton.GetPlayerState() != PlayerState.Alive) {
            return; // shooting only allowed when player is alive
        }

        if (Time.time - lastShotTime < timeBetweenShots) {
            return; // prevent shooting too fast
        }

        lastShotTime = Time.time;
        audioPlayer.playAudio();
        ShotGunShotServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShotGunShotServerRpc(Vector3 bulletSpawnPosition, Quaternion bulletSpawnRotation) {
        for (int i = 0; i < numberOfBullets; i++) {
            Vector3 direction = bulletSpawnRotation * Quaternion.Euler(Random.Range(-coneAngle, coneAngle), Random.Range(-coneAngle, coneAngle), 0) * Vector3.forward;
            GameObject spawnedObject = NetworkObjectPool.Singleton.GetNetworkObject(bulletPrefab, bulletSpawnPosition, Quaternion.LookRotation(direction)).gameObject;
        }
    }

    public void ResetState() {
        // nothing to reset
    }

    public void UpdateState() {
        // nothing to update
    }
}
