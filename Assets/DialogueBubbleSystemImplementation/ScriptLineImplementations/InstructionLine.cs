using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;


[Serializable]
public class DialogueInstruction
{
    public string name;
    public UnityEvent action;
}

public class InstructionLine : ScriptLine
{
    private DialogueInstruction instruction;

    public InstructionLine(DialogueInstruction instruction)
    {
        this.instruction = instruction;
    }

    public static InstructionLine GenerateInstructionline(DialogueInstruction instruction)
    {
        InstructionLine line = new InstructionLine(instruction);

        return line;
    }

    //Change this based on the game implementation
    public override void PerformLine()
    {
        instruction.action?.Invoke();
    }

    public override bool IsFinished()
    {
        return true;
    }

    public override DialogueEngine.LineType GetLineType()
    {
        return DialogueEngine.LineType.Instruction;
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