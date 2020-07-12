using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : ControllableGridMenu
{
    public MenuUIGroup group;

    //hacky
    public bool isTitle;
    public bool isGameplayMenu;


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

            this.Close();
            if (!isGameplayMenu)
            {
                Debug.Assert(!isTitle && previousMenu != null);

                previousMenu?.Open();
            }
            else
            {
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
        throw new System.NotImplementedException();
    }
    public override void Blur()
    {
        throw new System.NotImplementedException();
    }

    public override void Open()
    {
        this.gameObject.SetActive(true);
    }
    public override void Close()
    {
        this.gameObject.SetActive(false);
    }

    #endregion
}
