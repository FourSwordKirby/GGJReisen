using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShardMenuUIElement : MenuUIElement
{
    private bool revealed;
    public bool IsRevealed
    {
        get
        {
            return revealed;
        }
        set
        {
            ShardImage.color = value ? FoundColor : MissingColor;
            revealed = value;
        }
    }

    public Color FoundColor;
    public Color MissingColor;

    public Sprite singleShardSprite;
    public Sprite doubleShardSprite;

    public Image ShardImage;
    public Image ShardSelectionHighlight;

    public bool isPauseMenuVersion;

    public ShardMenuUI shardMenuUI;
    public Shard shardData;

    public override void Blur()
    {
        ShardSelectionHighlight.color = blurColor;
    }

    public override void Focus()
    {
        if(revealed)
            shardMenuUI.ShowDescription(shardData);
        else
            shardMenuUI.ShowDescription(shardData, "Still waiting to be discovered");

        ShardSelectionHighlight.color = focusColor;
    }
}
