using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShardMenuUIElement : MenuUIElement
{
    public Color FoundColor;
    public Color MissingColor;

    public bool revealed = false;

    public Sprite singleShardSprite;
    public Sprite doubleShardSprite;

    public Image ShardImage;
    public Image ShardSelectionHighlight;

    public bool isPauseMenuVersion;

    public ShardMenuUI shardMenuUI;
    public Shard shardData;

    public override void Blur()
    {
        ShardImage.color = Color.black;
    }

    public override void Focus()
    {
        shardMenuUI.ShowDescription(shardData);
        ShardImage.color = Color.white;
    }
}
