using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeDamage : Granade {

    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float damage = 100f;
    [SerializeField] private GameObject explosionHitPrefab;

    protected override void Explode() {
        if (IsServer) {
            // Explosion visual effect
            GameObject explosion = NetworkObjectPool.Singleton.GetNetworkObject(explosionHitPrefab, transform.position, Quaternion.identity).gameObject;

            // Explosion damage to players
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            HashSet<NetworkPlayer> hitPlayers = new HashSet<NetworkPlayer>();

            foreach (Collider hit in colliders) {
                NetworkPlayer hitPlayer = hit.gameObject.GetComponentInParent<NetworkPlayer>();
                if (hitPlayer != null) {
                    hitPlayers.Add(hitPlayer);
                }
            }

            foreach (NetworkPlayer player in hitPlayers) {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                float calculated_damage = Mathf.Lerp(damage, 0, distance / explosionRadius);

                player.BulletHit(gameObject.transform, damage);
            }
        }
    }
}
