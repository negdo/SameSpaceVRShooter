using System.Collections;
using System.Collections.Generic;

using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelComponentSpawner : MonoBehaviour{
    [Header("Component prefabs")]
    [SerializeField] private GameObject WallPrefab;
    [SerializeField] private GameObject WeaponSpawnPointPrefab;
    [SerializeField] private GameObject TransparentWallPrefab;

    [Header("Other")]
    [SerializeField] private InputActionReference SpawnWallAction;
    [SerializeField] private MeshRenderer MeshRenderer;
    private int state = SpawningState.PointOne;
    private Vector3 PointOne;
    private GameObject LastSpawnedWall;
    private LevelEditorType LevelEditorType;

    private void Update() {
        if (GameOperator.Singleton.gameState.Value == State.Game){
            if (state != SpawningState.PointOne) {
                state = SpawningState.PointOne;
            }
            if (MeshRenderer.enabled) {
                MeshRenderer.enabled = false;
            }
            return;
        }

        if (!MeshRenderer.enabled) {
            MeshRenderer.enabled = true;
        }

        if (state == SpawningState.PointTwo) {
            Transform pointTwo = transform;
            Vector3 positionDifference = pointTwo.position - PointOne;
            float distance = positionDifference.magnitude;
            LastSpawnedWall.transform.localScale = new Vector3(distance, 1, 1);
            LastSpawnedWall.transform.LookAt(pointTwo);
            LastSpawnedWall.transform.Rotate(0, -90, 0);
        }
    }

    private void onSpawnWallAction(InputAction.CallbackContext obj) {
        if (GameOperator.Singleton.gameState.Value == State.Game) {
            return;
        }
        
        if (state == SpawningState.PointOne) {
            if (LevelEditorType == LevelEditorType.Wall) {
                SpawnWallServerRpc(transform.position);

            }else if (LevelEditorType == LevelEditorType.WeaponSpawn) {
                SpawnWeaponSpawnPointServerRpc(transform.position);

            } else if (LevelEditorType == LevelEditorType.TransparentWall) {
                state = SpawningState.PointTwo;
                PointOne = transform.position;
                LastSpawnedWall = Instantiate(TransparentWallPrefab, PointOne, Quaternion.identity);
            }
        } else if (state == SpawningState.PointTwo) {
            if (LevelEditorType == LevelEditorType.TransparentWall) {
                state = SpawningState.PointOne;
                Destroy(LastSpawnedWall);
                SpawnTransparentWallServerRpc(PointOne, transform.position);
            }   
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnWallServerRpc(Vector3 position) {
        Debug.Log("Spawning wall ServerRpc");
        GameObject spawnedWall = Instantiate(WallPrefab, position, Quaternion.identity);
        spawnedWall.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnWeaponSpawnPointServerRpc(Vector3 position) {
        GameObject spawnedWeaponSpawnPoint = Instantiate(WeaponSpawnPointPrefab, position, Quaternion.identity);
        spawnedWeaponSpawnPoint.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnTransparentWallServerRpc(Vector3 pointOne, Vector3 pointTwo) {
        GameObject spawnedTransparentWall = Instantiate(TransparentWallPrefab, pointOne, Quaternion.identity);
        spawnedTransparentWall.transform.LookAt(pointTwo);
        spawnedTransparentWall.transform.Rotate(0, -90, 0);
        spawnedTransparentWall.transform.localScale = new Vector3(Vector3.Distance(pointOne, pointTwo), 1, 1);
        spawnedTransparentWall.GetComponent<NetworkObject>().Spawn();

    }

    public void SetLevelEditorType(LevelEditorType type) {
        LevelEditorType = type;
        state = SpawningState.PointOne;
    }

    private void OnEnable() {
        SpawnWallAction.action.performed += onSpawnWallAction;
    }

    private void OnDisable() {
        if (state == SpawningState.PointTwo) {
            Destroy(LastSpawnedWall);
        }
        state = SpawningState.PointOne;
        SpawnWallAction.action.performed -= onSpawnWallAction;
    }
}

public class SpawningState {
    public static int PointOne = 1;
    public static int PointTwo = 2;
}
