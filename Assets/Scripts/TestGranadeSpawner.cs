using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestGranadeSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private InputActionReference spawnAction;


    private void Awake() {
        spawnAction.action.performed += OnSpawnAction;
    }

    public override void OnDestroy() {
        spawnAction.action.performed -= OnSpawnAction;
        base.OnDestroy();
    }

    private void OnSpawnAction(InputAction.CallbackContext obj) {
        SpawnCubeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnCubeServerRpc() {
        GameObject spawnedObject = NetworkObjectPool.Singleton.GetNetworkObject(objectPrefab, transform.position, Quaternion.identity).gameObject;
    }

}
