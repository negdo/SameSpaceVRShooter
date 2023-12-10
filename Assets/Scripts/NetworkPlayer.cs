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
        }
    }
}


