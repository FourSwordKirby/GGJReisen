using UnityEngine;
using System;

public class SpeakingLine : ScriptLine
{
    SpeakingLineContent content;
    public DialogueBubbleType type;

    public SpeakingLine(string speaker, string lineText, int lineNumber, string label = "" ): base(label)
    {
        if (lineText.StartsWith("**"))
            type = DialogueBubbleType.Thought;
        if (lineText.StartsWith("^^"))
            type = DialogueBubbleType.Exclamation;
        if (lineText.StartsWith("~~"))
            type = DialogueBubbleType.Whisper;
        if (lineText.StartsWith("``"))
            type = DialogueBubbleType.Weak;

        if (lineText.StartsWith("**") || lineText.StartsWith("^^") || lineText.StartsWith("~~"))
            lineText = lineText.Substring(2);

        content = new SpeakingLineContent(speaker, lineText, lineNumber);
    }

    public static SpeakingLine CreateSpeakingLine(string speaker, string lineText, int lineNumber)
    {
        SpeakingLine line = new SpeakingLine(speaker, lineText, lineNumber);

        return line;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        if (content.lineText == "")
            return;
        try
        {
            // very per game specific stuff
            DialogueAnimator speakerAnimator;
            speakerAnimator = GameObject.Find(speaker).GetComponent<DialogueAnimator>();
            if (speakerAnimator == null)
                speakerAnimator = GameObject.Find(speaker).GetComponentInChildren<DialogueAnimator>();
            Vector3 speakerPosition = speakerAnimator.getSpeechOrigin();

            DialogueBubbleUI.instance.DisplaySpeechBubble(content, speakerPosition, type);
        }
        catch (Exception e)
        {
            Debug.Log("attempted speaker is " + speaker);
            throw e;
        }
    }

    public override bool IsFinished()
    {
        return DialogueBubbleUI.instance.ready;
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