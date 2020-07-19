using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTabUIElement : MenuUIElement
{
    public GameObject tabTitleDropShadow;
    public GameObject tabPanel;
    public GameObject blurPanel;


    public override void Blur()
    {
        tabTitleDropShadow.SetActive(false);
        tabPanel.SetActive(false);
        blurPanel.SetActive(false);
    }

    public override void Focus()
    {
        blurPanel.transform.SetSiblingIndex(1);
        tabTitleDropShadow.SetActive(true);
        tabPanel.SetActive(true);
        blurPanel.SetActive(true);
    }

    public override void Select()
    {
        blurPanel.transform.SetAsFirstSibling();
        base.Select();
    }
}
