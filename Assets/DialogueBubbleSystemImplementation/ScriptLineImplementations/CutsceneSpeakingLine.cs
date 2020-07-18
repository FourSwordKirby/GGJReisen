using UnityEngine;
using System;

public class CutsceneSpeakingLine : SpeakingLine
{
    public SpeakingLineContent content;
    public DialogueBubbleType type;

    public CutsceneSpeakingLine(string speaker, string lineText, int lineNumber, string label = "" ):base(speaker,lineText,lineNumber,label)
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

    public new static SpeakingLine CreateSpeakingLine(string speaker, string lineText, int lineNumber)
    {
        SpeakingLine line = new CutsceneSpeakingLine(speaker, lineText, lineNumber);

        return line;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        CutsceneUI.instance.DisplayText(content);
    }

    public override bool IsFinished()
    {
        return CutsceneUI.instance.ready;
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