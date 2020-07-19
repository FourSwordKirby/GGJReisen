using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuUI : MenuUI
{
    public void ReturnToTitle()
    {
        ReisenGameManager.instance.ReturnToTitle();
    }

    public void QuitGame()
    {
        ReisenGameManager.instance.QuitGame();
    }
}
