using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTabUI : MenuUIElement
{
    public GameObject activeTab;
    public GameObject inactiveTab;

    public override void Blur()
    {
        inactiveTab.SetActive(true);
        activeTab.SetActive(false);
    }

    public override void Focus()
    {
        inactiveTab.SetActive(false);
        activeTab.SetActive(true);
    }
}
