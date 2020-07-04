using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ExpressionLine : ScriptLine
{
    CharacterExpression desiredExpression;

    public ExpressionLine(CharacterExpression expression)
    {
        desiredExpression = expression;
    }

    public static ExpressionLine CreateInstructionLine(CharacterExpression expression)
    {
        ExpressionLine line = new ExpressionLine(expression);

        return line;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        // very per game specific stuff, only the player can change expressions at the moment
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterDialogueAnimator>().changeExpression(desiredExpression);
    }

    public override bool IsFinished()
    {
        return true;
    }

    public override DialogueEngine.LineType GetLineType()
    {
        return DialogueEngine.LineType.SpeakingLine;
    }

    public override ScriptLine GetPreviousLine()
    {
        throw new System.NotImplementedException();
    }

    public override ScriptLine GetNextLine()
    {
        throw new System.NotImplementedException();
    }
}