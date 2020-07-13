using Assets.PodgeStandardLibrary.RPGSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuUI : MenuUI
{
    public PauseMenuUI pauseMenuUI;

    public GameObject ItemMenuElement;
    public Transform initialPosition;

    public float spacing;

    public override void Init()
    {
        ReisenGameProgress gameProgress = ReisenGameManager.instance.gameProgress;
        List<KeyItem> inventoryKeyItems = gameProgress.Player.GetKeyItemsInInventory();

        foreach (MenuUIElement oldElement in group.menuElements)
        {
            Destroy(oldElement.gameObject);
        }
        group.menuElements.Clear();

        for (int i = 0; i < inventoryKeyItems.Count; i++)
        {
            KeyItem item = inventoryKeyItems[i];

            ItemUIElement element = Instantiate(ItemMenuElement).GetComponent<ItemUIElement>();
            element.transform.parent = group.transform;
            element.transform.position = initialPosition.position + i * Vector3.down * spacing;
            element.parentMenu = this;
            element.parentGroup = group;
            element.pauseMenuUI = pauseMenuUI;
            element.parentMenuOnSelectMode = ParentMenuStatusPostSelect.none;
            element.itemData = item;
            group.menuElements.Add(element);
        }


        //if (group.menuElements.Count == 0)
        //{
        //    KeyItem fillerItem = new KeyItem("", "Gah where did all of my inventory go!");
        //    ItemUIElement element = Instantiate(ItemMenuElement).GetComponent<ItemUIElement>();
        //    element.transform.parent = group.transform;
        //    element.transform.position = initialPosition.position;
        //    element.parentMenu = this;
        //    element.parentGroup = group;
        //    element.pauseMenuUI = pauseMenuUI;
        //    element.itemData = fillerItem;
        //    group.menuElements.Add(element);
        //}

        base.Init();
    }


    public override void Open()
    {
        if (group.menuElements.Count == 0)
        {
            pauseMenuUI.ShowDescription("Gah where did all of my inventory go!", CharacterExpression.frown);
        }
        else
        {
            group.FocusElement(group.currentMenuElementIndex);
        }

        base.Open();
    }
}
