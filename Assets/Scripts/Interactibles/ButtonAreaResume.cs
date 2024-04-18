using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAreaResume : ButtonArea
{
    [SerializeField] private PauseMenu pauseMenu;

    protected override void OnButtonActionHand() {
        pauseMenu.OnSettingsButton();
    }

    protected override void OnButtonAction() {}
}
