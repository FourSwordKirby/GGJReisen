using System.Collections.Generic;

public class LunarReisenQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.LunarReisen;

    public void LunarReisen_Stage000()
    {
        Stage = 001;
    }

    public void LunarReisen_Stage001()
    {
        Stage = 002;
    }

    public void LunarReisen_Stage002()
    {
        Stage = 003;
    }

    public void LunarReisen_Stage003_Elixir()
    {
        AssignAvailableElixir(Assignment.LunarReisen);
        TransformToNormalSprite();
        Stage = 1200;
    }

    public void LunarReisen_Stage003_Smartphone()
    {
        GameProgress.Player.AddShard(Shard.LunarReisen_Smartphone);
        GameProgress.Player.Smartphone = Assignment.LunarReisen;
        GameProgress.Keine.Stage = 200; 

        GameProgress.Player.Magazine = Assignment.Inventory;

        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Smartphone -1", "Magazine +1", "Shard + 1" });
        Stage = 100;
    }

    public void LunarReisen_Stage003_Wrench()
    {
        GameProgress.Player.Wrench = Assignment.LunarReisen;
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Wrench -1" });
        // No reward, no stage advancement.
    }

    public void LunarReisen_Stage100()
    {
        Stage = 101;
        // Maybe there's an animation here for Lunar Reisen to run around.
    }

    public void LunarReisen_Stage101()
    {
        Stage = 102;
        // Maybe there's code here to make Lunar Reisen stop running around
    }

    public void LunarReisen_Stage102_Schematic()
    {
        GameProgress.Player.Schematic = Assignment.LunarReisen;
        GameProgress.Player.Novel = Assignment.Inventory;
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Schematic -1, Novel +1" });
        TransformToNormalSprite();
        Stage = 1000;
    }

    public void LunarReisen_Stage1000()
    {
        GameProgress.Player.AddShard(Shard.LunarReisen_GoodEnd);
        DisplayShardTransaction(Shard.LunarReisen_GoodEnd);
        Stage = 1001;
    }

    public void LunarReisen_Stage1001()
    {
        ReisenGameManager.instance.StartEnding();
        // This is the game ending instruction
    }

    public void LunarReisen_Stage1200()
    {
        GameProgress.Player.AddShard(Shard.LunarReisen_Elixir);
        DisplayShardTransaction(Shard.LunarReisen_Elixir);
        Stage = 1201;
    }
}
