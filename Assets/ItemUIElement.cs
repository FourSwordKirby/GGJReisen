using Assets.PodgeStandardLibrary.RPGSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemUIElement : MenuUIElement
{
    public PauseMenuUI pauseMenuUI;
    public KeyItem itemData;

    public override void Blur()
    {
        text.color = Color.black;
    }

    public override void Focus()
    {
        text.color = Color.white;
        pauseMenuUI.ShowDescription(itemData.ItemDescription);
    }
}
