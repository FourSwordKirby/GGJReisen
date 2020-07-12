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
    protected bool gainFocus;

    void Start()
    {
        Init();
    }

    public void Update()
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
        Init();
        gainFocus = true;
        Debug.Log(gainFocus);
    }

    public override void Blur()
    {
        gainFocus = false;
    }

    public override void Open()
    {
        this.gameObject.SetActive(true);
        Focus();
    }
    public override void Close()
    {
        Debug.Log("close");
        this.gameObject.SetActive(false);
        Blur();
    }

    #endregion
}
