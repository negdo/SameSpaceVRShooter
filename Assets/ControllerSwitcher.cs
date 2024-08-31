using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerSwitcher : MonoBehaviour {
    [SerializeField] private GameObject controller;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject instructions;

    // hide instructions input action reference
    [SerializeField] private InputActionReference hideInstructionsAction;
    
    private void Start() {
        controller.SetActive(true);
        instructions.SetActive(true);
        hand.SetActive(false);

        // hide instructions on input
        hideInstructionsAction.action.performed += _ => hideInstructions();

        // listen for gameoperator.gamestartevent
        GameOperator.Singleton.GameStartEvent.AddListener(SwitchToHand);
    }

    void OnDestroy() {
        hideInstructionsAction.action.performed -= _ => hideInstructions();
    }

    public void SwitchToHand() {
        controller.SetActive(false);
        instructions.SetActive(false);
        hand.SetActive(true);
    }

    public void hideInstructions() {
        instructions.SetActive(false);
    }
}