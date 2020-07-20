using Assets.PodgeStandardLibrary.RPGSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


[Serializable]
public class Shard
{
    public string Id;
    public int ShardValue;
    public string FriendlyName;
    public string Description;

    public Shard(string id, int shardValue, string friendlyName, string description)
    {
        Id = id;
        ShardValue = shardValue;
        FriendlyName = friendlyName;
        Description = description;
    }

    public static Shard Keine_TextBook { get; } = new Shard(nameof(Keine_TextBook), 1, "Don't forget your textbook!", "Well, figures that a history teacher teaches from a history text book.");
    public static Shard Keine_Elixir { get; } = new Shard(nameof(Keine_Elixir), 1, "Medical History", "I better work fast to resolve the incident before Keine kills me...");
    public static Shard Keine_Newspaper { get; } = new Shard(nameof(Keine_Newspaper), 1, "Scandal with the Teacher!", "The Bunbunmaru newspaper sure doesn't hesistate to kill reputations.");
    public static Shard Keine_QuestionCorrect { get; } = new Shard(nameof(Keine_QuestionCorrect), 1, "Get lectured by the teacher", "\"Those who fail to learn from history are doomed to repeat it.\" so the humans say.");
    public static Shard Keine_GoodEnd { get; } = new Shard(nameof(Keine_GoodEnd), 2, "Rich History", "\"A rich history is one with many relationships.\" Maybe I can put a little more effort...");

    public static Shard Kosuzu_CoughingMedicine { get; } = new Shard(nameof(Kosuzu_CoughingMedicine), 1, "Medicine Peddler Arrives!", "Wonder if the coughing was due to allergies.");
    public static Shard Kosuzu_QuestionCorrect { get; } = new Shard(nameof(Kosuzu_QuestionCorrect), 1, "Youkai Enthusiast", "Living dangerously. Well it might be fun... Don't get me involved though.");
    public static Shard Kosuzu_MagazineBadEnd { get; } = new Shard(nameof(Kosuzu_MagazineBadEnd), 1, "Buy and Sell Used Books", "I never asked for the book shop keeper's name, huh?");
    public static Shard Kosuzu_GoodEnd { get; } = new Shard(nameof(Kosuzu_GoodEnd), 2, "Suzunaan, Opened for Business", "Looks like the book shop is in order again.");
    public static Shard Kosuzu_Elixir { get; } = new Shard(nameof(Kosuzu_Elixir), 1, "A New Incident Solver on the Case!", "Perfectly honest, I don't think that human is able to do anything about this incident...");

    public static Shard Nitori_Tool_Smartphone { get; } = new Shard(nameof(Nitori_Tool_Smartphone), 1, "", "");
    public static Shard Nitori_Tool_Wrench { get; } = new Shard(nameof(Nitori_Tool_Wrench), 1, "", "");
    public static Shard Nitori_QuestionCorrect { get; } = new Shard(nameof(Nitori_QuestionCorrect), 1, "", "");
    public static Shard Nitori_GoodEnd { get; } = new Shard(nameof(Nitori_GoodEnd), 2, "", "");
    public static Shard Nitori_Elixir { get; } = new Shard(nameof(Nitori_Elixir), 1, "", "");

    public static Shard Akyu_Textbook { get; } = new Shard(nameof(Akyu_Textbook), 1, "Book Shop Intern", "I met the Gensokyo Chronicler... she was singing a weird song to herself.");
    public static Shard Akyu_Novel { get; } = new Shard(nameof(Akyu_Novel), 1, "Case closed!", "A chronicler working as novelist as a side job. I wonder if she gets fact and fiction mixed up.");
    public static Shard Akyu_BadElixir1 { get; } = new Shard(nameof(Akyu_BadElixir1), 1, "Malfunctioning Elixir?", "I cured the Hieda girl's cube form, but it seems like she's still forgetting something...");
    public static Shard Akyu_BadElixir2 { get; } = new Shard(nameof(Akyu_BadElixir2), 1, "Not All Powerful", "Master Yagokoro's elixir didn't work!?");
    public static Shard Akyu_GoodEnd { get; } = new Shard(nameof(Akyu_GoodEnd), 2, "One in a Thousand", "Akyu looked very young, but to think that she might be as old as Mokou...");

    public static Shard LunarReisen_Smartphone { get; } = new Shard(nameof(LunarReisen_Smartphone), 1, "Can't Live Without it", "Even my Lunatic Red Eyes would get tired at looking at that screen for too long");
    public static Shard LunarReisen_GoodEnd { get; } = new Shard(nameof(LunarReisen_GoodEnd), 2, "Inaba of the Moon", "I see we get worked to the bone whether we're from the eart or the moon");
    public static Shard LunarReisen_Elixir { get; } = new Shard(nameof(LunarReisen_Elixir), 1, "Help me Eiren!", "I restored this moon rabbit but she's quite unreliable all things considered...");

    public static Shard Miyoi_Company { get; } = new Shard(nameof(Miyoi_Company), 2, "Poster Girl of Geidontei", "I guess some people really love their work");
    public static Shard Kogasa_Spook { get; } = new Shard(nameof(Kogasa_Spook), 1, "Quite the Surprise", "I never liked scary movie nights at Eientei");


    private static Dictionary<string, Shard> _shardDictionary;
    public static Dictionary<string, Shard> ShardDictionary
    {
        get
        {
            if (_shardDictionary == null)
            {
                var properties = typeof(Shard).GetProperties(BindingFlags.Public | BindingFlags.Static);
                var shards = properties.Where(pi => pi.PropertyType == typeof(Shard))
                                       .Select(pi => pi.GetValue(null) as Shard);
                _shardDictionary = shards.ToDictionary(s => s.Id, s => s);
            }

            return _shardDictionary;
        }
    }
}

[Serializable]
public class ReisenNpcCharacterProgress : CharacterProgress
{
    public int Stage;
    public bool DialogueRead = false; // Default

    public bool StageDialogueHasBeenRead => DialogueRead;

    public int GetStage()
    {
        return Stage;
    }
}

[Serializable]
public enum Assignment
{
    NotAcquired,
    Inventory,
    Keine,
    Kosuzu,
    Nitori,
    Akyu,
    LunarReisen
}

public enum ReisenPickupItemType
{
    Wrench,
    Newspaper,
    Shard
}

[Serializable]
public class PlayerProgress
{
    public string nameTag;

    public List<Shard> ShardsAcquired;
    public Assignment CoughingMedicine;
    public Assignment TextBook;
    public Assignment Encyclopedia;
    public Assignment Newspaper;
    public Assignment Smartphone;
    public Assignment Magazine;
    public Assignment Wrench;
    public Assignment Scroll;
    public Assignment Schematic;
    public Assignment Novel;
    public Assignment Elixir1;
    public Assignment Elixir2;

    public bool HasAvailableElixir => (Elixir1 == Assignment.Inventory) || (Elixir2 == Assignment.Inventory);

    public void AddShard(Shard s)
    {
        Debug.Log("addded shard");
        ShardsAcquired.Add(s);
    }

    public List<KeyItem> GetKeyItemsInInventory()
    {
        List<KeyItem> inventoryItems = new List<KeyItem>();
        
        if (CoughingMedicine == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Coughing Medicine", "medicine description"));
        if (TextBook == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("History Book", "medicine description"));
        if (Encyclopedia == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Encyclopeida", "medicine description"));
        if (Newspaper == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Newspaper", "medicine description"));
        if (Smartphone == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Smartphone", "medicine description"));
        if (Magazine == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Magazine", "medicine description"));
        if (Wrench == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Wrench", "medicine description"));
        if (Scroll == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Scroll", "medicine description"));
        if (Schematic == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Schematic", "medicine description"));
        if (Novel == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Novel", "medicine description"));
        if (Elixir1 == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Elixir", "medicine description"));
        if (Elixir2 == Assignment.Inventory)
            inventoryItems.Add(new KeyItem("Elixir", "medicine description"));

        return inventoryItems;
    }
}



[Serializable]
public class ReisenGameProgress: GameProgress
{
    public ReisenSavePoint savePoint;

    public PlayerProgress Player;
    public ReisenNpcCharacterProgress Keine;
    public ReisenNpcCharacterProgress Kosuzu;
    public ReisenNpcCharacterProgress Nitori;
    public ReisenNpcCharacterProgress Akyu;
    public ReisenNpcCharacterProgress LunarReisen;
    public ReisenNpcCharacterProgress Orb;

    public ReisenNpcCharacterProgress Miyoi;
    public ReisenNpcCharacterProgress Kogasa;
    public int KogasaCount;

    public const string KeineStage = "keine_stage";
    public const string KosuzuStage = "kosuzu_stage";
    public const string NitoriStage = "nitori_stage";
    public const string AkyuStage = "akyu_stage";
    public const string LunarReisenStage = "lunar_reisen_stage";
    public const string MiyoiStage = "miyoi_stage";
    public const string KogasaStage = "kogasa_stage";

    public const string ShardCount = "shard_count";
    public const string ElixirCount = "elixir";

    public const string HasCough = "cough";
    public const string HasTextbook = "textbook";
    public const string HasEncyclopedia = "encyclopedia";
    public const string HasNewspaper = "newspaper";
    public const string HasSmartphone = "smartphone";
    public const string HasMagazine = "magazine";
    public const string HasWrench = "wrench";
    public const string HasScroll = "scroll";
    public const string HasSchematic = "schematic";
    public const string HasNovel = "novel";
}

// Interfaces to leverage for later
public interface CharacterProgress
{
    int GetStage();
    bool StageDialogueHasBeenRead { get; }
}

public interface GameProgress
{
}
