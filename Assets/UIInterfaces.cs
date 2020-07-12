using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public interface ISelectableMenuElement
{
    void Select();
    void Focus();
    void Blur();
}

public interface ISelectableMenuElementGroup
{
    void Focus();
    void Blur();

    void FocusNextElementInDirection(InputDirection direction);
    void FocusElement(int i);
    void SelectElement(int i);
}

public interface IMenu 
{
    void Init();
    void Open();
    void Close();
    void Focus();
    void Blur();
}


public abstract class ControllableGridMenuElement : MonoBehaviour, ISelectableMenuElement
{
    public ControllableGridMenuGroup parentGroup;
    public UnityEvent SelectionEvent;
    public ControllableGridMenu parentMenu;
    public bool closeParent;

    public abstract void Blur();
    public abstract void Focus();

    public void Select()
    {
        SelectionEvent.Invoke();
        if(closeParent)
            parentMenu?.Close();
    }
}

public abstract class ControllableGridMenu : MonoBehaviour, IMenu
{
    public ControllableGridMenu previousMenu;

    public abstract void Blur();
    public abstract void Close();
    public abstract void Focus();
    public abstract void Init();
    public abstract void Open();

    public void Open(ControllableGridMenu previousMenu)
    {
        this.previousMenu = previousMenu;
        Open();
    }
}

public abstract class ControllableGridMenuGroup : MonoBehaviour, ISelectableMenuElementGroup
{
    public List<ControllableGridMenuElement> menuElements;
    public int currentMenuElementIndex;
    
    public abstract void Focus();
    public abstract void Blur();


    public void FocusElement(int index)
    {
        currentMenuElementIndex = index;
        for (int i = 0; i < menuElements.Count; i++)
        {
            ControllableGridMenuElement menuElement = menuElements[i];
            if (i == currentMenuElementIndex)
            {
                menuElement.Focus();
            }
            else
                menuElement.Blur();
        }
    }

    public void SelectElement(int i)
    {
        FocusElement(i);
        menuElements[i].Select();
    }

    public void FocusNextElementInDirection(InputDirection dir)
    {
        if (dir == InputDirection.N || dir == InputDirection.W)
        {
            currentMenuElementIndex--;
            if (currentMenuElementIndex < 0)
                currentMenuElementIndex = menuElements.Count - 1;
            FocusElement(currentMenuElementIndex);
        }
        else if (dir == InputDirection.S || dir == InputDirection.E)
        {
            currentMenuElementIndex++;
            if (currentMenuElementIndex >= menuElements.Count)
                currentMenuElementIndex = 0;
            FocusElement(currentMenuElementIndex);
        }
    }
}
