using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraTransform : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
