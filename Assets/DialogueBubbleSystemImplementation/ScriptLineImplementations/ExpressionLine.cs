using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ExpressionLine : ScriptLine
{
    CharacterExpression desiredExpression;

    public ExpressionLine(string speaker, CharacterExpression expression)
    {
        this.speaker = speaker;
        desiredExpression = expression;
    }

    public static ExpressionLine CreateInstructionLine(string speaker, CharacterExpression expression)
    {
        ExpressionLine line = new ExpressionLine(speaker, expression);

        return line;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        try
        {
            // very per game specific stuff
            CharacterDialogueAnimator speakerAnimator = GameObject.Find(speaker).GetComponent<CharacterDialogueAnimator>();
            if (speakerAnimator == null)
                speakerAnimator = GameObject.Find(speaker).GetComponentInChildren<CharacterDialogueAnimator>();
            speakerAnimator.changeExpression(desiredExpression);
        }
        catch (Exception e)
        {
            Debug.Log("attempted speaker for this expression is " + speaker);
            throw e;
        }
    }

    public override bool IsFinished()
    {
        return true;
    }

    public override DialogueEngine.LineType GetLineType()
    {
        return DialogueEngine.LineType.SpeakingLine;
    }

    // hacky implementation that relies on the overarching dialogue object incrementing and decrementing properly
    public override ScriptLine GetPreviousLine()
    {
        return null;
    }

    public override ScriptLine GetNextLine()
    {
        return nextLine;
    }
}