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
    public string Description;

    public Shard(string id, int shardValue, string description)
    {
        Id = id;
        ShardValue = shardValue;
        Description = description;
    }

    public static Shard Keine_HistoryIsImportant { get; } = new Shard(nameof(Keine_HistoryIsImportant), 1, "");

    public static Shard Kosuzu_CoughingMedicine { get; } = new Shard(nameof(Kosuzu_CoughingMedicine), 1, "");
    public static Shard Kosuzu_CoughingMedicineElixir { get; } = new Shard(nameof(Kosuzu_CoughingMedicineElixir), 1, "");

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
    public LunarReisenProgress LunarResisen;

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
