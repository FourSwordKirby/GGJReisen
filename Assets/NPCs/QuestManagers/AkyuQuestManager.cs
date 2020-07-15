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
        GameProgress.Player.AddShard(Shard.Akyu_Textbook);
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
        Stage = 1001;
    }

    public void Akyu_Stage1002()
    {
        GameProgress.Player.AddShard(Shard.Akyu_BadElixir2);
        Stage = 1001;
    }

    public void Akyu_Stage1100()
    {
        GameProgress.Player.AddShard(Shard.Akyu_GoodEnd);
        Stage = 1101;
    }
}
