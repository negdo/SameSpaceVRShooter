using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Shotgun : PhysicsGrabable {
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float coneAngle = 30.0f;
    [SerializeField] private float coneDistance = 5.0f;
    [SerializeField] private float damage = 80.0f;
    [SerializeField] private GameObject explosionShot;

    public override void OnPrimaryAction() {
        if (!grabEnabled.Value || !isGrabbed.Value || !isGrabbedLocal || ownerClientId.Value != NetworkManager.Singleton.LocalClientId) {
            return; // check if isEnabled in parent class
        }

        if (PlayerLocalState.Singleton.GetPlayerState() != PlayerState.Alive) {
            return; // shooting only allowed when player is alive
        }

        ShotGunShotServerRpc(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShotGunShotServerRpc(Vector3 bulletSpawnPosition, Quaternion bulletSpawnRotation) {
        GameObject explosion = NetworkObjectPool.Singleton.GetNetworkObject(explosionShot, bulletSpawnPosition, bulletSpawnRotation).gameObject;

        // check collision in a cone in front of the player
        RaycastHit[] hits = Physics.SphereCastAll(bulletSpawnPosition, 0.5f, bulletSpawnRotation * Vector3.forward, coneDistance);

        HashSet<NetworkPlayer> hitPlayers = new HashSet<NetworkPlayer>();

        foreach (RaycastHit hit in hits) {
            NetworkPlayer player = hit.collider.gameObject.GetComponentInParent<NetworkPlayer>();

            if (player != null) {
                hitPlayers.Add(player);
            }
        }

        foreach (NetworkPlayer player in hitPlayers) {
            float d = Vector3.Distance(bulletSpawnPosition, player.transform.position) / coneDistance;

            player.BulletHit(gameObject.transform, damage * (1 - d));
        }


    }
}
