using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    void InitializeTitleMenu()
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
    }

    void AddMenuElement(MenuUIElement element, int index)
    {
        group.menuElements.Add(element);
        element.transform.position = menuElementPositions[index].position;
    }

}
