using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneSequence : MonoBehaviour
{
    public TextAsset dialogueTextAsset;
    public Dialogue dialogue;
    public List<DialogueInstruction> AvailableInstructions;

    public bool playOnAwake;

    public string cutsceneMusic = "";

    // Start is called before the first frame update
    void Start()
    {
        DialogueEngine.InitializeGenerators(CutsceneSpeakingLine.CreateSpeakingLine, ExpressionLine.CreateInstructionLine, ChoiceLine.GenerateChoiceLine, InstructionLine.GenerateInstructionline, StallLine.GenerateStallLine, (x) => true);
        List<ScriptLine> lines = DialogueEngine.CreateDialogueComponents(dialogueTextAsset.text, AvailableInstructions);
        HashSet<string> speakers = new HashSet<string>(lines.Select(x => x.speaker).Distinct());
        dialogue = new Dialogue(lines, speakers);

        if(playOnAwake)
            StartCoroutine(PlayCutscene());
    }

    internal IEnumerator PlayCutscene()
    {
        if(!string.IsNullOrWhiteSpace(cutsceneMusic))
            AudioMaster.instance.PlayTrack(cutsceneMusic);

        int lineTracker = 0;
        //string currentSpeaker = "";
        while (!dialogue.IsFinished)
        {
            ScriptLine line = dialogue.GetNextLine();
            line.PerformLine();

            while (!line.IsFinished())
            {
                yield return null;
            }
            lineTracker++;
        }
    }
}
