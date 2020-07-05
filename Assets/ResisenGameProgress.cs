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
public class KeineProgress
{
    public int Stage;
    public int ElixirUsed;
}

[Serializable]
public class KosuzuProgress
{
    public int Stage;
    public int ElixirUsed;
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
public class ReisenGameProgress
{
    public ReisenSavePoint savePoint;

    public PlayerProgress Player;
    public KeineProgress Keine;
    public KosuzuProgress Kosuzu;
}
