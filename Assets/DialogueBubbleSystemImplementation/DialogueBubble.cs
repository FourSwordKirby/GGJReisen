using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueBubble : MonoBehaviour, IDialogueBubble
{
    public bool loggable = true;

    public MeshRenderer textFrame;
    public Animator animator;

    public Transform anchorPoint;
    public float displayModifier;
    public Color focusColor;
    public Color blurColor;

    private void Start()
    {
        animator.speed = displayModifier == 0 ? 1 : displayModifier;
    }

    //currently the bubble only displays facing left or right depending on the argument
    //in the future we want it to display based on any number of relative positions.
    //at the very least, it should make sure that it is visible on camera and can be positioned over
    //the center of the relevant character
    public virtual void DeployAt(Vector3 speakerPosition, Vector3 displacementVector)
    {
        Show();

        if(displacementVector.x < 0)
            this.transform.position = speakerPosition - anchorPoint.localPosition;
        else
        {
            textFrame.transform.localScale -= Vector3.right * textFrame.transform.localScale.x * 2;
            this.transform.position = speakerPosition - anchorPoint.localPosition + Vector3.right * anchorPoint.localPosition.x * 2;
        }
    }

    public virtual void Show()
    {
        animator.SetBool("Deployed", true);
    }

    public virtual void Focus()
    {
        textFrame.material.color = focusColor;
    }

    public virtual void Blur()
    {
        textFrame.material.color = blurColor;
    }

    public virtual void Hide()
    {
        animator.SetBool("Deployed", false);
    }

    public virtual void Cleanup()
    {
        if (!loggable)
            Destroy();
    }

    public virtual void Destroy()
    {
        Destroy(this.gameObject);
    }
}
