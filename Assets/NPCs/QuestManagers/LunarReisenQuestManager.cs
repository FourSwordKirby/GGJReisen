using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LunarReisenQuestManager : Npc
{
    // Start is called before the first frame update
    public void addFirstShard()
    {
        Debug.Log("adding shard");
    }

    // Start is called before the first frame update
    public void addBook()
    {
        ReisenGameManager.instance.gameProgress.Player.TextBook = Assignment.Inventory;
        Debug.Log("adding book");
    }
}
