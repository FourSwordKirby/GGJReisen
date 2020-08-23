using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardMenuUIGroup : MenuUIGroup
{
    public int rows;
    public int cols;

    public override void FocusNextElementInDirection(InputDirection dir)
    {
        if (menuElements.Count == 0 || menuElements.Count == 1)
            return;

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
        else if(dir == InputDirection.N)
        {
            AudioMaster.instance.PlayMenuSelectSfx();

            currentMenuElementIndex -= cols;
            if (currentMenuElementIndex < 0)
                currentMenuElementIndex = Mathf.Min(menuElements.Count - 1, (menuElements.Count/cols) * cols + ((currentMenuElementIndex + cols) % cols));
            FocusElement(currentMenuElementIndex);
        }
        else if (dir == InputDirection.S)
        {
            AudioMaster.instance.PlayMenuSelectSfx();

            currentMenuElementIndex += cols;
            if (currentMenuElementIndex >= menuElements.Count)
                currentMenuElementIndex = currentMenuElementIndex % cols;
            FocusElement(currentMenuElementIndex);
        }
    }
}
