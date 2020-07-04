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

    public Color focusColor;
    public Color blurColor;

    // add animations for choices flipping to active and inactive etc.

    public void Show()
    {
        animator.SetBool("Deployed", true);
    }

    public void Focus()
    {
        textFrame.material.color = focusColor;
    }

    public void Blur()
    {
        textFrame.material.color = blurColor;
    }

    public void Hide()
    {
        animator.SetBool("Deployed", false);
    }

    public void SetChoicePanelContent(ChoiceLineContent content)
    {
        textMesh.text = content.lineText;
    }

}
