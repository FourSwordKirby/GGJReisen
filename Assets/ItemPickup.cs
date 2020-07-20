using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ReisenPickupItemType item;
    public GameObject ItemSprite;

    public void PickupItem()
    {
        switch (item)
        {
            case ReisenPickupItemType.Wrench:
                ReisenGameManager.instance.gameProgress.Player.Wrench = Assignment.Inventory;
                ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Wrench +1" });
                break;
            case ReisenPickupItemType.Newspaper:
                ReisenGameManager.instance.gameProgress.Player.Newspaper = Assignment.Inventory;
                ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Newspaper +1" });
                break;
            case ReisenPickupItemType.Shard:
                ReisenGameManager.instance.ShowItemTransaction(new List<string>() { "Shard +1" });
                Debug.Log("potentially pick up a shard");
                break;
        }

        ItemSprite.SetActive(false);
    }
}
