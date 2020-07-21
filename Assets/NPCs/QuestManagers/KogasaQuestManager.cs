using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KogasaQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.Kogasa;

    public Rigidbody selfBody;
    public Collider ECB;
    public DialoguePromptTrigger trigger;

    public GameObject CollisionBox;

    bool hiding = true;

    public void Boo()
    {
        StartCoroutine(BooSequence());
    }

    public IEnumerator BooSequence()
    {

        selfBody.AddForce(Vector3.up * 800.0f);
        selfBody.useGravity = true;
        yield return new WaitForFixedUpdate();
        while (ECB.transform.position.y < 1.0f)
        {
            yield return new WaitForFixedUpdate();
        }
        ECB.enabled = true;
        trigger.forceDialogueOnEnter = false;
        trigger.speakingPositionConfig = DialogueTriggerSpeakerConfig.ForceRight;
        yield return null;
    }


    public void Kogasa_Stage000()
    {
        Stage = 100;
        hiding = false;
        selfBody.useGravity = false;
        CollisionBox.SetActive(true);
        ECB.gameObject.SetActive(false);
        GameProgress.KogasaCount++;
    }

    public void Kogasa_Stage200()
    {
        Stage = 100;
        hiding = false;
        selfBody.useGravity = false;
        CollisionBox.SetActive(true);
        ECB.gameObject.SetActive(false);
        GameProgress.KogasaCount++;
    }

    public void Kogasa_Stage300()
    {
        Stage = 100;
        hiding = false;
        selfBody.useGravity = false;
        CollisionBox.SetActive(true);
        ECB.gameObject.SetActive(false);
        GameProgress.Player.AddShard(Shard.Kogasa_Spook);
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Shard +1" });
        GameProgress.KogasaCount++;
    }


    public override void InitNpcState(CharacterProgress characterProgress)
    {
        int currentStage = characterProgress.GetStage();

        if(hiding)
        {
            if (GameProgress.KogasaCount == 0)
                currentStage = 0;
            else if (GameProgress.KogasaCount == 2)
                currentStage = 300;
            else if (GameProgress.KogasaCount < 2)
                currentStage = 200;
            else
            {
                currentStage = 100;
                selfBody.gameObject.transform.position -= Vector3.up * selfBody.gameObject.transform.position.y;
                trigger.forceDialogueOnEnter = false;
                trigger.speakingPositionConfig = DialogueTriggerSpeakerConfig.ForceRight;
                CollisionBox.SetActive(true);
                ECB.gameObject.SetActive(false);
            }
        }

        Stage = currentStage;
        TextAsset dialogueTextForStage = dialogues.Find(x => x.stage == currentStage).dialogue;
        dialoguePromptTrigger.SetDialogueText(dialogueTextForStage, !characterProgress.StageDialogueHasBeenRead);
    }
}
