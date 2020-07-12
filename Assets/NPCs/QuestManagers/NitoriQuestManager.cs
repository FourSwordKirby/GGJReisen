﻿public class NitoriQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.Nitori;

    public void Nitori_Stage000()
    {
        Stage = 001;
    }

    public void Nitori_Stage001()
    {
        Stage = 002;
    }

    public void Nitori_Stage002()
    {
        Stage = 003;
    }

    public void Nitori_Stage003_Elixir()
    {
        AssignAvailableElixir(Assignment.Nitori);
        TransformToNormalSprite();
        Stage = 1000;
    }

    public void Nitori_Stage003_Smartphone()
    {
        GameProgress.Player.Smartphone = Assignment.Nitori;
        Stage = 100;
    }

    public void Nitori_Stage003_Wrench()
    {
        GameProgress.Player.Wrench = Assignment.Nitori;
        Stage = 100;
    }

    public void Nitori_Stage100()
    {
        if (GameProgress.Player.Smartphone == Assignment.Nitori)
        {
            GameProgress.Player.AddShard(Shard.Nitori_Tool_Smartphone);
        }
        else if (GameProgress.Player.Wrench == Assignment.Nitori)
        {
            GameProgress.Player.AddShard(Shard.Nitori_Tool_Wrench);
        }
        Stage = 101;
    }

    public void Nitori_Stage101_Correct()
    {
        GameProgress.Player.AddShard(Shard.Nitori_QuestionCorrect);
        Stage = 200;
    }

    public void Nitori_Stage101_Wrong()
    {
        Stage = 200;
    }

    public void Nitori_Stage200()
    {
        Stage = 201;
    }

    public void Nitori_Stage201_Elixir()
    {
        AssignAvailableElixir(Assignment.Nitori);
        TransformToNormalSprite();
        Stage = 1000;
    }

    public void Nitori_Stage201_Smartphone()
    {
        GameProgress.Player.Smartphone = Assignment.Nitori;
        GameProgress.Player.Scroll = Assignment.Inventory;
        Stage = 500;
    }

    public void Nitori_Stage201_Magazine()
    {
        GameProgress.Player.Magazine = Assignment.Nitori;
        if (GameProgress.Player.ShardsAcquired.Contains(Shard.Nitori_QuestionCorrect.Id))
        {
            Stage = 301;
        }
        else
        {
            Stage = 300;
        }
    }

    public void Nitori_Stage300()
    {
        GameProgress.Player.Scroll = Assignment.Inventory;
        Stage = 501;
    }

    public void Nitori_Stage301()
    {
        TransformToNormalSprite();
        Stage = 1100;
    }

    public void Nitori_Stage1000()
    {
        GameProgress.Player.AddShard(Shard.Nitori_Elixir);
        Stage = 1001;
    }

    public void Nitori_Stage1100()
    {
        GameProgress.Player.AddShard(Shard.Nitori_GoodEnd);
        Stage = 1101;
    }

    public void Nitori_Stage1101()
    {
        GameProgress.Player.Scroll = Assignment.Inventory;
        Stage = 1102;
    }
}
