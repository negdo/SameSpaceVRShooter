using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTestManager : MonoBehaviour {
    private List<TargetController> targetControllers;
    private List<TargetController> upTargets = new List<TargetController>();

    private float delay = 0.3f;

    public int current_color = 0;
    // last color switch time
    private float last_color_switch_time = 0;
    private float min_tome_switch_color = 5;
    private float max_time_switch_color = 10;

    // list of text mesh pro ui objects
    public List<TMPro.TextMeshProUGUI> colorTexts = new List<TMPro.TextMeshProUGUI>();
    // List of game objects
    public List<GameObject> UITypes = new List<GameObject>();

    // dict of color int to color name
    public Dictionary<int, string> colorDict = new Dictionary<int, string> {
        {0, "Blue"},
        {1, "Orange"},
        {2, "Green"}
    };


    


    void Start() {
        /// only enable ui type matching sceneloader test type
        foreach (GameObject UIType in UITypes) {
            UIType.SetActive(false);
        }
        UITypes[SceneLoader.testType].SetActive(true);

        targetControllers = new List<TargetController>(FindObjectsOfType<TargetController>());
        StartCoroutine(StartTarget());

    }

    void switchColor() {
        // probability to switch color at 5 seconds is 0 and at 10 seconds is 1
        float probability = (Time.time - last_color_switch_time - min_tome_switch_color) / (max_time_switch_color - min_tome_switch_color);
        if (Random.value < probability) {
            current_color = Random.Range(0, 2);
            last_color_switch_time = Time.time;
            foreach (TMPro.TextMeshProUGUI text in colorTexts) {
                text.text = colorDict[current_color];
            }
        }
    }

    void popRandomTarget() {
        switchColor();

        // get all targets that are down and ready
        List<TargetController> readyTargets = new List<TargetController>();
        foreach (TargetController target in targetControllers) {
            if (target.isReady) {
                readyTargets.Add(target);
            }
        }

        // if there are any ready targets, pop up a random one
        // add it to the upTargets list
        if (readyTargets.Count > 0) {
            int randomIndex = Random.Range(0, readyTargets.Count);
            TargetController target = readyTargets[randomIndex];
            target.targetUp();
            upTargets.Add(target);
        }

        // filter out the targets that are not ready
        List<TargetController> filteredTargets = new List<TargetController>();
        foreach (TargetController target in upTargets) {
            if (!target.isReady) {
                filteredTargets.Add(target);
            }
        }
        upTargets = filteredTargets;


        // if there are more than 10 targets up, move some down
        while (upTargets.Count > 10) {
            int randomIndex = Random.Range(0, 5);
            TargetController target = upTargets[randomIndex];
            if (!target.moveUp) {
                target.targetDown();
                upTargets.Remove(target);
            }
        }
    }

    public bool HitTarget(TargetController target) {
        if (target.color == current_color) {
            Debug.Log("Hit the right target");
            target.targetDown();
            upTargets.Remove(target);
            return true;
        } else {
            Debug.Log("Hit the wrong target");
            return false;
        }
    }

    private IEnumerator StartTarget() {
        yield return new WaitForSeconds(delay);
        popRandomTarget();

        // after 2 seconds pop up the second target
        StartCoroutine(StartTarget());
    }




}
