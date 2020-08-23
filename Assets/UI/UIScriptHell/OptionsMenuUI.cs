using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsMenuUI : MenuUI
{
    public Dropdown resolutionDropdown;

    public override void Open()
    {
        //if (resolutionDropdown != null)
        //{
        //    resolutionDropdown.OnDeselect(new BaseEventData(EventSystem.current));
        //}

        base.Open();
    }

    public override void Update()
    {
        if(Controls.cancelInputDown() && GameObject.Find("Dropdown List"))
        {
            AudioMaster.instance.PlayCancelSfx();
            resolutionDropdown.Hide();
            return;
        }

        base.Update();
    }

    public void ReturnToTitle()
    {
        ReisenGameManager.instance.ReturnToTitle();
    }

    public void QuitGame()
    {
        ReisenGameManager.instance.QuitGame();
    }
}
