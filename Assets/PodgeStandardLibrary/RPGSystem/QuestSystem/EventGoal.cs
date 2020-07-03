using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
namespace Assets.PodgeStandardLibrary.RPGSystem
{
    public class EventGoal : IGoal
    {
        public IGameEvent linkedGameEvent;

        public bool GoalCompleted()
        {
            return linkedGameEvent.EventCompleted();
        }
    }
}