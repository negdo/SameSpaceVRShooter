using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorTypeButton : ButtonArea {
    [SerializeField] private LevelEditorType levelEditorType;
    [SerializeField] private LevelComponentSpawner ComponentSpawner;
    private Image image;

    private Color defaultColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    private Color selectedColor = new Color(0.4f, 0.4f, 0.4f, 1f);

    private void Start() {
        image = GetComponent<Image>();
        image.color = defaultColor;
    }

    public void Deselect() {
        image.color = defaultColor;
    }

    protected override void OnButtonAction() {
        image.color = selectedColor;
        ComponentSpawner.SetLevelEditorType(levelEditorType);
        ButtonReseter buttonReseter = GetComponentInParent<ButtonReseter>();
        buttonReseter.ResetButtons(this);
    }


    
}

public enum LevelEditorType {
    Wall,
    TransparentWall,
    WeaponSpawn
}
