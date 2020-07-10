using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public List<NpcDialogue> dialogues;
    public DialoguePromptTrigger dialoguePromptTrigger;

    public void InitNpcState(CharacterProgress characterProgress)
    {
        dialoguePromptTrigger.dialogue = dialogues.Find(x => x.stage == characterProgress.GetStage()).dialogue;
    }


    #region dialogue stage events
    public void SetKosuzuStage(int stage)
    {
        ReisenGameManager.instance.gameProgress.Kosuzu.Stage = stage;
    }

    public void SetKeineStage(int stage)
    {
        ReisenGameManager.instance.gameProgress.Keine.Stage = stage;
    }

    public void SetNitoriStage(int stage)
    {
        ReisenGameManager.instance.gameProgress.Nitori.Stage = stage;
    }

    public void SetAkyuStage(int stage)
    {
        ReisenGameManager.instance.gameProgress.Akyu.Stage = stage;
    }

    public void SetLunarReisenStage(int stage)
    {
        ReisenGameManager.instance.gameProgress.LunarResisen.Stage = stage;
    }

    public void SetMiyoiStage(int stage)
    {
        ReisenGameManager.instance.gameProgress.Miyoi.Stage = stage;
    }

    public void SetKogasa(int stage)
    {
        ReisenGameManager.instance.gameProgress.Kogasa.Stage = stage;
    }
    #endregion

    #region dialogue item events

    public void AssignCoughingMedicine(int assignment)
    {
        Debug.Log("here");
        ReisenGameManager.instance.gameProgress.Player.CoughingMedicine = (Assignment)assignment;
        Debug.Log(ReisenGameManager.instance.gameProgress.Player.CoughingMedicine);
    }

    public void AssignTextbook(int assignment)
    {

        ReisenGameManager.instance.gameProgress.Player.TextBook = (Assignment)assignment;
    }

    public void AssignEncyclopedia(int assignment)
    {

        ReisenGameManager.instance.gameProgress.Player.Encyclopedia = (Assignment)assignment;
    }

    public void AssignNewspaper(int assignment)
    {
        ReisenGameManager.instance.gameProgress.Player.Newspaper = (Assignment)assignment;

    }


    public void AssignSmartphone(int assignment)
    {
        ReisenGameManager.instance.gameProgress.Player.Smartphone = (Assignment)assignment;

    }

    public void AssignMagazine(int assignment)
    {
        ReisenGameManager.instance.gameProgress.Player.Magazine = (Assignment)assignment;

    }

    public void AssignWrench(int assignment)
    {
        ReisenGameManager.instance.gameProgress.Player.Wrench = (Assignment)assignment;

    }

    public void AssignScroll(int assignment)
    {
        ReisenGameManager.instance.gameProgress.Player.Scroll = (Assignment)assignment;

    }

    public void AssignSchematic(int assignment)
    {
        ReisenGameManager.instance.gameProgress.Player.Schematic = (Assignment)assignment;

    }

    public void AssignNovel(int assignment)
    {
        ReisenGameManager.instance.gameProgress.Player.Novel = (Assignment)assignment;

    }


    public void AssignElixir(int assignment)
    {
        ReisenGameManager.instance.gameProgress.Player.CoughingMedicine = (Assignment)assignment;

    }
    #endregion

    public void AddShard()
    {
        Shard s = new Shard("0", 0, "testing");
        ReisenGameManager.instance.gameProgress.Player.AddShard(s);
    }

    public ReisenGameProgress GameProgress => ReisenGameManager.instance.gameProgress;

    public void AssignAvailableElixir(Assignment a)
    {
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
}

[Serializable]
public class NpcDialogue
{
    public int stage;
    public TextAsset dialogue;
}