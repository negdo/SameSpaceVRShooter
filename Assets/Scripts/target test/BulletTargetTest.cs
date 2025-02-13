using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTargetTest : MonoBehaviour {
    [SerializeField] float speed = 10f;

    // explosion effect prefab
    [SerializeField] GameObject explosionEffect;
    private float lifeTime = 2f;


    private void Update() {
        if (lifeTime <= 0) {
            Destroy(gameObject);
        }
        lifeTime -= Time.deltaTime;

        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Finish") {
            Debug.Log("Hit the target");
            // get parent object
            GameObject parent = other.transform.parent.gameObject;
            TargetController target = parent.GetComponent<TargetController>();
            if (target.HitTarget()) {
                // create explosion effect
                GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(explosion, 1f);
            }
        }
        Destroy(gameObject);
    }

}
