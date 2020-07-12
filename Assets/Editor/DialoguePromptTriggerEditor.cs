using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(DialoguePromptTrigger))]
[CanEditMultipleObjects]
public class DialoguePromptTriggerEditor : Editor
{
    public SerializedProperty DialogueInstructionsProperty;
    public DialoguePromptTrigger DPTrigger;

    void OnEnable()
    {
        DialogueInstructionsProperty = serializedObject.FindProperty("dialogueInstructions");
        DPTrigger = target as DialoguePromptTrigger;
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();
        DrawDefaultInspector();

        if (GUILayout.Button("Find Dialogue Instructions"))
        {
            var parent = DPTrigger.GetComponentInParent<Npc>();
            if (parent != null)
            {
                DPTrigger.dialogueInstructions.Clear();
                string npcName = parent.name;
                var functionNames = parent.GetType().GetMethods().Where(m => m.Name.ToLower().Contains($"{npcName.ToLower()}_"));
                foreach (var func in functionNames)
                {
                    string instructionName = func.Name.Substring(func.Name.IndexOf("Stage"));
                    UnityAction action = Delegate.CreateDelegate(typeof(UnityAction), parent, func) as UnityAction;
                    UnityEvent uEvent = new UnityEvent();
                    UnityEventTools.AddPersistentListener(uEvent, action);
                    DialogueInstruction instruction = new DialogueInstruction()
                    {
                        name = instructionName,
                        action = uEvent
                    };
                    DPTrigger.dialogueInstructions.Add(instruction);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        //// Show the custom GUI controls.
        //EditorGUILayout.IntSlider(damageProp, 0, 100, new GUIContent("Damage"));

        //// Only show the damage progress bar if all the objects have the same damage value:
        //if (!damageProp.hasMultipleDifferentValues)
        //    ProgressBar(damageProp.intValue / 100.0f, "Damage");

        //EditorGUILayout.IntSlider(armorProp, 0, 100, new GUIContent("Armor"));

        //// Only show the armor progress bar if all the objects have the same armor value:
        //if (!armorProp.hasMultipleDifferentValues)
        //    ProgressBar(armorProp.intValue / 100.0f, "Armor");

        //EditorGUILayout.PropertyField(gunProp, new GUIContent("Gun Object"));

        //// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        //serializedObject.ApplyModifiedProperties();
    }

}
