using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRigReferences : MonoBehaviour
{

    public static VRRigReferences Singelton;

    public Transform root;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Transform leftHandPhysics;
    public Transform rightHandPhysics;
    


    private void Awake()
    {
        Singelton = this;
    }
}
