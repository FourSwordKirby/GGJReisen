using System.Collections.Generic;

public class KosuzuQuestManager : Npc
{
    public override ReisenNpcCharacterProgress NpcProgress => GameProgress.Kosuzu;

    public void Kosuzu_Stage000_CoughMedicine()
    {
        GameProgress.Player.CoughMedicine = Assignment.Kosuzu;
        GameProgress.Player.TextBook = Assignment.Inventory;
        GameProgress.Player.AddShard(Shard.Kosuzu_CoughMedicine);
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Cough Medicine -1", "History Book +1", "Shard +1" });
        Stage = 99;
        MarkNextDialogueAsRead();
    }

    public void Kosuzu_Stage000_Elixir()
    {
        AssignAvailableElixir(Assignment.Kosuzu);
        TransformToNormalSprite();
        Stage = 1100;
    }

    public void Kosuzu_Stage100_Correct()
    {
        GameProgress.Player.AddShard(Shard.Kosuzu_QuestionCorrect);
        DisplayShardTransaction(Shard.Kosuzu_QuestionCorrect);
        Stage = 200;
    }

    public void Kosuzu_Stage100_Wrong()
    {
        Stage = 200;
    }

    public void Kosuzu_Stage200_Elixir()
    {
        AssignAvailableElixir(Assignment.Kosuzu);
        TransformToNormalSprite();
        Stage = 1100;
    }

    public void Kosuzu_Stage200_Magazine()
    {
        GameProgress.Player.Magazine = Assignment.Kosuzu;
        GameProgress.Player.AddShard(Shard.Kosuzu_MagazineBadEnd);
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Magazine -1", "Shard +1" });
        Stage = 600;
    }

    public void Kosuzu_Stage200_Scroll()
    {
        GameProgress.Player.Scroll = Assignment.Kosuzu;
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Scroll -1" });
        TransformToNormalSprite();
        Stage = 1000;
    }

    public void Kosuzu_Stage1000()
    {
        GameProgress.Player.AddShard(Shard.Kosuzu_GoodEnd);
        DisplayShardTransaction(Shard.Kosuzu_GoodEnd);
        Stage = 1001;
    }

    public void Kosuzu_Stage1001()
    {
        GameProgress.Player.Schematic = Assignment.Inventory;
        ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Schematic +1" });
        Stage = 1002;
        MarkNextDialogueAsRead();
    }

    public void Kosuzu_Stage1100()
    {
        GameProgress.Player.AddShard(Shard.Kosuzu_Elixir);
        DisplayShardTransaction(Shard.Kosuzu_Elixir);
        Stage = 1101;
        MarkNextDialogueAsRead();
    }
}
