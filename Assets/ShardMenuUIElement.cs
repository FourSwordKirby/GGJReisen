using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShardMenuUIElement : MenuUIElement
{
    public Color FoundColor;
    public Color MissingColor;

    public bool revealed = false;

    public Image ShardSprite;
    public Image ShardSelectionHighlight;

    public bool isPauseMenuVersion;

    public ShardMenuUI shardMenuUI;
    public Shard shardData;

    public override void Blur()
    {
        ShardSprite.color = Color.black;
    }

    public override void Focus()
    {
        shardMenuUI.ShowDescription(shardData);
        ShardSprite.color = Color.white;
    }
}
