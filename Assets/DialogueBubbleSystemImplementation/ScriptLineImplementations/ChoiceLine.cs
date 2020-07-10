using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ChoiceLine : ScriptLine
{
    string speaker;
    DialogueAnimator speakerAnimator;

    public List<ChoiceLineContent> dialogueChoices;
    private int chosenOptionIndex = 0;

    public ChoiceLine(string speaker, List<ChoiceLineContent> choices)
    {
        dialogueChoices = choices;

        // very per game specific stuff, only the player can change have the choice dialogue appear over them
        try
        {
            // very per game specific stuff
            speakerAnimator = GameObject.Find(speaker).GetComponent<DialogueAnimator>();
        }
        catch (Exception e)
        {
            Debug.Log("attempted speaker is " + speaker);
            throw e;
        }

    }

    public static ChoiceLine GenerateChoiceLine(string speaker, List<ChoiceLineContent> choices)
    {
        ChoiceLine line = new ChoiceLine(speaker, choices);

        return line;
    }

    public void NextOption()
    {
        chosenOptionIndex++;
        if (chosenOptionIndex >= dialogueChoices.Count)
            chosenOptionIndex = 0;
    }

    public void PreviousOption()
    {
        chosenOptionIndex--;
        if (chosenOptionIndex < 0)
            chosenOptionIndex = dialogueChoices.Count - 1;
    }

    public int GetOptionIndex()
    {
        return chosenOptionIndex;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        Vector3 speakerPosition = speakerAnimator.getSpeechOrigin();

        DialogueBubbleUI.instance.DisplayChoices(this, speakerPosition);
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
        return null;
    }

    public override ScriptLine GetNextLine()
    {
        return dialogueChoices[chosenOptionIndex].jumpLine;
    }

    internal void InitJumps(Dictionary<string, ScriptLine> labeledLines)
    {
        List<ChoiceLineContent> reInitDialogueChoices = new List<ChoiceLineContent>();

        for (int i = 0; i < dialogueChoices.Count; i++)
        {
            ChoiceLineContent choice = dialogueChoices[i];
            string jumpLabel = choice.jumpLabel;
            if (labeledLines[jumpLabel] == null)
                throw new Exception("line wasn't tagged in this dialogue");
            else
                choice.jumpLine = labeledLines[jumpLabel];
            reInitDialogueChoices.Add(choice);
        }

        dialogueChoices = reInitDialogueChoices;
    }
}