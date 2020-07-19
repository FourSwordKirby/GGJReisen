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
                break;
            case ReisenPickupItemType.Newspaper:
                ReisenGameManager.instance.gameProgress.Player.Newspaper = Assignment.Inventory;
                break;
            case ReisenPickupItemType.Shard:
                Debug.Log("potentially pick up a shard");
                break;
        }
        ItemSprite.SetActive(false);
    }
}
