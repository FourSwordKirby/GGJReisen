using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
namespace Assets.PodgeStandardLibrary.RPGSystem
{
    public class TriggerGoal : IGoal
    {
        public ITrigger trigger;

        public bool GoalCompleted()
        {
            return trigger.TriggerCompleted();
        }
    }
}