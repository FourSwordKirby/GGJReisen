using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuUI : MenuUI
{
    public List<CreditsMenuUIElement> CreditElements;
    public Sprite unknownEndingSprite;
    public List<Sprite> endingSprites;
    public List<string> endingNames;

    public override void Open()
    {
        List<int> availableEndings = SaveManager.GetAcquiredEndings();
        for (int i =0; i < CreditElements.Count;i++)
        {
            int endingIndex = i + 1;
            CreditsMenuUIElement ending = CreditElements[i];
            Sprite endingSprite = endingSprites[i];
            string endingName = endingNames[i];


            if (availableEndings.Contains(endingIndex))
            {
                ending.SetText("Ending " + endingIndex + ": \n" + endingName);
                ending.SetPreviewImage(endingSprite);
            }
            else
            {
                ending.SetText("Ending " + endingIndex + ": \n???");
                ending.SetPreviewImage(unknownEndingSprite);
            }
        }

        base.Open();
    }

    public void ShowEnding(int ending)
    {
        List<int> availableEndings = SaveManager.GetAcquiredEndings();
        Debug.Log(availableEndings);
        if(availableEndings.Contains(ending))
        {
            AudioMaster.instance.PlayConfirmSfx();
            TitleScreenUtils.instance.ShowEnding(ending);
        }
    }
}
