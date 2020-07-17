using UnityEngine;

public class MiyoiQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.Miyoi;

    bool stage_0_read;
    bool stage_10_read;
    bool stage_20_read;
    bool stage_30_read;

    public void Miyoi_Stage000()
    {
        stage_0_read = true;
        if (GameProgress.Player.ShardsAcquired.Count > 5)
            Stage = 10;
        else
            Stage = 100;
    }

    public void Miyoi_Stage010()
    {
        stage_10_read = true;
        if (GameProgress.Player.ShardsAcquired.Count > 10)
            Stage = 20;
        else
            Stage = 100;
    }

    public void Miyoi_Stage020()
    {
        stage_20_read = true;
        if (GameProgress.Player.ShardsAcquired.Count > 20)
            Stage = 30;
        else
            Stage = 100;
    }

    public void Miyoi_Stage030()
    {
        stage_30_read = true;
        GameProgress.Player.AddShard(Shard.Miyoi_Company);
        Stage = 100;
    }

    public override void InitNpcState(CharacterProgress characterProgress)
    {
        int currentStage = characterProgress.GetStage();

         if (GameProgress.Player.ShardsAcquired.Count < 5 && !stage_0_read)
             currentStage = 0;
         else if (GameProgress.Player.ShardsAcquired.Count < 10 && !stage_10_read)
            currentStage = 10;
         else if (GameProgress.Player.ShardsAcquired.Count < 20 && !stage_20_read)
            currentStage = 20;
         else if (GameProgress.Player.ShardsAcquired.Count >= 20 && !stage_30_read)
             currentStage = 30;

        Stage = currentStage;
        TextAsset dialogueTextForStage = dialogues.Find(x => x.stage == currentStage).dialogue;
        dialoguePromptTrigger.SetDialogueText(dialogueTextForStage, !characterProgress.StageDialogueHasBeenRead);
    }
}
