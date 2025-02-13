using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ARSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject SkyPanels;
    [SerializeField] private Material FloorMaterial;
    // Start is called before the first frame update
    public void SetAR(bool ar) {
        SkyPanels.SetActive(ar);
        // set material attribute AR to ar
        FloorMaterial.SetInt("_AR", ar ? 1 : 0);
    }

}
