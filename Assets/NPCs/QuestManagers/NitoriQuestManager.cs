using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NitoriQuestManager : Npc
{
    public void Nitori_Stage000()
    {
        GameProgress.Nitori.Stage = 001;
    }

    public void Nitori_Stage001()
    {
        GameProgress.Nitori.Stage = 002;
    }

    public void Nitori_Stage002()
    {
        GameProgress.Nitori.Stage = 003;
    }

    public void Nitori_Stage003_Elixir()
    {
        AssignAvailableElixir(Assignment.Nitori);
        TransformToNormalSprite();
        GameProgress.Nitori.Stage = 1000;
    }

    public void Nitori_Stage003_Smartphone()
    {
        GameProgress.Player.Smartphone = Assignment.Nitori;
        GameProgress.Nitori.Stage = 100;
    }

    public void Nitori_Stage003_Wrench()
    {
        GameProgress.Player.Wrench = Assignment.Nitori;
        GameProgress.Nitori.Stage = 100;
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
        GameProgress.Nitori.Stage = 101;
    }

    public void Nitori_Stage101_Correct()
    {
        GameProgress.Player.AddShard(Shard.Nitori_QuestionCorrect);
        GameProgress.Nitori.Stage = 200;
    }

    public void Nitori_Stage101_Wrong()
    {
        GameProgress.Nitori.Stage = 200;
    }

    public void Nitori_Stage200()
    {
        GameProgress.Nitori.Stage = 201;
    }

    public void Nitori_Stage201_Elixir()
    {
        AssignAvailableElixir(Assignment.Nitori);
        TransformToNormalSprite();
        GameProgress.Nitori.Stage = 1000;
    }

    public void Nitori_Stage201_Smartphone()
    {
        GameProgress.Player.Smartphone = Assignment.Nitori;
        GameProgress.Player.Scroll = Assignment.Inventory;
        GameProgress.Nitori.Stage = 500;
    }

    public void Nitori_Stage201_Magazine()
    {
        GameProgress.Player.Magazine = Assignment.Nitori;
        if (GameProgress.Player.ShardsAcquired.Contains(Shard.Nitori_QuestionCorrect.Id))
        {
            GameProgress.Nitori.Stage = 301;
        }
        else
        {
            GameProgress.Nitori.Stage = 300;
        }
    }

    public void Nitori_Stage300()
    {
        GameProgress.Player.Scroll = Assignment.Inventory;
        GameProgress.Nitori.Stage = 501;
    }

    public void Nitori_Stage301()
    {
        TransformToNormalSprite();
        GameProgress.Nitori.Stage = 1100;
    }

    public void Nitori_Stage1000()
    {
        GameProgress.Player.AddShard(Shard.Nitori_Elixir);
        GameProgress.Nitori.Stage = 1001;
    }

    public void Nitori_Stage1100()
    {
        GameProgress.Player.AddShard(Shard.Nitori_GoodEnd);
        GameProgress.Nitori.Stage = 1101;
    }

    public void Nitori_Stage1101()
    {
        GameProgress.Player.Scroll = Assignment.Inventory;
        GameProgress.Nitori.Stage = 1102;
    }
}
