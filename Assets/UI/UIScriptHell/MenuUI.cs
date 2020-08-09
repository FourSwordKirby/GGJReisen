using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : ControllableGridMenu
{
    public MenuUIGroup group;

    //hacky
    public bool isGameplayMenu;
    public bool persistOnExit;
    public bool gainFocus;
    public bool focusSubmenu = true;

    public ParentMenuStatusPostSelect prevMenuMode;

    void Start()
    {
        Init();
    }

    public virtual void Update()
    {
        if (!InFocus())
        {
            if (gainFocus)
            {
                inFocus = true;
            }

            return;
        }

        if (!gainFocus)
        {
            inFocus = false;
            return;
        };

        InputDirection dir = Controls.getInputDirectionDown();
        group.FocusNextElementInDirection(dir);
        if (Controls.confirmInputDown())
        {
            group.SelectElement(group.currentMenuElementIndex);
        }
        if (Controls.cancelInputDown())
        {
            AudioMaster.instance.PlayCancelSfx();
            if (!isGameplayMenu)
            {
                Blur();
                if (!persistOnExit)
                    Close();


                switch (prevMenuMode)
                {
                    case ParentMenuStatusPostSelect.blur:
                        previousMenu?.Focus();
                        break;
                    case ParentMenuStatusPostSelect.close:
                        previousMenu?.Open();
                        break;
                    case ParentMenuStatusPostSelect.none:
                        break;
                }
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
        if (gainFocus || inFocus)
        {
            group.FocusElement(group.currentMenuElementIndex);
        }
        else
        {
            Blur();
        }
    }


    public override void Focus()
    {
        if (focusSubmenu)
            group.FocusElement(group.currentMenuElementIndex);
        gainFocus = true;
    }

    public override void Blur()
    {
        gainFocus = false;
        if(group != null)
        {
            for (int i = 0; i < group.menuElements.Count; i++)
            {
                ControllableGridMenuElement menuElement = group.menuElements[i];
                menuElement.Blur();
            }
        }
    }

    public override void Open()
    {
        this.gameObject.SetActive(true);
        Focus();
    }
    public override void Close()
    {
        this.gameObject.SetActive(false);
        Blur();
    }

    #endregion
}
