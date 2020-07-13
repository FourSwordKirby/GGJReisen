using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardMenuUI : MenuUI
{
    public bool isPauseMenuVersion;

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
            shardsToDisplay = gameProgress.Player.ShardsAcquired;
        else
            shardsToDisplay = new List<Shard>() { new Shard("0", 0, "testing", "why not"), new Shard("0", 0, "testing2", "why not2") };

        if (shardsToDisplay.Count == 0)
            Debug.Log("Insert edge case of reisen playing making a quip");

        for (int i = 0; i < shardsToDisplay.Count; i++)
        {
            Shard shard = shardsToDisplay[i];

            MenuUIElement element = Instantiate(shardMenuElement).GetComponent<MenuUIElement>();
            element.transform.parent = group.transform;
            element.transform.position = initialPosition.position + i % cols * Vector3.right * xspacing + ((i / (rows))) * Vector3.down * yspacing;
            element.parentMenu = this;
            element.parentGroup = group;
            group.menuElements.Add(element);

        }
        base.Init();
    }
}
