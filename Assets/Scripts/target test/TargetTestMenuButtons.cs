using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTestMenuButtons : MonoBehaviour
{
    public void OnTest1ButtonClicked()
    {
        SceneLoader.testType = 0;
        SceneLoader.LoadTargetTest();
    }

    public void OnTest2ButtonClicked()
    {
        SceneLoader.testType = 1;
        SceneLoader.LoadTargetTest();
    }

    public void OnTest3ButtonClicked()
    {
        SceneLoader.testType = 2;
        SceneLoader.LoadTargetTest();
    }
}
