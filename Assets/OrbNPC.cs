using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbNPC : Npc
{
    public void Stage_000()
    {
        GameProgress.Orb.Stage = 001;
    }

    public void EndGame()
    {
        Debug.Log("ending game via orb");
    }
}
