using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetTestShooter : MonoBehaviour
{
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private GameObject bulletPrefab;

    private void OnEnable()
    {
        shootAction.action.Enable();
        shootAction.action.performed += Shoot;
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
        shootAction.action.performed -= Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
