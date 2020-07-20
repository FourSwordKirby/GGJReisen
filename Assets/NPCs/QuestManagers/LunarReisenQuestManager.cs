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


}
