using System.Linq;
using UnityEngine;

public class MiyoiQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.Miyoi;

    int MiyoiSeenShardDialogue;

    public void Miyoi_Stage000()
    {
        if (GameProgress.Player.ShardsAcquired.Count >= 5)
            Stage = 10;
        else
            Stage = 100;
    }

    public void Miyoi_Stage010()
    {
        GameProgress.MiyoiShardSeenCount = 10;
        if (GameProgress.Player.ShardsAcquired.Count >= 10)
            Stage = 20;
        else
            Stage = 100;
    }

    public void Miyoi_Stage020()
    {
        GameProgress.MiyoiShardSeenCount = 15;
        if (GameProgress.Player.ShardsAcquired.Count >= 15)
            Stage = 30;
        else
            Stage = 100;
    }

    public void Miyoi_Stage030()
    {
        GameProgress.MiyoiShardSeenCount = 25;
        GameProgress.Player.AddShard(Shard.Miyoi_Company);
        DisplayShardTransaction(Shard.Miyoi_Company);
        Stage = 100;
    }

    public override void SyncNpcState(CharacterProgress characterProgress)
    {
        int currentStage = characterProgress.GetStage();

        //only try to play miyoi 10, 20, and 30 if we are at stage 100
        if(currentStage == 100)
        {
            int acquiredShardCount = GameProgress.Player.ShardsAcquired.Sum(x => x.ShardValue);
            MiyoiSeenShardDialogue = GameProgress.MiyoiShardSeenCount;

            if (MiyoiSeenShardDialogue < 10 && acquiredShardCount >= 5)
            {
                MiyoiSeenShardDialogue = 10;
                currentStage = 10;
            }
            else if (MiyoiSeenShardDialogue < 15 && acquiredShardCount >= 10)
            {
                MiyoiSeenShardDialogue = 20;
                currentStage = 20;
            }
            else if(MiyoiSeenShardDialogue < 25 && acquiredShardCount >= 15)
            {
                MiyoiSeenShardDialogue = 30;
                currentStage = 30;
            }
        }

        GameProgress.MiyoiShardSeenCount = MiyoiSeenShardDialogue;

        Stage = currentStage;
        TextAsset dialogueTextForStage = dialogues.Find(x => x.stage == currentStage).dialogue;
        dialoguePromptTrigger.SetDialogueText(dialogueTextForStage, !characterProgress.StageDialogueHasBeenRead);
    }
}
