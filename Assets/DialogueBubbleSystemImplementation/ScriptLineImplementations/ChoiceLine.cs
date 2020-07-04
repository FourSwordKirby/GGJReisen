using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ChoiceLine : ScriptLine
{
    DialogueAnimator speakerAnimator;
    public string chosenOption;

    public List<ChoiceLineContent> dialogueChoices;

    public ChoiceLine(List<ChoiceLineContent> choices)
    {
        dialogueChoices = choices;

        // very per game specific stuff, only the player can change have the choice dialogue appear over them
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        speakerAnimator = player.GetComponent<DialogueAnimator>();
    }

    public static ChoiceLine GenerateChoiceLine(List<ChoiceLineContent> choices)
    {
        ChoiceLine line = new ChoiceLine(choices);

        return line;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        Vector3 speakerPosition = speakerAnimator.getSpeechOrigin();

        DialogueBubbleUI.instance.DisplayChoices(dialogueChoices, speakerPosition);
    }

    public override bool IsFinished()
    {
        return DialogueBubbleUI.instance.ready;
    }

    public override DialogueEngine.LineType GetLineType()
    {
        return DialogueEngine.LineType.Choice;
    }

    public override ScriptLine GetPreviousLine()
    {
        throw new System.NotImplementedException();
    }

    public override ScriptLine GetNextLine()
    {
        throw new System.NotImplementedException();
    }

    //public static ExpressionLine CreateInstructionLine(string instruction)
    //{
    //    ExpressionLine line = new ExpressionLine(instruction);

    //    return line;
    //}

    ////Change this based on the game implementation
    //public override void PerformLine()
    //{
    //    // very per game specific stuff
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    player.GetComponent<CharacterDialogueAnimator>().changeExpression(desiredExpression);
    //}

    //public override bool IsFinished()
    //{
    //    return true;
    //}

    //public override DialogueEngine.LineType GetLineType()
    //{
    //    return DialogueEngine.LineType.SpeakingLine;
    //}
}