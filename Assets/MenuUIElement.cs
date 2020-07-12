using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUIElement : ControllableGridMenuElement
{
    public TextMeshProUGUI text;

    public Color focusColor;
    public Color blurColor;

    public override void Blur()
    {
        text.color = blurColor;
    }

    public override void Focus()
    {
        text.color = focusColor;
    }
}
