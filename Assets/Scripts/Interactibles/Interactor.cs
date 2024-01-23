using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.OpenXR.Input;

public class Interactor : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private InputActionReference grabAction;
    [SerializeField] private InputActionReference primaryAction;
    [SerializeField] private InputActionReference secondaryAction;

    [Header("Grab Settings")]
    [SerializeField] private float grabRadius = 3.0f;

    [SerializeField] private XRBaseController controllerDevice;




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
                HapticFeedbackOutArea();
            }
        }
    }

    private void OnGrabAction()
    {
        print("Grab action");
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
                    HapticFeedbackGrab();
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
        if (isGrabbing) { lastGrabable.OnPrimaryAction(); }
    }

    private void OnSecondaryAction()
    {
        if (isGrabbing) { lastGrabable.OnSecondaryAction(); }
    }


    // Haptic Feedback to controller
    private void HapticFeedbackInArea()
    {
        float intensity = 0.5f;
        float duration = 0.1f;

        controllerDevice.SendHapticImpulse(intensity, duration);
    }

    private void HapticFeedbackOutArea()
    {
        float intensity = 0.2f;
        float duration = 0.1f;

        controllerDevice.SendHapticImpulse(intensity, duration);
    }

    private void HapticFeedbackGrab()
    {
        float intensity = 1.0f;
        float duration = 0.2f;

        controllerDevice.SendHapticImpulse(intensity, duration);
    }

    private void Awake()
    {
        grabAction.action.performed += _ => OnGrabAction();
        grabAction.action.canceled += _ => OnReleaseAction();
        primaryAction.action.performed += _ => OnPrimaryAction();
        secondaryAction.action.performed += _ => OnSecondaryAction();
    }
}
