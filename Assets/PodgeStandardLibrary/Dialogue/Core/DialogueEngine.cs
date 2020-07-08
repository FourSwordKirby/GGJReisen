using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text.RegularExpressions;

public delegate ScriptLine SpeakingLineGenerator(string speaker, string lineText, int lineNumber);
public delegate ScriptLine ExpressionLineGenerator(string speaker, CharacterExpression expression);
public delegate ScriptLine ChoiceLineGenerator(string speaker, List<ChoiceLineContent> choices);
public delegate ScriptLine InstructionLineGenerator(DialogueInstruction instruction);
public delegate bool LineChecker(List<string> conditions);


public class DialogueEngine
{
    public static SpeakingLineGenerator GenerateSpeakingLine;
    public static ExpressionLineGenerator GenerateExperssionLine;
    public static ChoiceLineGenerator GenerateChoiceLine;
    public static InstructionLineGenerator GenerateInstructionLine;
    public static LineChecker IsLineValid;
    static bool initialized;

    public static void InitializeGenerators(SpeakingLineGenerator speakingLineGenerator, ExpressionLineGenerator expressionLineGenerator,
                                            ChoiceLineGenerator choiceLineGenerator, InstructionLineGenerator instructionLineGenerator,
                                            LineChecker isLineValid)
    {
        GenerateSpeakingLine = speakingLineGenerator;
        GenerateExperssionLine = expressionLineGenerator;
        GenerateChoiceLine = choiceLineGenerator;
        GenerateInstructionLine = instructionLineGenerator;
        IsLineValid = isLineValid;
        initialized = true;
    }

    public static List<ScriptLine> CreateDialogueComponents(string text, List<DialogueInstruction> AvailableInstructions = null)
    {
        if (!initialized)
            throw new Exception("InitializeGenerators not yet called");

        List<string> rawLines = new List<string>(text.Split('\n'));
        rawLines = rawLines.Select(x => x.Trim()).ToList();
        rawLines = rawLines.Where(x => x != "").ToList();

        List<ScriptLine> processedLines = new List<ScriptLine>();

        string currentSpeaker = "";
        int speakingLineNumber = 0;
        Dictionary<string, ScriptLine> labeledLines = new Dictionary<string, ScriptLine>();

        for (int i = 0; i < rawLines.Count; i++)
        {
            ScriptLine processedLine = null;

            string line = rawLines[i];

            // Dealing with commented out lines
            if (line.StartsWith("//"))
                continue;

            // processing current speaker
            string speaker = GetSpeaker(line);
            if (speaker != "")
                currentSpeaker = speaker;
            else if (currentSpeaker == "")
                Debug.LogWarning("Speaker not specified");
            line = RemoveSpeaker(line);

            // processing jump statements
            string jump = GetJump(line);
            line = RemoveJump(line);

            // processing labels
            string label = GetLabel(line);
            line = RemoveLabel(line);

            // Checking if the line is valid based on what the game says
            // This is mainly used to filter out statements which should appear/not appear based on the game
            List<string> conditions = GetConditions(line);
            if (!IsLineValid(conditions))
                continue;
            line = RemoveConditions(line);

            switch (GetLineType(line))
            {
                case LineType.SpeakingLine:
                    processedLine = GenerateSpeakingLine(currentSpeaker, GetSpokenLine(line), speakingLineNumber);
                    speakingLineNumber++;
                    break;
                case LineType.Expression:
                    CharacterExpression desiredExpression = GetExpression(line);
                    processedLine = GenerateExperssionLine(currentSpeaker, desiredExpression);
                    break;
                case LineType.Choice:
                    List<ChoiceLineContent> choices = GetChoices(line, currentSpeaker, speakingLineNumber);
                    processedLine = GenerateChoiceLine(currentSpeaker, choices);
                    speakingLineNumber++;
                    break;
                case LineType.Instruction:
                    if (AvailableInstructions != null)
                    {
                        line = GetInstructionName(line);
                        DialogueInstruction instruction = AvailableInstructions.Find(x => x.name == line);
                        if(instruction != null)
                            processedLine = GenerateInstructionLine(instruction);
                        else
                            throw new Exception(string.Format("Instruction with name '{0}' not found", line));
                    }
                    else
                        throw new Exception("No instructions provided");
                    break;
            }

            // adding label to our repository of labels
            if (!string.IsNullOrEmpty(jump))
            {
                processedLine.jumpLabel = jump;
            }

            // adding label to our repository of labels
            if (!string.IsNullOrEmpty(label))
            {
                labeledLines.Add(label, processedLine);
                processedLine.lineLabel = label;
                processedLine.lineNumber = i;
            }

            processedLines.Add(processedLine);
        }

        // final scrub through of processed lines to set up proper routing to tags
        for (int i = 0; i < processedLines.Count; i++)
        {
            ScriptLine processedLine = processedLines[i];

            switch (processedLine.GetLineType())
            {
                case LineType.SpeakingLine:
                case LineType.Expression:
                    if (!string.IsNullOrEmpty(processedLine.jumpLabel))
                        try
                        {
                            processedLine.nextLine = labeledLines[processedLine.jumpLabel];
                        }
                        catch(Exception e)
                        {
                            Debug.Log(processedLine.jumpLabel);
                            throw e;
                        }
                    break;
                case LineType.Choice:
                    ((ChoiceLine)processedLine).InitJumps(labeledLines);
                    break;
            }
        }
        return processedLines;
    }

    private static List<string> GetConditions(string line)
    {
        List<string> conditions = new List<string>();
        if(line.StartsWith("?"))
        {
            MatchCollection matches = Regex.Matches(line, @"(?<=\{).+?(?=\})");
            // Use foreach-loop.
            foreach (Match match in matches)
            {
                foreach (Capture capture in match.Captures)
                {
                    string condition = capture.Value;
                    conditions.Add(condition);
                }
            }
        }
        return conditions;
    }

    private static string RemoveConditions(string line)
    {
        if (line.StartsWith("?"))
        {
            line = Regex.Replace(line, @"\{.+?\}", "");
            return line.Substring(1).Trim();
        }
        else
            return line;
    }


    private static string RemoveSpeaker(string line)
    {
        string[] dialoguePieces = line.Split(':');
        if (dialoguePieces.Length > 1)
            line = dialoguePieces[1];

        return line.Trim();
    }
    private static string RemoveLabel(string line)
    {
        if (!line.Contains("["))
            return line;

        string newLine = line.Substring(0, line.LastIndexOf('['));
        if (line.Substring(line.LastIndexOf("[")).Contains("|"))
            return line;
        else
            return newLine.Trim();
    }
    private static string RemoveJump(string line)
    {
        string[] jumpSplit = line.Split('(');
        if (jumpSplit.Length > 1)
            line = jumpSplit[0];

        return line.Trim();
    }

    static LineType GetLineType(string line)
    {
        if (line.StartsWith("[expression]"))
            return LineType.Expression;
        else if (line.StartsWith("[choice]"))
            return LineType.Choice;
        else if (line.StartsWith("[instruction]"))
            return LineType.Instruction;
        else
            return LineType.SpeakingLine;
    }

    public static string GetSpeaker(string line)
    {
        string[] dialoguePieces = line.Split(':');

        if (dialoguePieces.Length > 1)
            return dialoguePieces[0];
        else
            return "";
    }

    internal static string GetSpokenLine(string line)
    {
        string[] dialoguePieces = line.Split(':');

        if (dialoguePieces.Length > 1)
            return dialoguePieces[1];
        else
            return line;
    }

    /// <summary>
    /// Gets the label denoted by a {label} at the end of a line, like so
    /// 
    /// Anne: Today is a good day {good end}
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static string GetLabel(string line)
    {
        Regex regex = new Regex("(?<=^\\]).+?(?=\\[)");
        // reverse a string to make the non-greedy match work properly
        Match match = regex.Match(new string(line.Reverse().ToArray()));

        if (match.Success)
        {
            if(!match.Value.Contains("|"))
                return new string(match.Value.Reverse().ToArray());
            else
                return "";
        }
        else
            return "";
    }

    /// <summary>
    /// Gets the jump point denoted by a (label) at the end of a line, like so
    /// 
    /// Anne: Today is a good day (good end)
    /// 
    /// Natural limitation is that a jump label can't immediately go to a jump line with our hack implementation
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static string GetJump(string line)
    {
        Regex regex = new Regex("(?<=^\\)).+?(?=\\()");
        // reverse a string to make the non-greedy match work properly
        Match match = regex.Match(new string(line.Reverse().ToArray()));

        if (match.Success)
        {
            return new string(match.Value.Reverse().ToArray());
        }
        else
            return "";
    }

    private static CharacterExpression GetExpression(string line)
    {
        string expressionString = line.Split(' ')[1];
        return (CharacterExpression)Enum.Parse(typeof(CharacterExpression), expressionString);
    }


    /// <summary>
    /// Gets the label denoted by a {label} at the end of a line, like so
    /// 
    /// Anne: Today is a good day {good end}
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static List<ChoiceLineContent> GetChoices(string line, string speaker, int lineNumber)
    {
        List<ChoiceLineContent> dialogueChoices = new List<ChoiceLineContent>();

        MatchCollection matches = Regex.Matches(line, @"(?<=\[).+?(?=\])");
        // Use foreach-loop.
        foreach (Match match in matches)
        {
            foreach (Capture capture in match.Captures)
            {
                string choice = capture.Value;
                if (choice == "choice")
                    continue;

                string choiceLine = choice.Split('|')[0].Trim();
                string jumpLabel = choice.Split('|')[1].Trim();

                ChoiceLineContent content = new ChoiceLineContent(speaker, choiceLine, lineNumber, jumpLabel);

                dialogueChoices.Add(content);
            }
        }
        
        return dialogueChoices;
    }

    public static string GetInstructionName(string line)
    {
        string[] dialoguePieces = line.Split(']');
        if (dialoguePieces.Length > 1)
            line = dialoguePieces[1];

        return line.Trim();
    }

    public enum LineType
    {
        SpeakingLine,
        Expression,
        Choice,
        Instruction
    }
}
