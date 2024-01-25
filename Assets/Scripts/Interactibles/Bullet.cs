using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public NetworkVariable<float> speed = new NetworkVariable<float>();

    private float lifeTime = 3f;
    private float damage = 10f;
    private float explosionRadius = 0.1f;

    [SerializeField] private GameObject explosionHitPrefab;


    void Update()
    {
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }

        transform.Translate(GetMovement(Time.deltaTime));

        lifeTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet hit: " + other.gameObject.name);

        GameObject explosionHit = Instantiate(explosionHitPrefab, transform.position, Quaternion.identity);
        explosionHit.transform.localScale = Vector3.one * 0.02f;


        if (IsOwner)
        {
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



            gameObject.GetComponent<NetworkObject>().Despawn(true);
            Destroy(gameObject);
        }
    }


    public Vector3 GetMovement(float deltaTime)
    {
        return Vector3.forward * speed.Value * deltaTime;
    }
}
