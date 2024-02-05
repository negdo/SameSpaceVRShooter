using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPooled : NetworkPooledObject {
    private float maxLifeTime = 1f;
    private float lifeTime = 1f;

    protected override void ResetState() {
        // reset the life time
        lifeTime = maxLifeTime;
        // start the explosion animation
        gameObject.GetComponent<ParticleSystem>().Play();
    }

    protected override void UpdateState() {
        if (lifeTime <= 0){
            ReturnToPool();
        }
        lifeTime -= Time.deltaTime;
    }
}
