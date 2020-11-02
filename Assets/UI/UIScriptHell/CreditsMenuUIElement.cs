using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsMenuUIElement : MenuUIElement
{
    public TextMeshProUGUI textbackdrop;
    public Image creditPreviewImage;

    public void SetText(string content)
    {
        text.text = content;
        textbackdrop.text = content;
    }

    public void SetPreviewImage(Sprite sprite)
    {
        creditPreviewImage.sprite = sprite;
    }
}
