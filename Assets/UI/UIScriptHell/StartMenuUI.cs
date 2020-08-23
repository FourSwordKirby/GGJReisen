using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MenuUI
{
    public List<Transform> menuElementPositions;
    public MenuUIElement newGameElement;
    public MenuUIElement continueGameElement;
    public MenuUIElement viewShardsElement;
    public MenuUIElement clearShardDataElement;
    public MenuUIElement optionsElement;


    // Start is called before the first frame update
    void Start()
    {
        InitializeTitleMenu();
    }

    public override void Update()
    {
        if (Controls.cancelInputDown())
            return;

        base.Update();
    }

    public void InitializeTitleMenu()
    {
        group.menuElements.Clear();

        bool hasSaveData = false;
        foreach (string saveName in SaveManager.saveNames)
        {
            GGJReisenSave saveData = SaveManager.FetchSaveData(saveName);
            hasSaveData |= saveData != null;
        }

        bool hasShardData = false;
        ShardSaveData shardData = SaveManager.FetchSeenShardData();
        hasShardData = hasShardData || (shardData != null);

        newGameElement.gameObject.SetActive(true);
        optionsElement.gameObject.SetActive(true);
        continueGameElement.gameObject.SetActive(hasSaveData);


        viewShardsElement.gameObject.SetActive(hasShardData);
        clearShardDataElement.gameObject.SetActive(hasShardData);

        int activeMenuElement = 0;
        if (hasSaveData)
        {
            AddMenuElement(continueGameElement, activeMenuElement);
            activeMenuElement++;
            UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = continueGameElement.gameObject;
        }
        AddMenuElement(newGameElement, activeMenuElement);
        activeMenuElement++;

        if (hasShardData)
        {
            AddMenuElement(viewShardsElement, activeMenuElement);
            activeMenuElement++;
            AddMenuElement(clearShardDataElement, activeMenuElement);
            activeMenuElement++;
        }

        AddMenuElement(optionsElement, activeMenuElement);
        activeMenuElement++;

        //setting navigation
        if (!hasSaveData || !hasShardData)
        {
            Navigation newGameNav = new Navigation();
            newGameNav.mode = Navigation.Mode.Explicit;
            Navigation optionsNav = new Navigation();
            optionsNav.mode = Navigation.Mode.Explicit;

            if (!hasSaveData)
            {
                newGameNav.selectOnUp = optionsElement.GetComponent<Button>();
                optionsNav.selectOnDown = newGameElement.GetComponent<Button>();
            }
            else
            {
                newGameNav.selectOnUp = continueGameElement.GetComponent<Button>();
                optionsNav.selectOnDown = continueGameElement.GetComponent<Button>();
            }

            if (!hasShardData)
            {
                newGameNav.selectOnDown = optionsElement.GetComponent<Button>();
                optionsNav.selectOnUp = newGameElement.GetComponent<Button>();
            }
            else
            {
                newGameNav.selectOnDown = viewShardsElement.GetComponent<Button>();
                optionsNav.selectOnUp = clearShardDataElement.GetComponent<Button>();
            }

            newGameElement.GetComponent<Button>().navigation = newGameNav;
            optionsElement.GetComponent<Button>().navigation = optionsNav;
        }

        //dumb hacks we don't really care about groups anymore lol
        group.menuElements.Clear();
        group.FocusElement(group.currentMenuElementIndex);
    }

    void AddMenuElement(MenuUIElement element, int index)
    {
        group.menuElements.Add(element);
        element.transform.position = menuElementPositions[index].position;
        element.transform.parent = menuElementPositions[index];
    }

}
