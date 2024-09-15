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
    


    private void Awake() {
        Singelton = this;
    }

    void Start() {
        int initState = SceneLoader.isSpectator ? PlayerState.Spectating : PlayerState.NotReady;

        if (initState == PlayerState.Spectating) {
            Transform newTransform = new GameObject().transform;
            newTransform.position = new Vector3(0, -100, 0);

            root = newTransform;
            head = newTransform;
            leftHand = newTransform;
            rightHand = newTransform;
            leftHandPhysics = newTransform;
            rightHandPhysics = newTransform;
        }
    }
}
