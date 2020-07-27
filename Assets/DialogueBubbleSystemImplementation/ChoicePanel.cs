using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoicePanel : MonoBehaviour
{
    public MeshRenderer textFrame;
    public TextMeshPro textMesh;
    public Animator animator;

    public Color whitebackground;
    public Color greybackground;

    public bool ChoiceBubbleInFocus;

    // add animations for choices flipping to active and inactive etc.

    public void Show()
    {
        //animator.SetBool("Deployed", true);
    }

    public void Focus()
    {
        if(ChoiceBubbleInFocus)
            textFrame.material.color = greybackground;
        else
            textFrame.material.color = whitebackground;
    }

    public void Blur()
    {
        if (ChoiceBubbleInFocus)
            textFrame.material.color = whitebackground;
        else
            textFrame.material.color = greybackground;
    }

    public void Hide()
    {
        //animator.SetBool("Deployed", false);
    }

    public void SetChoicePanelContent(ChoiceLineContent content)  
    {
        textMesh.text = content.lineText;
    }

}
