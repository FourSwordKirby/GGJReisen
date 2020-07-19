using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsUIElement : MenuUIElement
{
    public GameObject tabTitleDropShadow;

    public override void Blur()
    {
        tabTitleDropShadow.SetActive(false);
    }

    public override void Focus()
    {
        tabTitleDropShadow.SetActive(true);
    }
}
