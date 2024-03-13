using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadePaint : Granade {
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private GameObject explosionHitPrefab;
    [SerializeField] private LayerMask raycastLayerMask;

    protected override void Explode() {
        if (IsServer) {
            // Explosion visual effect
            GameObject explosion = NetworkObjectPool.Singleton.GetNetworkObject(explosionHitPrefab, transform.position, Quaternion.identity).gameObject;

            // Paint on players
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            HashSet<NetworkPlayer> hitPlayers = new HashSet<NetworkPlayer>();

            foreach (Collider hit in colliders) {
                NetworkPlayer hitPlayer = hit.gameObject.GetComponentInParent<NetworkPlayer>();
                if (hitPlayer != null) {

                    // try raycast from granade to hit collider
                    var ray = new Ray(transform.position, (hit.transform.position - transform.position).normalized);

                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo, explosionRadius, raycastLayerMask)) {
                        Debug.Log("original: " + hit.gameObject.name + " raycast: " + hitInfo.collider.gameObject.name);
                        if (hitInfo.collider == hit) {
                            hitPlayers.Add(hitPlayer);
                        }
                    }
                }
            }

            foreach (NetworkPlayer player in hitPlayers) {
                // Multiply y component by 0.5 as players head is far from the ground
                Vector3 scaledPlayerPos = player.transform.position;
                scaledPlayerPos.y *= 0.3f;
                Vector3 scaledGranadePos = transform.position;
                scaledGranadePos.y *= 0.3f;
                float distance = Vector3.Distance(scaledPlayerPos, scaledGranadePos);
                
                if (distance < explosionRadius) {
                    float calculated_damage = (1 - distance / explosionRadius) * damage;
                    player.PaintHit(gameObject.transform, calculated_damage);
                }
            }
        }
    }
}
