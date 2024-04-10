using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReseter : MonoBehaviour
{
    // list of LevelEditorTypeButton
    [SerializeField] private List<LevelEditorTypeButton> levelEditorTypeButtons;

    public void ResetButtons(LevelEditorTypeButton buttonToIgnore) {
        foreach (LevelEditorTypeButton button in levelEditorTypeButtons) {
            if (button != buttonToIgnore) {
                button.Deselect();
            }
        }
    }
}
