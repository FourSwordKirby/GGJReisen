using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIInteractionManager : MonoBehaviour
{
    public IUIController activeUIController;
    public Stack<IUIController> UIControllerStack;

    public delegate void ClickAction();

    private void Start()
    {
    }

    public void EnterUI(IUIController newController)
    {
        if(activeUIController != null)
            UIControllerStack.Push(activeUIController);
        activeUIController.Suspend();
        activeUIController = newController;
        activeUIController.Enter();
    }

    public void ExitUI()
    {
        activeUIController.Exit(); 
        activeUIController = UIControllerStack.Pop();
        activeUIController.Resume();
    }
}

public interface IUIController
{
    void InterpretInputDirection(InputDirection dir);
    void InterpretConfirm();
    void InterpretCancel();
    void Suspend();
    void Resume();

    void Enter();
    void Exit();
}


public struct MenuElement
{
    public IHighlightableElement content;
    public List<MenuElement> neighbors;
    public Vector2 position;
}

public abstract class AbstractControllableMenu : MonoBehaviour, ISelectableMenuElementGroup
{
    public List<MenuElement> menuElements;
    public int currentMenuElementIndex;

    public abstract void Focus();
    public abstract void Blur();


    public void FocusElement(int index)
    {
        currentMenuElementIndex = index;
        for (int i = 0; i < menuElements.Count; i++)
        {
            IHighlightableElement content = menuElements[i].content;
            if (i == currentMenuElementIndex)
            {
                content.Focus();
            }
            else
                content.Blur();
        }
    }



    public void FocusElement(MenuElement element)
    {
        for (int i = 0; i < menuElements.Count; i++)
        {
            IHighlightableElement content = menuElements[i].content;
            if (menuElements[i].Equals(element))
            {
                content.Focus();
            }
            else
                content.Blur();
        }
    }

    //public void SelectElement(int i)
    //{
    //    if (menuElements.Count > 0)
    //    {
    //        FocusElement(i);
    //        content[i].Select();
    //    }
    //}


    public bool hortizontal;
    public virtual void FocusNextElementInDirection(Vector2 direction)
    {
        MenuElement currentElement = menuElements[currentMenuElementIndex];

        MenuElement closestElement = currentElement;
        float smallestAngle = 90.0f;
        foreach(MenuElement element in currentElement.neighbors)
        {
            Vector2 orientation = element.position - currentElement.position;
            float offset = Vector2.Angle(direction.normalized, orientation.normalized);
            if (offset < smallestAngle)
                closestElement = element;
        }

        FocusElement(closestElement);
    }

    public void FocusNextElementInDirection(InputDirection direction)
    {
        throw new System.NotImplementedException();
    }

    public void SelectElement(int i)
    {
        throw new System.NotImplementedException();
    }
}

public interface IUIElement
{
    void Show();
    void Hide();
}

public interface IHighlightableElement
{
    void Focus();
    void Blur();
}

public interface SelectableElement
{
    void Select();
}