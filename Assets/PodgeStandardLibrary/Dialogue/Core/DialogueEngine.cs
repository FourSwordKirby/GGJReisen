using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Text.RegularExpressions;

public delegate ScriptLine SpeakingLineGenerator(string speaker, string lineText, int lineNumber);
public delegate ScriptLine ExpressionLineGenerator(CharacterExpression expression);
public delegate ScriptLine ChoiceLineGenerator(List<ChoiceLineContent> choices);


public class DialogueEngine
{
    public static SpeakingLineGenerator GenerateSpeakingLine;
    public static ExpressionLineGenerator GenerateInstructionLine;
    public static ChoiceLineGenerator GenerateChoiceLine;
    static bool initialized;

    public static void InitializeGenerators(SpeakingLineGenerator speakingLineGenerator, ExpressionLineGenerator instructionLineGenerator, ChoiceLineGenerator choiceLineGenerator)
    {
        GenerateSpeakingLine = speakingLineGenerator;
        GenerateInstructionLine = instructionLineGenerator;
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
        for(int i = 0; i < rawLines.Count; i++)
        {
            ScriptLine processedLine = null;

            string line = rawLines[i];
            string label = GetLabel(line);

            switch (GetLineType(line))
            {
                case LineType.SpeakingLine:
                    string speaker = GetSpeaker(line);
                    if (speaker != "")
                        currentSpeaker = speaker;
                    else if (currentSpeaker == "")
                        Debug.LogWarning("Speaker not specified");
                    processedLine = GenerateSpeakingLine(currentSpeaker, GetSpokenLine(line), speakingLineNumber);
                    speakingLineNumber++;
                    break;
                case LineType.Expression:
                    CharacterExpression desiredExpression = GetExpression(line);
                    processedLine = GenerateInstructionLine(desiredExpression);
                    break;
                case LineType.Choice:
                    List<ChoiceLineContent> choices = GetChoices(line, currentSpeaker, speakingLineNumber);
                    processedLine = GenerateChoiceLine(choices);
                    speakingLineNumber++;
                    break;
            }
            processedLines.Add(processedLine);
        }

        return processedLines;
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
        Regex regex = new Regex("(?<=^}).+?(?={)");
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

        string allChoices =  line.Split(':') [1];
        MatchCollection matches = Regex.Matches(allChoices, @"(?<=\[).+?(?=\])");

        // Use foreach-loop.
        foreach (Match match in matches)
        {
            foreach (Capture capture in match.Captures)
            {
                string choice = capture.Value;
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
