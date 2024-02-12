using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Assertions;

public class HandGun : PhysicsGrabable
{
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed;

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

        // Get delay of the server
        ulong clientId = NetworkManager.Singleton.LocalClientId;
        float serverDelay = NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(clientId) / 1000.0f;

        Transform compensatedBulletSpawnPoint = bulletSpawnPoint;
        compensatedBulletSpawnPoint.Translate(Vector3.forward * bulletSpeed * serverDelay);
        
        SpawnBulletServerRpc(compensatedBulletSpawnPoint.position, compensatedBulletSpawnPoint.rotation, bulletSpeed);
    }


    [ServerRpc(RequireOwnership = false)]
    private void SpawnBulletServerRpc(Vector3 bulletSpawnPosition, Quaternion bulletSpawnRotation, float bulletSpeed) {
        GameObject spawnedObject = NetworkObjectPool.Singleton.GetNetworkObject(bulletPrefab, bulletSpawnPosition, bulletSpawnRotation).gameObject;
    }
}
