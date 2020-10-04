using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

public class ChoiceLine : ScriptLine
{
    DialogueAnimator speakerAnimator;

    public List<ChoiceLineContent> dialogueChoices;
    private int chosenOptionIndex;

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
            throw new Exception($"Cannot determine speaker '{speaker}'", e);
        }

        if(choices.Any(x => x.jumpLabel == "leave"))
            chosenOptionIndex = dialogueChoices.Count - 1;
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
            if (labeledLines.TryGetValue(choice.jumpLabel, out ScriptLine jumpline))
            {
                choice.jumpLine = jumpline;
            }
            else
            {
                throw new Exception($"Line {choice.lineNumber}: Choice with text '{choice.lineText}' was unable to find label '{jumpLabel}'");
            }
            reInitDialogueChoices.Add(choice);
        }

        dialogueChoices = reInitDialogueChoices;
    }
}