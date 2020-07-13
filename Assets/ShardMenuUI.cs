using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    

    public override void Init()
    {
        ReisenGameProgress gameProgress = ReisenGameManager.instance.gameProgress;

        List<Shard> shardsToDisplay;

        if (isPauseMenuVersion)
            shardsToDisplay = new List<Shard>();
        else
            shardsToDisplay = new List<Shard>() { new Shard("0", 0, "testing", "why not"), new Shard("0", 0, "testing2", "why not2") };

        foreach (MenuUIElement oldElement in group.menuElements)
        {
            Destroy(oldElement.gameObject);
        }
        group.menuElements.Clear();

        for (int i = 0; i < shardsToDisplay.Count; i++)
        {
            Shard shard = shardsToDisplay[i];

            ShardMenuUIElement element = Instantiate(shardMenuElement).GetComponent<ShardMenuUIElement>();
            element.transform.parent = group.transform;
            Debug.Log((i % cols) + " " + (i / rows));
            element.transform.position = initialPosition.position + ((i % cols) * Vector3.right * xspacing) + ((i / rows) * Vector3.down * yspacing);
            element.parentMenu = this;
            element.parentGroup = group;
            element.pauseMenuUI = pauseMenuUI;
            element.parentMenuOnSelectMode = ParentMenuStatusPostSelect.none;
            element.shardData = shard;
            group.menuElements.Add(element);
        }
        base.Init();
    }

    public override void Open()
    {
        if (group.menuElements.Count == 0)
        {
            pauseMenuUI.ShowDescription("I haven't found any shards yet...", CharacterExpression.frown);
        }
        else
        {
            Debug.Log("focusing shard");
            group.FocusElement(group.currentMenuElementIndex);
        }
        base.Open();
    }
}
