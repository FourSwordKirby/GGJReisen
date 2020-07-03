using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
namespace Assets.PodgeStandardLibrary.RPGSystem
{
    public class ItemGoal : IGoal
    {
        public Dictionary<Item, int> RequiredItems;

        public bool GoalCompleted()
        {
            IRpgParty testParty = null;
            Inventory partyInventory = testParty.GetInventory();

            foreach (Item item in RequiredItems.Keys)
            {
                if (partyInventory.GetItemCount(item) != RequiredItems[item])
                    return false;
            }

            return true;
        }
    }
}