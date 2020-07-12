using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : ControllableGridMenu
{
    public MenuUIGroup group;

    //hacky
    public bool isTitle;
    public bool isGameplayMenu;
    public bool persistOnExit;

    void Start()
    {
        Init();
    }

    public void Update()
    {
        InputDirection dir = Controls.getInputDirectionDown();
        group.FocusNextElementInDirection(dir);
        if (Controls.confirmInputDown())
        {
            group.SelectElement(group.currentMenuElementIndex);
        }
        if (Controls.cancelInputDown())
        {
            if (isTitle)
                return;

            if (!isGameplayMenu)
            {
                Debug.Assert(!isTitle && previousMenu != null);

                Blur();
                if (!persistOnExit)
                    Close();
                previousMenu?.Open();
            }
            else
            {
                Close();
                RpgGameManager.instance.ResumeGameplay();
            }
        }
    }

    #region interface implementation
    public override void Init()
    {
        group.FocusElement(group.currentMenuElementIndex);
    }


    public override void Focus()
    {
        this.enabled = true;
    }
    public override void Blur()
    {
        this.enabled = false;
    }

    public override void Open()
    {
        Focus();
        this.gameObject.SetActive(true);
    }
    public override void Close()
    {
        Blur();
        this.gameObject.SetActive(false);
    }

    #endregion
}
