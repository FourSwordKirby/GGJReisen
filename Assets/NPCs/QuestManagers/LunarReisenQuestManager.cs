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
        TransformToNormalSprite();
        Stage = 1200;
    }

    public void LunarReisen_Stage003_Smartphone()
    {
        GameProgress.Player.AddShard(Shard.LunarReisen_Smartphone);
        GameProgress.Player.Smartphone = Assignment.LunarReisen;
        GameProgress.Player.Magazine = Assignment.Inventory;
        Stage = 100;
    }

    public void LunarReisen_Stage003_Wrench()
    {
        GameProgress.Player.Wrench = Assignment.LunarReisen;
        // No reward, no stage advancement.
    }

    public void LunarReisen_Stage100()
    {
        Stage = 101;
    }
}
