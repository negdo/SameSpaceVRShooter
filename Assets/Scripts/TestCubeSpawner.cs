using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestCubeSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private InputActionReference spawnAction;

    private void Awake()
    {
        spawnAction.action.performed += OnSpawnAction;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        spawnAction.action.performed -= OnSpawnAction;
    }


    private void OnSpawnAction(InputAction.CallbackContext obj)
    {
        SpawnCubeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnCubeServerRpc()
    {
        GameObject spawnedObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }
}
