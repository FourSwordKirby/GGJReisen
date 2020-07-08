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
    Hidden,
    NotAcquired,
    Inventory,
    Keine,
    Kosuzu,
    Hieda,
    Aya,
}

[Serializable]
public class PlayerProgress
{
    public string nameTag;

    public List<string> ShardsAcquired;
    public Assignment Elixir1;
    public Assignment Elixir2;
    public Assignment CoughingMedicine;
    public Assignment TextBook;
    public Assignment Newspaper;
    public Assignment Manuscript;

    public void AddShard(Shard s)
    {
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
}

// Interfaces to leverage for later
public interface CharacterProgress
{
    int GetStage();
}

public interface GameProgress
{
}
