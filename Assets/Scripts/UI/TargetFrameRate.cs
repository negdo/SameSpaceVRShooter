using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TargetFrameRate : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        float[] rates;
        Unity.XR.Oculus.Performance.TryGetAvailableDisplayRefreshRates(out rates);
        // log the available refresh rates
        foreach (var r in rates)
        {
            Debug.Log("Available refresh rate: " + r);
        }
        
        Unity.XR.Oculus.Performance.TrySetDisplayRefreshRate(90);
        
        float rate;
        Unity.XR.Oculus.Performance.TryGetDisplayRefreshRate(out rate);

        Debug.Log("Current refresh rate: " + rate);
    }

    // Update is called once per frame

}
