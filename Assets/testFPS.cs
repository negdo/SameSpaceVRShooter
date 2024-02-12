using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testFPS : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI fpstext;

    private float lastFrameTime = 0;

    private void Update()
    {
        float t = Time.time;
        float deltaTime = t - lastFrameTime;
        float fps = 1 / deltaTime;
        lastFrameTime = t;
        fpstext.text = Mathf.Ceil(fps).ToString();
    }

}
