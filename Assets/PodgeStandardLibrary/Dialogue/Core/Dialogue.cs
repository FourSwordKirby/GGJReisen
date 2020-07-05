using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class Dialogue
{
    List<ScriptLine> lines;
    public int currentPosition = -1;

    const string END_LABEL = "end";

    private bool isFinished = false;
    public bool IsFinished { get => currentPosition >= lines.Count - 1 || (currentPosition > 0 && lines[currentPosition].jumpLabel == END_LABEL); }

    //temporary variables for getting this working with the current implementation
    public int speakingLineCount {
        get => lines.Where(x => x.GetLineType() == DialogueEngine.LineType.SpeakingLine).ToList().Count;
    }

    public Dialogue (List<ScriptLine> lines)
    {
        this.lines = lines;
        currentPosition = -1;
    }

    public void Init()
    {
        currentPosition = -1;
    }

    // Throws an exception if we try to get the next line when we already passed the current line count
    public ScriptLine GetNextLine()
    {
        if(currentPosition < lines.Count)
        {
            if(currentPosition == -1)
            {
                currentPosition = 0;
                ScriptLine line = lines[currentPosition];
                return line;
            }
            else
            {
                ScriptLine line = lines[currentPosition];
                ScriptLine nextLine = line.GetNextLine();
                if(nextLine == null)
                {
                    currentPosition++;
                    if (currentPosition < lines.Count)
                        return lines[currentPosition];
                    else
                        return null;
                }
                else
                {
                    currentPosition = nextLine.lineNumber;
                    return nextLine;
                }
            }
        }
        else
            throw new System.Exception("Reached the end of the dialogue, there is no next line");
    }

    public ScriptLine GetPreviousLine()
    {
        if (currentPosition >= 0)
        {
            if (currentPosition >= lines.Count)
            {
                currentPosition = lines.Count-1;
                ScriptLine line = lines[currentPosition];
                return line;
            }
            else
            {
                ScriptLine line = lines[currentPosition];
                ScriptLine previousLine = line.GetPreviousLine();
                if (previousLine == null)
                {
                    currentPosition--;
                    if (currentPosition >= 0)
                        return lines[currentPosition];
                    else
                        return null;
                }
                else
                {
                    currentPosition = previousLine.lineNumber;
                    return previousLine;
                }
            }
        }
        else
            throw new System.Exception("Reached the start of the dialogue, there is no previous line");
    }
}