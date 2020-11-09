using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


public class StallLine : ScriptLine
{
    private float startTime;
    private float stallTime;

    public StallLine(float stallTime)
    {
        this.stallTime = stallTime;
    }

    public static StallLine GenerateStallLine(float stallTime)
    {
        StallLine line = new StallLine(stallTime);

        return line;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        startTime = Time.realtimeSinceStartup;
    }

    public override bool IsFinished()
    {
        return Time.realtimeSinceStartup - startTime > stallTime;
    }

    public override DialogueEngine.LineType GetLineType()
    {
        return DialogueEngine.LineType.Stall;
    }

    public override ScriptLine GetPreviousLine()
    {
        return null;
    }

    public override ScriptLine GetNextLine()
    {
        return nextLine;
    }
}