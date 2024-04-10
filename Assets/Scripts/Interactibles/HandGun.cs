using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Assertions;

public class HandGun : PhysicsGrabable, IPooledObject {
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float timeBetweenShots = 0.3f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSourceGunShot;

    private float lastShotTime = 0f;

    public void Awake() {
        // check if bulletPrefab has a NetworkObject component
        Assert.IsNotNull(bulletPrefab.GetComponent<NetworkObject>(), $"{nameof(HandGun)}: bulletPrefab has no {nameof(NetworkObject)} component.");
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
        PlayAudioGunShotClientRpc();
        SpawnBulletServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(Vector3 bulletSpawnPosition, Quaternion bulletSpawnRotation) {
        GameObject spawnedObject = NetworkObjectPool.Singleton.GetNetworkObject(bulletPrefab, bulletSpawnPosition, bulletSpawnRotation).gameObject;
    }

    [ClientRpc]
    private void PlayAudioGunShotClientRpc() {
        audioSourceGunShot.PlayOneShot(audioSourceGunShot.clip);
    }

    public void ResetState() {
        // nothing to reset
    }

    public void UpdateState() {
        // nothing to update
    }
}
