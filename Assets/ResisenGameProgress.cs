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
        FriendlyName = friendlyName;
        ShardValue = shardValue;
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
public class KeineProgress: CharacterProgress
{
    public int Stage;
    public int ElixirUsed;

    public int GetStage()
    {
        return Stage;
    }
}

[Serializable]
public class KosuzuProgress : CharacterProgress
{
    public int Stage;
    public int ElixirUsed;

    public int GetStage()
    {
        return Stage;
    }
}

[Serializable]
public class NitoriProgress : CharacterProgress
{
    public int Stage;
    public int ElixirUsed;

    public int GetStage()
    {
        return Stage;
    }
}

[Serializable]
public class AkyuProgress : CharacterProgress
{
    public int Stage;
    public int ElixirUsed;

    public int GetStage()
    {
        return Stage;
    }
}

[Serializable]
public class LunarReisenProgress : CharacterProgress
{
    public int Stage;
    public int ElixirUsed;

    public int GetStage()
    {
        return Stage;
    }
}


[Serializable]
public class MiyoiProgress : CharacterProgress
{
    public int Stage;

    public int GetStage()
    {
        return Stage;
    }
}

[Serializable]
public class KogasaProgress : CharacterProgress
{
    public int Stage;

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

[Serializable]
public class PlayerProgress
{
    public string nameTag;

    public List<string> ShardsAcquired;
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


    public void AddShard(Shard s)
    {
        Debug.Log("addded shard");
        ShardsAcquired.Add(s.Id);
    }
}

[Serializable]
public class ReisenGameProgress: GameProgress
{
    public ReisenSavePoint savePoint;

    public PlayerProgress Player;
    public KeineProgress Keine;
    public KosuzuProgress Kosuzu;
    public NitoriProgress Nitori;
    public AkyuProgress Akyu;
    public LunarReisenProgress LunarReisen;

    public MiyoiProgress Miyoi;
    public KogasaProgress Kogasa;


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
    public const string HasNewspaper = "newspapaer";
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
}

public interface GameProgress
{
}
