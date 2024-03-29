﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDialogueAnimator : DialogueAnimator
{
    public Animator animator;
    public CharacterExpressionAnimator expressionAnimator;

    public Transform speechBubbleOrigin;

    public void Turn(float direction)
    {
        animator.SetFloat("SpeedModifier", 1.0f);
        animator.SetFloat("xDirection", direction);
    }

    public override void startTalking()
    {
        animator.SetBool("Talking", true);

    }

    public override void stopTalking()
    {
        animator.SetBool("Talking", false);
    }

    public void changeExpression(CharacterExpression expression)
    {
        expressionAnimator?.changeExpression(expression);
    }

    public override Vector3 getSpeechOrigin()
    {
        return speechBubbleOrigin.position;
    }
}