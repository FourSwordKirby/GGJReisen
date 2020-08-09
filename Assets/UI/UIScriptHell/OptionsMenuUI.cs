using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuUI : MenuUI
{
    public Dropdown resolutionDropdown;

    public override void Init()
    {
        if (resolutionDropdown != null)
        {
            resolutionDropdown.Hide();
        }

        base.Init();
    }

    public override void Update()
    {
        if(Controls.cancelInputDown() && resolutionDropdown != null)
        {
            resolutionDropdown.Hide();
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
