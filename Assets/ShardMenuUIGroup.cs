using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardMenuUIGroup : MenuUIGroup
{
    public int rows;
    public int cols;

    public override void FocusNextElementInDirection(InputDirection dir)
    {
        if (dir == InputDirection.W)
        {
            currentMenuElementIndex--;
            if (currentMenuElementIndex < 0)
                currentMenuElementIndex = menuElements.Count - 1;
            FocusElement(currentMenuElementIndex);
        }
        else if (dir == InputDirection.E)
        {
            currentMenuElementIndex++;
            if (currentMenuElementIndex >= menuElements.Count)
                currentMenuElementIndex = 0;
            FocusElement(currentMenuElementIndex);
        }
        else if(dir == InputDirection.N)
        {
            currentMenuElementIndex -= cols;
            if (currentMenuElementIndex < 0)
                currentMenuElementIndex = menuElements.Count - cols + (currentMenuElementIndex % cols);
            FocusElement(currentMenuElementIndex);
        }
        else if (dir == InputDirection.S)
        {
            currentMenuElementIndex += cols;
            if (currentMenuElementIndex >= menuElements.Count)
                currentMenuElementIndex = currentMenuElementIndex % cols;
            FocusElement(currentMenuElementIndex);
        }
    }
}
