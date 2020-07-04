using UnityEngine;
using System;

public class SpeakingLine : ScriptLine
{
    SpeakingLineContent content;
    DialogueAnimator speakerAnimator;

    public SpeakingLine(string speaker, string lineText, int lineNumber, string label = "" ): base(label)
    {
        content = new SpeakingLineContent(speaker, lineText, lineNumber);

        try
        {
            // very per game specific stuff
            speakerAnimator = GameObject.Find(speaker).GetComponent<DialogueAnimator>();
        }
        catch(Exception e)
        {
            Debug.Log("attempted speaker is " + speaker);
            throw e;
        }
    }

    public static SpeakingLine CreateSpeakingLine(string speaker, string lineText, int lineNumber)
    {
        SpeakingLine line = new SpeakingLine(speaker, lineText, lineNumber);

        return line;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        Vector3 speakerPosition = speakerAnimator.getSpeechOrigin();

        DialogueBubbleUI.instance.DisplaySpeechBubble(content, speakerPosition);
    }

    public override bool IsFinished()
    {
        return DialogueBubbleUI.instance.ready;
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