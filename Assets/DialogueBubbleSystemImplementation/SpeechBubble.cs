using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeechBubble : DialogueBubble, ISpeechBubble
{
    public TextMeshPro textMesh;

    public void SetDialogueBubbleContent(SpeakingLineContent content)
    {
        textMesh.text = content.lineText;
    }
}
