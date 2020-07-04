using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text.RegularExpressions;

public delegate ScriptLine SpeakingLineGenerator(string speaker, string lineText, int lineNumber);
public delegate ScriptLine ExpressionLineGenerator(string speaker, CharacterExpression expression);
public delegate ScriptLine ChoiceLineGenerator(string speaker, List<ChoiceLineContent> choices);


public class DialogueEngine
{
    public static SpeakingLineGenerator GenerateSpeakingLine;
    public static ExpressionLineGenerator GenerateExperssionLine;
    public static ChoiceLineGenerator GenerateChoiceLine;
    static bool initialized;

    public static void InitializeGenerators(SpeakingLineGenerator speakingLineGenerator, ExpressionLineGenerator instructionLineGenerator, ChoiceLineGenerator choiceLineGenerator)
    {
        GenerateSpeakingLine = speakingLineGenerator;
        GenerateExperssionLine = instructionLineGenerator;
        GenerateChoiceLine = choiceLineGenerator;
        initialized = true;
    }

    public static List<ScriptLine> CreateDialogueComponents(string text)
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

            // processing current speaker
            string speaker = GetSpeaker(line);
            if (speaker != "")
                currentSpeaker = speaker;
            else if (currentSpeaker == "")
                Debug.LogWarning("Speaker not specified");

            // processing labels
            string label = GetLabel(line);

            // processing jump statements
            string jump = GetJump(line);

            line = RemoveMetaData(line);

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
                        processedLine.nextLine = labeledLines[processedLine.jumpLabel];
                    break;
                case LineType.Choice:
                    ((ChoiceLine)processedLine).InitJumps(labeledLines);
                    break;
            }
        }
        return processedLines;
    }

    private static string RemoveMetaData(string line)
    {
        string[] dialoguePieces = line.Split(':');

        if (dialoguePieces.Length > 1)
            line = dialoguePieces[1];


        string[] tagSplit = line.Split('{');
        if (tagSplit.Length > 1)
            line = tagSplit[0];

        string[] jumpSplit = line.Split('(');
        if (jumpSplit.Length > 1)
            line = jumpSplit[0];

        return line;
    }

    static LineType GetLineType(string line)
    {
        if (line.StartsWith("[expression]"))
            return LineType.Expression;
        else if (line.StartsWith("[choice]"))
            return LineType.Choice;
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
        Regex regex = new Regex("(?<=^\\}).+?(?=\\{)");
        // reverse a string to make the non-greedy match work properly
        Match match = regex.Match(new string(line.Reverse().ToArray()));

        if (match.Success)
        {
            Debug.Log(new string(match.Value.Reverse().ToArray()));
            return new string(match.Value.Reverse().ToArray());
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
            Debug.Log(new string(match.Value.Reverse().ToArray()));
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

    public enum LineType
    {
        SpeakingLine,
        Expression,
        Choice
    }
}
