using Assets.PodgeStandardLibrary.RPGSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIElement : MenuUIElement
{
    public PauseMenuUI pauseMenuUI;
    public KeyItem itemData;

    public TextMeshProUGUI ItemNameText;
    public Image ItemImage;

    public List<Sprite> itemSprites;

    public void InitAppearance()
    {
        Debug.Log(itemData.ItemName);
        ItemNameText.text = itemData.ItemName;

        if(itemData.ItemName == "History Book")
            ItemImage.sprite = itemSprites[0];
        else if(itemData.ItemName == "Encyclopeida")
            ItemImage.sprite = itemSprites[1];
        else if (itemData.ItemName == "Novel")
            ItemImage.sprite = itemSprites[2];
        else if (itemData.ItemName == "Newspaper")
            ItemImage.sprite = itemSprites[3];
        else if (itemData.ItemName == "Magazine")
            ItemImage.sprite = itemSprites[4];
        else if (itemData.ItemName == "Schematic")
            ItemImage.sprite = itemSprites[5];
        else if (itemData.ItemName == "Cough Medicine")
            ItemImage.sprite = itemSprites[6];
        else if (itemData.ItemName == "Smartphone")
            ItemImage.sprite = itemSprites[7];
        else if (itemData.ItemName == "Wrench")
            ItemImage.sprite = itemSprites[8];
        else if (itemData.ItemName == "Scroll")
            ItemImage.sprite = itemSprites[9];
        else if (itemData.ItemName == "Elixir")
            ItemImage.sprite = itemSprites[10];
    }

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
