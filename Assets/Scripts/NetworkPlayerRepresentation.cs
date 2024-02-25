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

    void Update() {
        if (IsOwner) {
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


            body.position = averageHeadPosition - new Vector3(0, 0.2f, 0);
            body.rotation = Quaternion.Euler(0, VRRigReferences.Singelton.head.rotation.eulerAngles.y, 0);
        }
    }
}
