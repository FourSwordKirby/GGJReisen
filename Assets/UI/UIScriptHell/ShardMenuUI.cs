using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShardMenuUI : MenuUI
{
    public bool isPauseMenuVersion;

    public PauseMenuUI pauseMenuUI;

    public GameObject shardMenuElement;
    public Transform initialPosition;
    public int rows;
    public int cols;

    public float xspacing;
    public float yspacing;

    public TextMeshProUGUI shardTitle;
    public TextMeshProUGUI shardDescription;
    public Image shardImage;

    public TextMeshProUGUI emptyText;
    public TextMeshProUGUI completionProgress;


    public override void Init()
    {
        List<Shard> totalShardData = SaveManager.FetchSeenShardData()?.shardData;
        List<Shard> shardsToDisplay;

        if (isPauseMenuVersion)
        {
            ReisenGameProgress gameProgress = ReisenGameManager.instance.gameProgress;
            shardsToDisplay = gameProgress.Player.ShardsAcquired;
                // debug things
                // new List<Shard>() { new Shard("0", 0, "titletesting", "why not"), new Shard("0", 0, "testing2", "why not2"),
                //                                    new Shard("0", 0, "titletesting2", "why not3"), new Shard("0", 0, "testing2", "why not2"),
                //                                    new Shard("0", 0, "titletesting3", "why not3"), new Shard("0", 0, "testing2", "why not2"),
                //                                    new Shard("0", 0, "titletesting3", "why not3"), new Shard("0", 0, "testing2", "why not2")  };
        }
        else
        {
            shardsToDisplay = Shard.ShardDictionary.Select(x => x.Value).ToList();
        }

        foreach (MenuUIElement oldElement in group.menuElements)
        {
            Destroy(oldElement.gameObject);
        }
        group.menuElements.Clear();

        emptyText.gameObject.SetActive(shardsToDisplay.Count == 0);

        for (int i = 0; i < shardsToDisplay.Count; i++)
        {
            Shard shard = shardsToDisplay[i];

            ShardMenuUIElement element = Instantiate(shardMenuElement).GetComponent<ShardMenuUIElement>();
            element.transform.SetParent(group.transform, false);
            element.transform.position = initialPosition.position + ((i % cols) * Vector3.right * xspacing) + ((i / cols) * Vector3.down * yspacing);
            element.parentMenu = this;
            element.parentGroup = group;
            element.isPauseMenuVersion = isPauseMenuVersion;
            element.shardMenuUI = this;
            element.parentMenuOnSelectMode = ParentMenuStatusPostSelect.none;
            element.shardData = shard;
            element.ShardImage.sprite = shard.ShardValue == 2 ? element.doubleShardSprite : element.singleShardSprite;
            group.menuElements.Add(element);

            if (isPauseMenuVersion)
                element.IsRevealed = true;
            else
                element.IsRevealed = totalShardData.Any(x => x.Id == shard.Id);
        }

        if(completionProgress != null)
        {
            float totalShardCount;
            if (totalShardData.Count == 0)
                totalShardCount = 0;
            else
                totalShardCount = totalShardData.Select(x => x.ShardValue).Aggregate((x, y) => x + y);
            float displayShardCount = shardsToDisplay.Select(x => x.ShardValue).Aggregate((x, y) => x + y);
            completionProgress.text = ((int)(totalShardCount / displayShardCount * 100.0f)).ToString() + "%";
        }

        base.Init();
    }

    public void ShowDescription(Shard shardData, string forcedDescription = "")
    {
        shardTitle.text = shardData.FriendlyName;

        string description = string.IsNullOrEmpty(forcedDescription) ? shardData.Description : forcedDescription;
        if (isPauseMenuVersion)
            pauseMenuUI.ShowDescription(description);
        else
        {
            shardDescription.text = description;
        }

    }

    public override void Open()
    {
        if (group.menuElements.Count == 0)
        {
            if (isPauseMenuVersion)
                pauseMenuUI.ShowDescription("I haven't found any shards yet...", CharacterExpression.sad);
            else
                Debug.Log("non pause verion");
        }
        else
        {
            Debug.Log("focusing shard");
            group.FocusElement(group.currentMenuElementIndex);
        }
        base.Open();
    }


    public override void Blur()
    {
        shardTitle.text = "";
        base.Blur();
    }
}
