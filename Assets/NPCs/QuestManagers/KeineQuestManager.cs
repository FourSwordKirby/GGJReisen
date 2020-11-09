using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.Keine;

    public void Keine_Stage000_CameraShift()
    {
        StartCoroutine(Stage000_CameraShift_Coroutine());
    }

    public IEnumerator Stage000_CameraShift_Coroutine()
    {
        var trigger = GameObject.FindObjectOfType<Keine_Stage000_Trigger>();
        CameraMan.instance.StartCinematicMode(trigger.CameraLoc3);
        while (!CameraMan.instance.InDesiredPosition())
        {
            if (Controls.confirmInputDown())
            {
                // Fast break.
                CameraMan.instance.StartCinematicMode(trigger.CameraLoc1);
                yield break;
            }
            yield return null;
        }

        float timeElapsed = 0f;
        while (timeElapsed < 2.6f)
        {
            if (Controls.confirmInputDown())
            {
                // Fast skip
                CameraMan.instance.StartCinematicMode(trigger.CameraLoc1);
                yield break;
            }
            yield return null;
        }

        CameraMan.instance.StartCinematicMode(trigger.CameraLoc1);
        while (!CameraMan.instance.InDesiredPosition())
        {
            if (Controls.confirmInputDown())
            {
                // Fast skip
                yield break;
            }
            yield return null;
        }
    }

    public void Keine_Stage000()
    {
        Stage = 100;

        string[] objectsToActive = new string[] { "SavePoint_FirstTime", "WestEntranceBlockUntilSavePoint", "EastEntranceBlockUntilSavePoint" };
        foreach (string objName in objectsToActive)
        {
            GameObject obj = GameObject.Find(objName);
            obj.transform.Find("TriggerZone").gameObject.SetActive(true);
        }
    }

    public void Keine_Stage100()
    {
        Stage = 101;
    }

    public void Keine_Stage101()
    {
        Stage = 103;
    }

    public void Keine_Stage103_Elixir()
    {
        AssignAvailableElixir(Assignment.Keine);
        TransformToNormalSprite();
        Stage = 1000;
    }

    public void Keine_Stage103_Textbook()
    {
        GameProgress.Player.TextBook = Assignment.Keine;

        // When trading textbook, increment Kosuzu's plotline too
        if (GameProgress.Kosuzu.Stage < 100)
        {
            GameProgress.Kosuzu.Stage = 100;
            GameProgress.Kosuzu.DialogueRead = false;
        }

        ReisenGameManager.instance.AddShard(Shard.Keine_TextBook);
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Shard +1", "History Book -1"});
    }

    public void Keine_Stage103_Newspaper()
    {
        GameProgress.Player.Newspaper = Assignment.Keine;
        ReisenGameManager.instance.AddShard(Shard.Keine_Newspaper);
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Shard +1", "Newspaper -1"});
    }

    public void Keine_Stage103_Conclude()
    {
        GameProgress.Player.Smartphone = Assignment.Inventory;
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Smartphone +1" });
        Stage = 199;
        MarkNextDialogueAsRead();
    }

    public void Keine_Stage199()
    {
        if (GameProgress.Player.Smartphone != Assignment.Inventory)
        {
            Stage = 200;
        }
    }

    public void Keine_Stage200_Correct()
    {
        ReisenGameManager.instance.AddShard(Shard.Keine_QuestionCorrect);
        DisplayShardTransaction(Shard.Keine_QuestionCorrect);
        Stage = 300;
    }

    public void Keine_Stage200_Wrong()
    {
        Stage = 300;
    }

    public void Keine_Stage300_Elixir()
    {
        AssignAvailableElixir(Assignment.Keine);
        TransformToNormalSprite();
        Stage = 1000;
    }

    public void Keine_Stage300_Encyclopedia()
    {
        GameProgress.Player.Encyclopedia = Assignment.Keine;
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Encyclopedia -1" });
        TransformToNormalSprite();
        Stage = 1100;
    }

    public void Keine_Stage1000()
    {
        ReisenGameManager.instance.AddShard(Shard.Keine_Elixir);
        DisplayShardTransaction(Shard.Keine_Elixir);
        Stage = 1001;
    }

    public void Keine_Stage1100()
    {
        ReisenGameManager.instance.AddShard(Shard.Keine_GoodEnd);
        DisplayShardTransaction(Shard.Keine_GoodEnd);
        Stage = 1102;
        MarkNextDialogueAsRead();
    }

    public void Keine_Stage1101()
    {
        // No longer in use
    }
}
