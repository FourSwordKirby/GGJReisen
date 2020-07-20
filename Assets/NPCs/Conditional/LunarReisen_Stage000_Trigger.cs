public class LunarReisen_Stage000_Trigger : DestroyIfTrue
{
    public override bool CheckCondition()
    {
        return GameProgress?.LunarReisen?.Stage > 0;
    }
}
