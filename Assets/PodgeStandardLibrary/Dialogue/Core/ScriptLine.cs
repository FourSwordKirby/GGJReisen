using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class ScriptLine
{
    public abstract DialogueEngine.LineType GetLineType();

    public abstract void PerformLine();
    public abstract bool IsFinished();

    public abstract ScriptLine GetPreviousLine();
    public abstract ScriptLine GetNextLine();

    /// <summary>
    /// Used for navigating a dialogue tree when navigating dialogue
    /// </summary>
    public readonly string lineLabel;

    public ScriptLine(string label = "")
    {
        lineLabel = label;
    }
}

public struct SpeakingLineContent
{
    public string speaker;
    public string lineText;
    public int lineNumber;

    public SpeakingLineContent(string speaker, string lineText, int lineNumber)
    {
        this.speaker = speaker;
        this.lineText = lineText;
        this.lineNumber = lineNumber;
    }
}

public struct ChoiceLineContent
{
    public string lineText;
    public int lineNumber;
    public string jumpLabel;

    public ChoiceLineContent(string speaker, string lineText, int lineNumber, string jumpLabel)
    {
        this.lineText = lineText;
        this.lineNumber = lineNumber;
        this.jumpLabel = jumpLabel;
    }
}