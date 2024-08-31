using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerRepresentation : NetworkBehaviour {
    [SerializeField] Transform root;
    [SerializeField] Transform head;
    [SerializeField] Transform leftHand;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform body;
    private Queue<Vector3> recentHeadPositions = new Queue<Vector3>();
    bool skipUpdates = false;

    public void SetSpectator() {
        head.gameObject.SetActive(false);
        leftHand.gameObject.SetActive(false);
        rightHand.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
        skipUpdates = true;
    }

    void Update() {
        if (IsOwner && !skipUpdates) {
            root.position = VRRigReferences.Singelton.root.position;
            root.rotation = VRRigReferences.Singelton.root.rotation;

            head.position = VRRigReferences.Singelton.head.position;
            head.rotation = VRRigReferences.Singelton.head.rotation;

            leftHand.position = VRRigReferences.Singelton.leftHandPhysics.position;
            leftHand.rotation = VRRigReferences.Singelton.leftHand.rotation * Quaternion.Euler(45, 0, 0);

            rightHand.position = VRRigReferences.Singelton.rightHandPhysics.position;
            rightHand.rotation = VRRigReferences.Singelton.rightHand.rotation * Quaternion.Euler(45, 0, 0);

            recentHeadPositions.Enqueue(VRRigReferences.Singelton.head.position);
            if (recentHeadPositions.Count > 20) {
                recentHeadPositions.Dequeue();
            }

            // average head position
            Vector3 averageHeadPosition = Vector3.zero;
            foreach (Vector3 position in recentHeadPositions) {
                averageHeadPosition += position;
            }
            averageHeadPosition /= recentHeadPositions.Count;

            // add vector in direction of head down to get neck pivot position
            Vector3 neckPivotPosition = averageHeadPosition + head.up * -0.1f;
            
            body.position = neckPivotPosition + Vector3.up * -0.1f;
            body.rotation = Quaternion.Euler(0, VRRigReferences.Singelton.head.rotation.eulerAngles.y, 0);
            body.Translate(body.forward * -0.1f, Space.World);
        }
    }
}
