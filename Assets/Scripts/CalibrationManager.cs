using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CalibrationManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;

    [SerializeField] InputActionReference CalibrationRightReference;
    [SerializeField] InputActionReference CalibrationLeftReference;
    [SerializeField] InputActionReference CalibrationFloorReference;

    private bool rightCalibrationPressed = false;
    private bool leftCalibrationPressed = false;

    private void Awake()
    {
        CalibrationRightReference.action.performed += CalibrateRightPressed;
        CalibrationLeftReference.action.performed += CalibrateLeftPressed;
        CalibrationLeftReference.action.canceled += CalibrateLeftReleased;
        CalibrationRightReference.action.canceled += CalibrateRightReleased;
        CalibrationFloorReference.action.performed += CalibrateHeight;
    }

    private void OnDestroy()
    {
        CalibrationRightReference.action.performed -= CalibrateRightPressed;
        CalibrationLeftReference.action.performed -= CalibrateLeftPressed;
        CalibrationLeftReference.action.canceled -= CalibrateLeftReleased;
        CalibrationRightReference.action.canceled -= CalibrateRightReleased;
        CalibrationFloorReference.action.performed -= CalibrateHeight;
    }

    private void CalibrateRightPressed(InputAction.CallbackContext obj)
    {
        rightCalibrationPressed = true;
        checkCalibration();
    }

    private void CalibrateLeftPressed(InputAction.CallbackContext obj)
    {
        leftCalibrationPressed = true;
        checkCalibration();
    }

    private void CalibrateLeftReleased(InputAction.CallbackContext obj)
    {
        leftCalibrationPressed = false;
    }

    private void CalibrateRightReleased(InputAction.CallbackContext obj)
    {
        rightCalibrationPressed = false;
    }

    private void checkCalibration()
    {
        if (rightCalibrationPressed && leftCalibrationPressed)
        {
            for (int i = 0; i < 5; i++)
            {
                CalibratePlayerLocation_two_points();
            }
        }
    }

    private void CalibratePlayerLocation_two_points()
    {
        // Get the center of the two controllers
        Vector3 CenterPosition = (leftController.transform.position + rightController.transform.position) / 2;
        CenterPosition.y = 0;

        // Move the player to the center of the room
        player.transform.position -= CenterPosition;

        // Get new rotation of the room
        Vector3 vecBetweenControlers = rightController.transform.position - leftController.transform.position;
        vecBetweenControlers.y = 0;
        vecBetweenControlers = vecBetweenControlers.normalized;

        // New vector is perpendicular to the vector betweem contollers and the y axis
        Vector3 newDirection = Vector3.Cross(vecBetweenControlers, Vector3.up);
        Quaternion newRotation = Quaternion.LookRotation(newDirection, Vector3.up);

        // Set rotation to the difference between the current rotation and the new rotation
        float angle = player.transform.rotation.eulerAngles.y - newRotation.eulerAngles.y;
        newRotation = Quaternion.Euler(0, angle, 0);

        // Rotate the player
        player.transform.rotation = newRotation;
    }

    private void CalibrateHeight(InputAction.CallbackContext obj)
    {
        Debug.Log("CalibrateHeight");
        // get position of right controller
        Vector3 rightControllerPosition = rightController.transform.position;

        // set y position of player to y position of right controller
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - rightControllerPosition.y + 0.05f, player.transform.position.z);
    }
}
