using Assets.PodgeStandardLibrary.RPGSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuUI : MenuUI
{
    public GameObject ItemMenuElement;
    public Transform initialPosition;

    public float spacing;

    public override void Init()
    {
        ReisenGameProgress gameProgress = ReisenGameManager.instance.gameProgress;

        List<KeyItem> inventoryKeyItems = gameProgress.Player.GetKeyItemsInInventory();

        if (inventoryKeyItems.Count == 0)
            Debug.Log("Insert edge case of reisen playing making a quip");

        for (int i = 0; i < inventoryKeyItems.Count; i++)
        {
            KeyItem item = inventoryKeyItems[i];

            MenuUIElement element = Instantiate(ItemMenuElement).GetComponent<MenuUIElement>();
            element.transform.parent = group.transform;
            element.transform.position = initialPosition.position + i * Vector3.down * spacing;
            element.parentMenu = this;
            element.parentGroup = group;
            group.menuElements.Add(element);

        }

        base.Init();
    }
}
