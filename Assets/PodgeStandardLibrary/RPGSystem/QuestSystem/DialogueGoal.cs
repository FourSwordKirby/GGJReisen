using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
namespace Assets.PodgeStandardLibrary.RPGSystem.QuestSystem
{
    public class DialogueGoal : IGoal
    {
        public Dialogue dialogue;

        public bool GoalCompleted()
        {
            return dialogue.IsFinished;
        }
    }
}