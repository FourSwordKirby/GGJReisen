using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
namespace Assets.PodgeStandardLibrary.RPGSystem
{
    public class EncounterGoal : IGoal
    {
        public IEncounter linkedEncounter;

        public bool GoalCompleted()
        {
            return linkedEncounter.EncounterCompleted();
        }
    }
}