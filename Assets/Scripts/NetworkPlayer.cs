using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] Transform head;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform body;

    // queue of last 10 head positions
    private Queue<Vector3> recentHeadPositions = new Queue<Vector3>();


    private float maxHealth = 100;
    public NetworkVariable<float> health = new NetworkVariable<float>();

    private void Start()
    {
        health.Value = 100;
    }

    void Update()
    {
        if (IsOwner)
        {
            root.position = VRRigReferences.Singelton.root.position;
            root.rotation = VRRigReferences.Singelton.root.rotation;

            head.position = VRRigReferences.Singelton.head.position;
            head.rotation = VRRigReferences.Singelton.head.rotation;

            leftHand.position = VRRigReferences.Singelton.leftHand.position;
            leftHand.rotation = VRRigReferences.Singelton.leftHand.rotation * Quaternion.Euler(45, 0, 0);

            rightHand.position = VRRigReferences.Singelton.rightHand.position;
            rightHand.rotation = VRRigReferences.Singelton.rightHand.rotation * Quaternion.Euler(45, 0, 0);

            recentHeadPositions.Enqueue(VRRigReferences.Singelton.head.position);
            if (recentHeadPositions.Count > 20)
            {
                recentHeadPositions.Dequeue();
            }

            // average head position
            Vector3 averageHeadPosition = Vector3.zero;
            foreach (Vector3 position in recentHeadPositions)
            {
                averageHeadPosition += position;
            }
            averageHeadPosition /= recentHeadPositions.Count;

            body.position = averageHeadPosition - new Vector3(0, 0.4f, 0);
            body.rotation = Quaternion.Euler(0, VRRigReferences.Singelton.head.rotation.eulerAngles.y, 0);

        }

        if (health.Value <= 0)
        {
            Debug.Log("Player died");
        }
    }

    public void BulletHit(Transform transform, float damage)
    {
        // shake camera


        // remove health
        if (IsOwner)
        {
            health.Value -= damage;
            Debug.Log("Health: " + health.Value);
        }
    }

    public float GetHealth()
    {
        return health.Value;
    }
}


