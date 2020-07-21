﻿using System.Collections.Generic;

public class AkyuQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.Akyu;

    public void Akyu_Stage000()
    {
        Stage = 001;
    }

    public void Akyu_Stage001_Elixir()
    {
        AssignAvailableElixir(Assignment.Akyu);
        TransformToNormalSprite();
        Stage = 1000;
    }

    public void Akyu_Stage001_Textbook()
    {
        GameProgress.Player.TextBook = Assignment.Akyu;

        // When trading textbook, increment Kosuzu's plotline too
        if (GameProgress.Kosuzu.Stage < 100)
        {
            GameProgress.Kosuzu.Stage = 100;
        }

        GameProgress.Player.AddShard(Shard.Akyu_Textbook);
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "History Book -1, Shard +1" });

        Stage = 100;
    }

    public void Akyu_Stage100()
    {
        Stage = 101;
    }

    public void Akyu_Stage101_Elixir()
    {
        AssignAvailableElixir(Assignment.Akyu);
        TransformToNormalSprite();
        Stage = 1002;
    }

    public void Akyu_Stage101_Novel()
    {
        GameProgress.Player.Novel = Assignment.Akyu;
        GameProgress.Player.AddShard(Shard.Akyu_Novel);
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Novel -1, Shard +1" });
        Stage = 200;
    }

    public void Akyu_Stage200_Elixir()
    {
        AssignAvailableElixir(Assignment.Akyu);
        TransformToNormalSprite();
        Stage = 1100;
    }

    public void Akyu_Stage1000()
    {
        GameProgress.Player.AddShard(Shard.Akyu_BadElixir1);
        DisplayShardTransaction(Shard.Akyu_BadElixir1);
        Stage = 1001;
    }

    public void Akyu_Stage1002()
    {
        GameProgress.Player.AddShard(Shard.Akyu_BadElixir2);
        DisplayShardTransaction(Shard.Akyu_BadElixir2);
        Stage = 1001;
    }

    public void Akyu_Stage1100()
    {
        GameProgress.Player.AddShard(Shard.Akyu_GoodEnd);
        DisplayShardTransaction(Shard.Akyu_GoodEnd);
        Stage = 1101;
    }
}
