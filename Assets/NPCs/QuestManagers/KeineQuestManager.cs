using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeineQuestManager : Npc
{
    public void Keine_Stage000()
    {
        GameProgress.Keine.Stage = 100;
    }

    public void Keine_Stage100()
    {
        GameProgress.Keine.Stage = 101;
    }

    public void Keine_Stage101()
    {
        GameProgress.Keine.Stage = 103;
    }

    public void Keine_Stage103_Elixir()
    {
        AssignAvailableElixir(Assignment.Keine);
        GameProgress.Keine.Stage = 1000;
    }

    public void Keine_Stage103_Book()
    {
        GameProgress.Player.TextBook = Assignment.Keine;
        GameProgress.Player.AddShard(Shard.Keine_TextBook);
        GameProgress.Keine.Stage = 200;
    }

    public void Keine_Stage103_Newspaper()
    {
        GameProgress.Player.Newspaper = Assignment.Keine;
        GameProgress.Player.AddShard(Shard.Keine_Newspaper);
        GameProgress.Keine.Stage = 200;
    }

    public void Keine_Stage200_Correct()
    {
        GameProgress.Player.AddShard(Shard.Keine_QuestionCorrect);
        GameProgress.Keine.Stage = 300;
    }

    public void Keine_Stage200_Wrong()
    {
        GameProgress.Keine.Stage = 300;
    }

    public void Keine_Stage300_Elixir()
    {
        AssignAvailableElixir(Assignment.Keine);
        GameProgress.Keine.Stage = 1000;
    }

    public void Keine_Stage300_Encyclopedia()
    {
        GameProgress.Player.Encyclopedia = Assignment.Keine;
        GameProgress.Keine.Stage = 1100;
    }

    public void Keine_Stage1000()
    {
        GameProgress.Player.AddShard(Shard.Keine_Elixir);
        GameProgress.Keine.Stage = 1001;
    }

    public void Keine_Stage1100()
    {
        GameProgress.Player.AddShard(Shard.Keine_GoodEnd);
        GameProgress.Keine.Stage = 1101;
    }

    public void Keine_Stage1101()
    {
        GameProgress.Player.Smartphone = Assignment.Inventory;
        GameProgress.Keine.Stage = 1102;
    }
}
