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
    bool InFocus();
}

public enum ParentMenuStatusPostSelect
{
    blur,
    close,
    none
}

public enum GridNavigationMode
{
    Horizontal,
    Vertical,
    FreeForm
}

public abstract class ControllableGridMenuElement : MonoBehaviour, ISelectableMenuElement
{
    public ControllableGridMenuGroup parentGroup;
    public UnityEvent SelectionEvent;
    public ControllableGridMenu parentMenu;
    public ParentMenuStatusPostSelect parentMenuOnSelectMode;

    public abstract void Blur();
    public abstract void Focus();

    public virtual void Select()
    {
        AudioMaster.instance.PlayConfirmSfx();
        SelectionEvent.Invoke();
        switch (parentMenuOnSelectMode)
        {
            case ParentMenuStatusPostSelect.blur:
                parentMenu?.Blur();
                break;
            case ParentMenuStatusPostSelect.close:
                parentMenu?.Close();
                break;
            case ParentMenuStatusPostSelect.none:
                break;
        }
    }
}

public abstract class ControllableGridMenu : MonoBehaviour, IMenu
{
    public ControllableGridMenu previousMenu;
    public bool inFocus;

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

    public bool InFocus()
    {
        return inFocus;
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
        if (menuElements.Count == 0)
            return;

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
        if (menuElements.Count == 0)
            return;

        if (menuElements.Count > 0)
        {
            FocusElement(i);
            menuElements[i].Select();
        }
    }


    public GridNavigationMode gridNavigationMode;
    public virtual void FocusNextElementInDirection(InputDirection dir)
    {
        // No navigation needed when there is 1 element
        if (menuElements.Count == 1)
            return;

        switch (gridNavigationMode)
        {
            case GridNavigationMode.Horizontal:
                if (dir == InputDirection.W)
                {
                    AudioMaster.instance.PlayMenuSelectSfx();

                    currentMenuElementIndex--;
                    if (currentMenuElementIndex < 0)
                        currentMenuElementIndex = menuElements.Count - 1;
                    FocusElement(currentMenuElementIndex);
                }
                else if (dir == InputDirection.E)
                {
                    AudioMaster.instance.PlayMenuSelectSfx();
                    currentMenuElementIndex++;
                    if (currentMenuElementIndex >= menuElements.Count)
                        currentMenuElementIndex = 0;
                    FocusElement(currentMenuElementIndex);
                }
                break;
            case GridNavigationMode.Vertical:
                if (dir == InputDirection.N)
                {
                    AudioMaster.instance.PlayMenuSelectSfx();
                    currentMenuElementIndex--;
                    if (currentMenuElementIndex < 0)
                        currentMenuElementIndex = menuElements.Count - 1;
                    FocusElement(currentMenuElementIndex);
                }
                else if (dir == InputDirection.S)
                {
                    AudioMaster.instance.PlayMenuSelectSfx();
                    currentMenuElementIndex++;
                    if (currentMenuElementIndex >= menuElements.Count)
                        currentMenuElementIndex = 0;
                    FocusElement(currentMenuElementIndex);
                }
                break;
            case GridNavigationMode.FreeForm:
                if (dir == InputDirection.W || dir == InputDirection.S || dir == InputDirection.E || dir == InputDirection.N)
                    AudioMaster.instance.PlayMenuSelectSfx();
                // This is case doesn't do anything fancy with focusing elements, we let Unity's UI navigation tools to the heavy lifting here
                break;
        }
    }
}
