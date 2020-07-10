public class KosuzuQuestManager : Npc
{
    public void Kosuzu_Stage000_CoughMedicine()
    {
        GameProgress.Player.CoughingMedicine = Assignment.Kosuzu;
        GameProgress.Player.TextBook = Assignment.Inventory;
        GameProgress.Player.AddShard(Shard.Kosuzu_CoughingMedicine);
        GameProgress.Kosuzu.Stage = 100;
    }

    public void Kosuzu_Stage000_Elixir()
    {
        AssignAvailableElixir(Assignment.Kosuzu);
        GameProgress.Kosuzu.Stage = 1100;
    }

    public void Kosuzu_Stage100_Correct()
    {
        GameProgress.Player.AddShard(Shard.Kosuzu_QuestionCorrect);
        GameProgress.Kosuzu.Stage = 200;
    }

    public void Kosuzu_Stage100_Wrong()
    {
        GameProgress.Kosuzu.Stage = 200;
    }

    public void Kosuzu_Stage200_Elixir()
    {
        GameProgress.Kosuzu.Stage = 1100;
    }

    public void Kosuzu_Stage200_Magazine()
    {
        GameProgress.Player.Magazine = Assignment.Kosuzu;
        GameProgress.Player.AddShard(Shard.Kosuzu_MagazineBadEnd);
        GameProgress.Kosuzu.Stage = 600;
    }

    public void Kosuzu_Stage200_Scroll()
    {
        GameProgress.Player.Scroll = Assignment.Kosuzu;
        GameProgress.Kosuzu.Stage = 1000;
    }

    public void Kosuzu_Stage1000()
    {
        GameProgress.Player.AddShard(Shard.Kosuzu_GoodEnd);
        GameProgress.Kosuzu.Stage = 1001;
    }

    public void Kosuzu_Stage1001()
    {
        GameProgress.Player.Schematic = Assignment.Inventory;
    }

    public void Kosuzu_Stage1100()
    {
        GameProgress.Player.AddShard(Shard.Kosuzu_Elixir);
        GameProgress.Kosuzu.Stage = 1101;
    }
}
