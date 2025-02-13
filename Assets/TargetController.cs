using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetController : MonoBehaviour {
    [SerializeField] private GameObject target;
    public Material[] materials;
    private float positionUp = 1.6f;
    private float positionDown = 0f;
    private float margin = 0.1f;
    public bool moveUp = false;
    private bool moveDown = false;
    private float speed = 0f;
    public bool isReady = true;
    TargetTestManager manager;

    public int color = 0;

    void Start() {
        manager = FindObjectOfType<TargetTestManager>();
    }


    public void targetUp() {
        moveUp = true;
        isReady = false;

        Renderer targetRenderer = target.GetComponent<Renderer>();
        color = Random.Range(0, materials.Length);
        Material randomMaterial = materials[color];
        Material[] currentMaterials = targetRenderer.materials;
        currentMaterials[0] = randomMaterial;
        targetRenderer.materials = currentMaterials;
    }

    public void targetDown() {
        moveDown = true;
        moveUp = false;
        isReady = false;
    }


    void OnEnable() {
    }

    public bool HitTarget() {
        return manager.HitTarget(this);
    }


    private void Update() {
        if (moveUp) {
            // time from last update
            speed += Time.deltaTime * 0.2f;

            // move target up
            target.transform.position = Vector3.Lerp(target.transform.position, new Vector3(transform.position.x, transform.position.y + positionUp + margin, transform.position.z), speed);

            if (target.transform.position.y >= positionUp + transform.position.y) {
                moveUp = false;
                speed = 0f;
            }
        }

        if (moveDown) {
            // time from last update
            speed += Time.deltaTime * 0.2f;

            // move target down
            target.transform.position = Vector3.Lerp(target.transform.position, new Vector3(target.transform.position.x, transform.position.y + positionDown - margin, target.transform.position.z), speed);

            if (target.transform.position.y <= positionDown + transform.position.y) {
                moveDown = false;
                isReady = true;
                speed = 0f;
            }
        }
    }


}
