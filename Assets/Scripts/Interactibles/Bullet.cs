using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public NetworkVariable<float> speed = new NetworkVariable<float>();

    private float lifeTime = 3f;


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


        if (IsOwner)
        {
            gameObject.GetComponent<NetworkObject>().Despawn(true);
            Destroy(gameObject);
        }
    }

    public Vector3 GetMovement(float deltaTime)
    {
        return Vector3.forward * speed.Value * deltaTime;
    }
}
