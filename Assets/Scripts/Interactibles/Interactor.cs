using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;

public class Interactor : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference grabAction;
    [SerializeField] private InputActionReference releaseAction;
    [SerializeField] private InputActionReference primaryAction;
    [SerializeField] private InputActionReference secondaryAction;

    [Header("Grab Settings")]
    [SerializeField] private float grabRadius = 0.1f;




    private bool isGrabbing = false;
    private Grabable lastGrabable = null;

    private void OnTriggerEnter(Collider other)
    {
        if (!isGrabbing)
        {
            Grabable grabable = other.GetComponent<Grabable>();
            if (grabable != null)
            {
                HapticFeedbackInArea();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isGrabbing)
        {
            Grabable grabable = other.GetComponent<Grabable>();
            if (grabable != null)
            {
                HapticFeedbackInArea();
            }
        }
    }

    private void OnGrabAction()
    {
        // get colliders in area
        Collider[] colliders = Physics.OverlapSphere(transform.position, grabRadius);
        foreach (Collider collider in colliders)
        {
            Grabable grabable = collider.GetComponent<Grabable>();
            if (grabable != null)
            {
                if (grabable.OnGrab(gameObject))
                {
                    lastGrabable = grabable;
                    isGrabbing = true;
                    HapticFeedbackInArea();
                    break;
                }
            }
        }
    }

    private void OnReleaseAction()
    {
        if (isGrabbing)
        {
            lastGrabable.OnRelease();
            isGrabbing = false;
        }
    }

    private void OnPrimaryAction()
    {
        if (isGrabbing)
        {
            lastGrabable.OnPrimaryAction();
        }
    }

    private void OnSecondaryAction()
    {
        if (isGrabbing)
        {
            lastGrabable.OnSecondaryAction();
        }
    }

    private void HapticFeedbackInArea()
    {
        float intensity = 0.5f;
        float duration = 0.1f;
        float frequency = 0.5f;
    }

    private void Awake()
    {
        // set action callbacks
        grabAction.action.performed += _ => OnGrabAction();
        releaseAction.action.performed += _ => OnReleaseAction();
        primaryAction.action.performed += _ => OnPrimaryAction();
        secondaryAction.action.performed += _ => OnSecondaryAction();

    }

}
