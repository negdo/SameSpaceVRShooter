using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private NetworkVariable<float> speed = new NetworkVariable<float>(10f);

    private float lifeTime = 3f;
    private float damage = 10f;
    private float explosionRadius = 0.1f;

    [SerializeField] private int selfPrefabPoolNumber;
    [SerializeField] private GameObject explosionHitPrefab;

    private void Start() {
        // for network object pool
        if (IsClient) {
            gameObject.SetActive(false);
        }
    }

    public void OnEnable() {
        // reset status when bullet is enabled from pool
        if (IsServer) {
            lifeTime = 3f;
        }
    }


    void Update() {
        if (IsServer) {
            if (lifeTime <= 0) {
                DestroyBullet();
            }
            lifeTime -= Time.deltaTime;

            transform.Translate(GetMovement(Time.deltaTime));
        }
    }

    void DestroyBullet() {
        // return bullet to pool
        NetworkObjectPool.Singleton.ReturnNetworkObject(gameObject.GetComponent<NetworkObject>(), selfPrefabPoolNumber);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Bullet hit: " + other.gameObject.name);

        GameObject explosionHit = Instantiate(explosionHitPrefab, transform.position, Quaternion.identity);
        explosionHit.transform.localScale = Vector3.one * 0.13f;

        if (IsServer) {

            
            // check all colliders in radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            HashSet<NetworkPlayer> hitPlayers = new HashSet<NetworkPlayer>();


            foreach (Collider collider in colliders)
            {
                // check if collider is a player
                NetworkPlayer networkPlayer = collider.gameObject.GetComponentInParent<NetworkPlayer>();
                if (networkPlayer != null)
                {
                    // add player to set of hit players
                    hitPlayers.Add(networkPlayer);
                }
            }

            foreach (NetworkPlayer networkPlayer in hitPlayers)
            {
                networkPlayer.BulletHit(gameObject.transform, damage);
            }

            DestroyBullet();
        }
    }


    private Vector3 GetMovement(float deltaTime)
    {
        return Vector3.forward * speed.Value * deltaTime;
    }

    [ClientRpc]
    public void EnableOnClientRpc()
    {
        gameObject.SetActive(true);
    }

    [ClientRpc]
    public void DisableOnClientRpc()
    {
        gameObject.SetActive(false);
    }
}
