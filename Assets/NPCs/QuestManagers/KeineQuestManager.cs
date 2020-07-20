using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.Keine;

    public void Keine_Stage000()
    {
        Stage = 100;
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
        }

        GameProgress.Player.AddShard(Shard.Keine_TextBook);
        Stage = 200;
    }

    public void Keine_Stage103_Newspaper()
    {
        GameProgress.Player.Newspaper = Assignment.Keine;
        GameProgress.Player.AddShard(Shard.Keine_Newspaper);
        Stage = 200;
    }

    public void Keine_Stage200_Correct()
    {
        GameProgress.Player.AddShard(Shard.Keine_QuestionCorrect);
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
        TransformToNormalSprite();
        Stage = 1100;
    }

    public void Keine_Stage1000()
    {
        GameProgress.Player.AddShard(Shard.Keine_Elixir);
        Stage = 1001;
    }

    public void Keine_Stage1100()
    {
        GameProgress.Player.AddShard(Shard.Keine_GoodEnd);
        Stage = 1101;
    }

    public void Keine_Stage1101()
    {
        GameProgress.Player.Smartphone = Assignment.Inventory;
        Stage = 1102;
    }
}
