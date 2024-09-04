using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ExplosionPooled : MonoBehaviour, IPooledObject {
    private float maxLifeTime = 1f;
    private float lifeTime = 1f;
    private NetworkPooledObject networkPooledObject;

    private void OnEnable() {
        networkPooledObject = gameObject.GetComponent<NetworkPooledObject>();
        gameObject.GetComponent<ParticleSystem>().Play();
    }

    public void ResetState() {
        // reset the life time
        lifeTime = maxLifeTime;
        // start the explosion animation
        gameObject.GetComponent<ParticleSystem>().Play();
    }

    public void UpdateState() {
        if (lifeTime <= 0){
            networkPooledObject.ReturnToPool();
        }
        lifeTime -= Time.deltaTime;
    }
}
