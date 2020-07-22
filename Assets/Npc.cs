using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public List<NpcDialogue> dialogues;
    public DialoguePromptTrigger dialoguePromptTrigger;

    public GameObject cubeForm;
    public GameObject stickForm;

    public virtual ReisenNpcCharacterProgress NpcProgress
    {
        get
        {
            Debug.LogWarning($"A Generic NPC is not expected to have any CharacterProgress state. Be sure to override the {nameof(NpcProgress)} property for each specific character");
            return null;
        }
    }

    public int Stage
    {
        get
        {
            return NpcProgress.Stage;
        }
        set
        {
            NpcProgress.Stage = value;
            StageWasUpdated = true;
        }
    }

    public bool StageWasUpdated = false;
    public bool NextDialogueIsRead = false;

    public virtual void SyncNpcState(CharacterProgress characterProgress)
    {
        if (characterProgress.GetStage() >= 1000)
        {
            this.GetComponentInChildren<CharacterExpressionAnimator>().changeExpression(CharacterExpression.normal);
            stickForm.SetActive(true);
            cubeForm.SetActive(false);
        }

        TextAsset dialogueTextForStage = dialogues.Find(x => x.stage == characterProgress.GetStage()).dialogue;
        dialoguePromptTrigger.SetDialogueText(dialogueTextForStage, !characterProgress.StageDialogueHasBeenRead);
    }

    public ReisenGameProgress GameProgress => ReisenGameManager.instance.gameProgress;

    public void AssignAvailableElixir(Assignment a)
    {
        //hacky and exists outside the procedure for all other transaction displays but should be harmless
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Elixir -1" });
        if (GameProgress.Player.Elixir1 == Assignment.Inventory)
        {
            GameProgress.Player.Elixir1 = a;
        }
        else if (GameProgress.Player.Elixir2 == Assignment.Inventory)
        {
            GameProgress.Player.Elixir2 = a;
        }
        else
        {
            throw new InvalidOperationException("Tried to assign Elixir, but player does not have any in inventory");
        }
    }

    public void DisplayShardTransaction(Shard s)
    {
        int value = s.ShardValue;
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Shard +" + value });
    }

    public void TransformToNormalSprite()
    {
        // TODO
        // For now a placeholder to know where we need it
        Debug.Log("tranforming to normal");
        StartCoroutine(TranformationSequence());
    }

    IEnumerator TranformationSequence()
    {
        this.GetComponentInChildren<CharacterExpressionAnimator>().changeExpression(CharacterExpression.normal);
        yield return new WaitForSeconds(0.2f);
        AudioMaster.instance.PlayTransformSfx();
        stickForm.SetActive(true);
        cubeForm.SetActive(false);
    }

    public void MarkNextDialogueAsRead()
    {
        NextDialogueIsRead = true;
    }

    public void DefaultDialogueEndEvent()
    {
        NpcProgress.DialogueRead = NextDialogueIsRead || !StageWasUpdated;

        StageWasUpdated = false;
        NextDialogueIsRead = false;
    }
}

[Serializable]
public class NpcDialogue
{
    public int stage;
    public TextAsset dialogue;
}