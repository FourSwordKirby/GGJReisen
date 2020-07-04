using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

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
    public string lineLabel;

    /// <summary>
    /// Used for navigating a dialogue tree when navigating dialogue
    /// </summary>
    public string jumpLabel;

    /// <summary>
    /// Used for navigating a dialogue tree when navigating dialogue
    /// </summary>
    public ScriptLine previousLine;

    /// <summary>
    /// Used for navigating a dialogue tree when navigating dialogue
    /// </summary>
    public ScriptLine nextLine;

    /// <summary>
    /// Used for identifying the line it's on
    /// </summary>
    public int lineNumber;

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
    public ScriptLine jumpLine;

    public ChoiceLineContent(string speaker, string lineText, int lineNumber, string jumpLabel, ScriptLine jumpLine = null)
    {
        this.lineText = lineText;
        this.lineNumber = lineNumber;
        this.jumpLabel = jumpLabel;
        this.jumpLine = jumpLine;
    }
}