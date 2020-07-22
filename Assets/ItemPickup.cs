using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ReisenPickupItemType itemType;
    public GameObject ItemSprite;

    public void PickupItem()
    {
        switch (itemType)
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
                throw new System.Exception("Picking up a shard not implemented");
                Debug.Log("potentially pick up a shard");
                break;
        }

        ItemSprite.SetActive(false);
    }
}
