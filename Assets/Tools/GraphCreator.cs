using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation
{
    public class GraphCreator : MonoBehaviour
    {
        GlobalDisplaySettings globalEditorDisplaySettings;

        public event System.Action graphUpdated;

        [SerializeField, HideInInspector]
        public GraphCreatorData EditorData;
        [SerializeField, HideInInspector]
        bool initialized;

        public Graph graph
        {
            get
            {
                return EditorData.graph;
            }
        }

        public void InitializeEditorData(bool in2DEditorMode)
        {
            if (EditorData == null)
            {
                EditorData = new GraphCreatorData();
            }
            EditorData.Initialize(in2DEditorMode);
        }

        // Draw the path when path objected is not selected (if enabled in settings)
        void OnDrawGizmos()
        {
            // Only draw path gizmo if the path object is not selected
            // (editor script is resposible for drawing when selected)

            if (globalEditorDisplaySettings == null)
            {
                globalEditorDisplaySettings = GlobalDisplaySettings.Load();
            }

            /*
            GameObject selectedObj = UnityEditor.Selection.activeGameObject;
            if (selectedObj != gameObject)
            {

                if (path != null)
                {
                    path.UpdateTransform(transform);


                    if (globalEditorDisplaySettings.visibleWhenNotSelected)
                    {

                        Gizmos.color = globalEditorDisplaySettings.bezierPath;

                        for (int i = 0; i < path.NumPoints; i++)
                        {
                            int nextI = i + 1;
                            if (nextI >= path.NumPoints)
                            {
                                if (path.isClosedLoop)
                                {
                                    nextI %= path.NumPoints;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            Gizmos.DrawLine(path.GetPoint(i), path.GetPoint(nextI));
                        }
                    }
                }
                */
        }
    }
}