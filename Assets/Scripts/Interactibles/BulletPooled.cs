using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

interface IMovement {
    Vector3 GetMovement(float deltaTime);
}

public class BulletPooled : NetworkBehaviour, IPooledObject, IMovement {
    [SerializeField] float speed = 10f;
    private float maxLifeTime = 2f;
    private float lifeTime = 2f;
    private float damage = 10f;
    private float explosionRadius = 0.1f;

    [SerializeField] private GameObject explosionHitPrefab;

    private NetworkPooledObject networkPooledObject;

    private void Start() {
        networkPooledObject = gameObject.GetComponent<NetworkPooledObject>();
    }

    public void ResetState() {
        lifeTime = maxLifeTime;
    }

    public void UpdateState() {
        if (lifeTime <= 0) {
            networkPooledObject.ReturnToPool();
        }
        lifeTime -= Time.deltaTime;

        transform.Translate(GetMovement(Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other) {
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
                player.BulletHit(gameObject.transform, damage);
            }

            networkPooledObject.ReturnToPool();
        }
    }

    public Vector3 GetMovement(float deltaTime) {
        return Vector3.forward * speed * deltaTime;
    }

}

