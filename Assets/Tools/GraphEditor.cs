using System.Collections.Generic;
using PathCreation;
using PathCreation.Utility;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace EditorTools {
    /// Editor class for the creation of Bezier and Vertex paths

    [CustomEditor (typeof (Graph))]
    public class GraphEditor : Editor {

        //#region Fields

        //// Interaction:
        //const float segmentSelectDistanceThreshold = 10f;
        //const float screenPolylineMaxAngleError = .3f;
        //const float screenPolylineMinVertexDst = .01f;

        //// Help messages:
        //const string helpInfo = "Shift-click to add or insert new points. Control-click to delete points. For more detailed infomation, please refer to the documentation.";
        //static readonly string[] spaceNames = { "3D (xyz)", "2D (xy)", "Top-down (xz)" };
        //static readonly string[] tabNames = { "Bézier Path", "Vertex Path" };
        //const string constantSizeTooltip = "If true, anchor and control points will keep a constant size when zooming in the editor.";

        //// Display
        //const int inspectorSectionSpacing = 10;
        //const float constantHandleScale = .01f;
        //const float normalsSpacing = .2f;
        //GUIStyle boldFoldoutStyle;

        //// References:
        Graph graph;
        PathSpace pathSpace;
        //Editor globalDisplaySettingsEditor;
        //ScreenSpaceGraph screenSpaceLine;
        //ScreenSpaceGraph.MouseInfo pathMouseInfo;
        //GlobalDisplaySettings globalDisplaySettings;
        //PathHandle.HandleColours splineAnchorColours;
        //PathHandle.HandleColours splineControlColours;
        //Dictionary<GlobalDisplaySettings.HandleType, Handles.CapFunction> capFunctions;

        //// State variables:
        int selectedSegmentIndex = -1;
        int draggingHandleIndex = -1;
        int mouseOverHandleIndex = -1;
        int handleIndexToDisplayAsTransform = -1;

        float handleRadius = 0.5f;
        ArcHandle anchorAngleHandle = new ArcHandle();
        bool showPerSegmentBounds;
        bool displayAnchorPoints;
        bool visibleBehindObjects;
        private bool shiftLastFrame;
        private float segmentSelectDistanceThreshold = 10.0f;
        private object pathMouseInfo;

        //bool shiftLastFrame;
        //bool hasUpdatedScreenSpaceLine;
        //bool hasUpdatedNormalsVertexPath;
        //bool editingNormalsOld;

        //Vector3 transformPos;
        //Vector3 transformScale;
        //Quaternion transformRot;

        //Color handlesStartCol;

        //// Constants
        //const int bezierPathTab = 0;
        //const int vertexPathTab = 1;

        //#endregion

        //#region Inspectors

        public override void OnInspectorGUI ()
        {
            showPerSegmentBounds = GUILayout.Toggle (showPerSegmentBounds, new GUIContent ("Show Segment Bounds"));
            displayAnchorPoints = GUILayout.Toggle (displayAnchorPoints, new GUIContent ("Show Anchor Points"));
            visibleBehindObjects = GUILayout.Toggle(visibleBehindObjects, new GUIContent("Visible behind objects"));
            pathSpace = (PathSpace)EditorGUILayout.EnumPopup("Restrict PathSpace:", pathSpace);
            handleRadius = EditorGUILayout.FloatField(handleRadius);

            base.OnInspectorGUI();
            //// Initialize GUI styles
            //if (boldFoldoutStyle == null) {
            //    boldFoldoutStyle = new GUIStyle (EditorStyles.foldout);
            //    boldFoldoutStyle.fontStyle = FontStyle.Bold;
            //}

            //Undo.RecordObject (creator, "Path settings changed");

            //// Draw Bezier and Vertex tabs
            //int tabIndex = GUILayout.Toolbar (data.tabIndex, tabNames);
            //if (tabIndex != data.tabIndex) {
            //    data.tabIndex = tabIndex;
            //    TabChanged ();
            //}

            //// Draw inspector for active tab
            //switch (data.tabIndex) {
            //    case bezierPathTab:
            //        DrawBezierPathInspector ();
            //        break;
            //    case vertexPathTab:
            //        break;
            //}

            //// Notify of undo/redo that might modify the path
            //if (Event.current.type == EventType.ValidateCommand && Event.current.commandName == "UndoRedoPerformed") {
            //    data.PathModifiedByUndo ();
            //}
        }

        void OnSceneGUI()
        {
            if (!visibleBehindObjects)
            {
                Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            }

            EventType eventType = Event.current.type;

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                ProcessGraphInput(Event.current);
                DrawGraphSceneEditor();
            }

            //SetTransformState();
        }
            

        void DrawGraphSceneEditor () {
            using (var check = new EditorGUI.ChangeCheckScope ())
            {
                // Check if out of bounds (can occur after undo operations)
                if (handleIndexToDisplayAsTransform >= graph.nodes.Count)
                {
                    handleIndexToDisplayAsTransform = -1;
                }
                // If a point has been selected
                if (handleIndexToDisplayAsTransform != -1) {
                    EditorGUILayout.LabelField ("Selected Point:");

                    using (new EditorGUI.IndentLevelScope ()) {
                        Debug.Log(handleIndexToDisplayAsTransform);
                        Vector3 currentPosition = graph.nodes[handleIndexToDisplayAsTransform].GetPosition();
                        Vector3 newPosition = EditorGUILayout.Vector3Field ("Position", currentPosition);
                        if (newPosition != currentPosition) {
                            Undo.RecordObject (graph, "Move point");
                            graph.MovePoint (handleIndexToDisplayAsTransform, newPosition);
                        }
                    }
                }
                
                for (int i = 0; i < graph.nodes.Count; i++)
                {
                    if (i == mouseOverHandleIndex || i == draggingHandleIndex)
                    {
                        Handles.color = Color.white;
                    }
                    else
                        Handles.color = Color.black; 

                    Handles.SphereHandleCap(i, graph.nodes[i].transform.position, Quaternion.identity, handleRadius, EventType.Repaint);
                    HandleUtility.Repaint();
                }
    
                if (GUILayout.Button ("Centre Transform")) {

                    Vector3 worldCentre = graph.CalculateBoundsWithTransform (graph.transform).center;
                    Vector3 transformPos = graph.transform.position;
                    if (pathSpace == PathSpace.xy) {
                        transformPos = new Vector3 (transformPos.x, transformPos.y, 0);
                    } else if (pathSpace == PathSpace.xz) {
                        transformPos = new Vector3 (transformPos.x, 0, transformPos.z);
                    }
                    Vector3 worldCentreToTransform = transformPos - worldCentre;

                    if (worldCentre != graph.transform.position) {
                        //Undo.RecordObject (creator, "Centralize Transform");
                        if (worldCentreToTransform != Vector3.zero) {
                            Vector3 localCentreToTransform = MathUtility.InverseTransformVector (worldCentreToTransform, graph.transform, pathSpace);
                            for (int i = 0; i < graph.nodes.Count; i++) {
                                //This is kind of fishy might be better to just save each node's world position and reset them to stay in that position
                                graph.SetNodePosition (i, graph.GetNodePosition(i) + localCentreToTransform, true);
                            }
                        }

                        graph.transform.position = worldCentre;
                        graph.NotifyPathModified ();
                    }
                }
                
                if (check.changed) {
                    SceneView.RepaintAll ();
                    EditorApplication.QueuePlayerLoopUpdate ();
                }
            }
        }

            //void DrawGlobalDisplaySettingsInspector () {
            //    using (var check = new EditorGUI.ChangeCheckScope ()) {
            //        data.globalDisplaySettingsFoldout = EditorGUILayout.InspectorTitlebar (data.globalDisplaySettingsFoldout, globalDisplaySettings);
            //        if (data.globalDisplaySettingsFoldout) {
            //            CreateCachedEditor (globalDisplaySettings, null, ref globalDisplaySettingsEditor);
            //            globalDisplaySettingsEditor.OnInspectorGUI ();
            //        }
            //        if (check.changed) {
            //            UpdateGlobalDisplaySettings ();
            //            SceneView.RepaintAll ();
            //        }
            //    }
            //}

            //#endregion

            //#region Scene GUI

            //void OnSceneGUI () {
            //    if (!globalDisplaySettings.visibleBehindObjects) {
            //        Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
            //    }

            //    EventType eventType = Event.current.type;

            //    using (var check = new EditorGUI.ChangeCheckScope ()) {
            //        handlesStartCol = Handles.color;
            //        switch (data.tabIndex) {
            //            case bezierPathTab:
            //                if (eventType != EventType.Repaint && eventType != EventType.Layout) {
            //                    ProcessBezierPathInput (Event.current);
            //                }

            //                DrawBezierPathSceneEditor ();
            //                break;
            //            case vertexPathTab:
            //                if (eventType == EventType.Repaint) {
            //                    DrawVertexPathSceneEditor ();
            //                }
            //                break;
            //        }

            //        // Don't allow clicking over empty space to deselect the object
            //        if (eventType == EventType.Layout) {
            //            HandleUtility.AddDefaultControl (0);
            //        }

            //        if (check.changed) {
            //            EditorApplication.QueuePlayerLoopUpdate ();
            //        }
            //    }

            //    SetTransformState ();
            //}

            //void DrawVertexPathSceneEditor () {

            //    Color bezierCol = globalDisplaySettings.bezierPath;
            //    bezierCol.a *= .5f;

            //    Handles.color = globalDisplaySettings.vertexPath;

            //    foreach (IGraphNode node in graph.nodes) {
            //        foreach (IGraphNode neighborNode in node.GetNeighbors())
            //        {
            //            Handles.DrawLine(node.GetPosition(), neighborNode.GetPosition());
            //        }
            //    }
            //}

            
        void ProcessGraphInput (Event e) {
            // Find which handle mouse is over. Start by looking at previous handle index first, as most likely to still be closest to mouse
            int previousMouseOverHandleIndex = (mouseOverHandleIndex == -1) ? 0 : mouseOverHandleIndex;
            mouseOverHandleIndex = -1;
            for (int i = 0; i < graph.nodes.Count; i ++) {

                int handleIndex = (previousMouseOverHandleIndex + i) % graph.nodes.Count;
                float dst = HandleUtility.DistanceToCircle (graph.nodes[handleIndex].GetPosition(), handleRadius * 0.5f);
                if (dst == 0) {
                    mouseOverHandleIndex = handleIndex;
                    break;
                }
            }

            // Shift-left click (when mouse not over a handle) to split or add segment to the end
            if (mouseOverHandleIndex == -1) {
                if (e.type == EventType.MouseDown && e.button == 0 && e.shift) {
                    UpdateClosestSegment ();
                    // Insert point along selected segment
                    if (selectedSegmentIndex != -1 && selectedSegmentIndex < graph.edges.Count)
                    {
                        // insert new point at same dst from scene camera as the point that comes before it (for a 3d path)
                        Vector3 newPathPoint;
                        newPathPoint = GetMouseWorldPosition(pathSpace);
                        Undo.RecordObject (graph, "Split segment");
                        graph.SplitSegment (newPathPoint, selectedSegmentIndex);
                    }
                    // If path is not a closed loop, add new point on to the end of the path or as a neighbor to other selected nodes
                    else {
                        // insert new point at same dst from scene camera as the point that comes before it (for a 3d path)
                        Vector3 newPathPoint;
                        if (graph.nodes.Count != 0)
                        {
                            float dstCamToEndpoint = (Camera.current.transform.position - graph.nodes[graph.nodes.Count - 1].GetPosition()).magnitude;
                            newPathPoint = GetMouseWorldPosition(pathSpace, dstCamToEndpoint);
                        }
                        else
                        {
                            newPathPoint = GetMouseWorldPosition(pathSpace);
                        }

                        newPathPoint = GetMouseWorldPosition(pathSpace);

                        Undo.RecordObject (graph, "Add segment");

                        
                        graph.AddNodeToLastSelected (newPathPoint);
                    }

                }
            }

            // Holding shift and moving mouse (but mouse not over a handle/dragging a handle)
            if (draggingHandleIndex == -1 && mouseOverHandleIndex == -1) {
                bool shiftDown = e.shift && !shiftLastFrame;
                if (shiftDown || ((e.type == EventType.MouseMove || e.type == EventType.MouseDrag) && e.shift)) {

                    UpdateClosestSegment ();

                    HandleUtility.Repaint();
                }
            }
            // Holding shift and moving mouse when mouse is over a handle
            else
            {
                if (e.shift)
                {
                    if(e.type == EventType.MouseDown)
                    {
                        draggingHandleIndex = mouseOverHandleIndex;
                        Debug.Log("extruding from" + draggingHandleIndex);
                    }
                    if(e.type == EventType.MouseUp)
                    {
                        Debug.Log("creaing new node from" + draggingHandleIndex);
                        draggingHandleIndex = -1;
                    }
                    HandleUtility.Repaint();
                }
            }

            shiftLastFrame = e.shift;

        }

        void UpdateClosestSegment()
        {
            Vector2 mousePos = Event.current.mousePosition;
            float minDst = float.MaxValue;
            int closestSegmentIndex = -1;

            for (int i = 0; i < graph.edges.Count; i++)
            {
                Edge edge = graph.edges[i];
                Vector2 screenSpaceVertex1 = HandleUtility.WorldToGUIPoint(edge.vertex1.GetPosition());
                Vector2 screenSpaceVertex2 = HandleUtility.WorldToGUIPoint(edge.vertex2.GetPosition());

                float dst = HandleUtility.DistancePointToLineSegment(mousePos, screenSpaceVertex1, screenSpaceVertex2);

                if (dst < minDst)
                {
                    minDst = dst;
                    closestSegmentIndex = i;
                }
            }

            if (minDst < segmentSelectDistanceThreshold)
            {
                if (closestSegmentIndex != selectedSegmentIndex)
                {
                    selectedSegmentIndex = closestSegmentIndex;
                    HandleUtility.Repaint();
                }
            }
            else
            {
                selectedSegmentIndex = -1;
            }
        }
        

        void OnEnable () {
            graph = (Graph) target;

            //data.graphCreated -= ResetState;
            //data.graphCreated += ResetState;
            Undo.undoRedoPerformed -= OnUndoRedo;
            Undo.undoRedoPerformed += OnUndoRedo;

            //ResetState ();
            //SetTransformState (true);
        }
        

        void OnUndoRedo () {
            selectedSegmentIndex = -1;
            Repaint ();
        }

        /// <summary>
        /// Determines mouse position in world. If PathSpace is xy/xz, the position will be locked to that plane.
        /// If PathSpace is xyz, then depthFor3DSpace will be used as distance from scene camera.
        /// </summary>
        public static Vector3 GetMouseWorldPosition(PathSpace space, float depthFor3DSpace = 10)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 worldMouse = mouseRay.GetPoint(depthFor3DSpace);

            // Mouse can only move on XY plane
            if (space == PathSpace.xy)
            {
                float zDir = mouseRay.direction.z;
                if (zDir != 0)
                {
                    float dstToXYPlane = Mathf.Abs(mouseRay.origin.z / zDir);
                    worldMouse = mouseRay.GetPoint(dstToXYPlane);
                }
            }
            // Mouse can only move on XZ plane 
            else if (space == PathSpace.xz)
            {
                float yDir = mouseRay.direction.y;
                if (yDir != 0)
                {
                    float dstToXZPlane = Mathf.Abs(mouseRay.origin.y / yDir);
                    worldMouse = mouseRay.GetPoint(dstToXZPlane);
                }
            }

            return worldMouse;
        }

        //#endregion

    }

    }