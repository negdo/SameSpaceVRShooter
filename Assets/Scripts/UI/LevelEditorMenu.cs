using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelEditorMenu : MonoBehaviour {
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject Cursor;
    [SerializeField] private GameObject SpawnPoint;
    [SerializeField] private InputActionReference openMenu;
    private bool menuOpen = false;

    void Start() {
        Menu.SetActive(false);
        Cursor.SetActive(false);
        SpawnPoint.SetActive(false);
    }

    private void Awake() {
        openMenu.action.performed += ToggleMenu;
    }

    private void OnDestroy() {
        openMenu.action.performed -= ToggleMenu;
    }

    void ToggleMenu(InputAction.CallbackContext obj) {
        if (GameOperator.Singleton.gameState.Value == State.Game) {
            menuOpen = false;
        } else {
            menuOpen = !menuOpen;
        }
        Menu.SetActive(menuOpen);
        Cursor.SetActive(menuOpen);
        SpawnPoint.SetActive(menuOpen);
    }
}
