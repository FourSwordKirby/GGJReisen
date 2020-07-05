using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceBubble : DialogueBubble
{
    public Transform choiceCenter;
    public List<ChoicePanel> choicePanels;
    public float panelSpacing = 0.55f;
    public float panelOffset = 0.3f;
    public float panelZOffset = 0.1f;

    public int chosenOptionIndex;

    public GameObject choicePanelPrefab;

    public void Instantiate(int choiceCount)
    {
        choicePanels = new List<ChoicePanel>();
        for(int i = 0; i < choiceCount; i++)
        {
            ChoicePanel choicePanel = Instantiate(choicePanelPrefab).GetComponent<ChoicePanel>();
            choicePanel.transform.parent = choiceCenter;

            choicePanel.gameObject.transform.localPosition = Vector3.up * (panelOffset - i * panelSpacing) 
                                                             + Vector3.up * panelSpacing * 0.5f * (choiceCount - 1) 
                                                             + Vector3.back * panelZOffset;
            choicePanels.Add(choicePanel);
        }
    }

    public override void Focus()
    {
        textFrame.material.color = focusColor;

        for(int i = 0; i < choicePanels.Count; i++)
        {
            ChoicePanel panel = choicePanels[i];
            panel.ChoiceBubbleInFocus = true;
            panel.Show();
            if(i == chosenOptionIndex)
                panel.Focus();
            else
                panel.Blur();
        }
    }

    public override void Blur()
    {
        textFrame.material.color = blurColor;

        for (int i = 0; i < choicePanels.Count; i++)
        {
            ChoicePanel panel = choicePanels[i];
            panel.ChoiceBubbleInFocus = false;
            if (i == chosenOptionIndex)
            {
                panel.Show();
                panel.Focus();
            }
            else
            {
                panel.Blur();
                panel.Hide();
            }
        }
    }

    internal void UpdateOption(int chosenOptionIndex)
    {
        this.chosenOptionIndex = chosenOptionIndex;
        for (int i = 0; i < choicePanels.Count; i++)
        {
            ChoicePanel panel = choicePanels[i];
            if (i == chosenOptionIndex)
            {
                panel.Show();
                panel.Focus();
            }
            else
                panel.Blur();
        }
    }
}