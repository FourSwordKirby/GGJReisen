using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShardMenuUIElement : MenuUIElement
{
    public Color FoundColor;
    public Color MissingColor;

    public Image ShardSprite;
    public Image ShardSelectionHighlight;

    public PauseMenuUI pauseMenuUI;
    public Shard shardData;

    public override void Blur()
    {
        ShardSprite.color = Color.black;
    }

    public override void Focus()
    {
        pauseMenuUI.ShowDescription(shardData.Description);
        ShardSprite.color = Color.white;
    }
}
