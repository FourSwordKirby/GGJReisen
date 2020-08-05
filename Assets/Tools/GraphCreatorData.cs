using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation {
    /// Stores state data for the path creator editor
    
    public class Graph
    {
        public List<IGraphNode> nodes;
        public List<Edge> edges;

        public PathSpace Space { get; set; }
        public Action OnModified { get; set; }

        public Graph()
        {
            nodes = new List<IGraphNode>();
            edges = new List<Edge>();
        }

        public void MovePoint(int handleIndexToDisplayAsTransform, Vector3 newPosition)
        {
            nodes[handleIndexToDisplayAsTransform].SetPosition(newPosition);
        }

        public void SetNodePosition(int i, Vector3 pos, bool v)
        {
            nodes[i].SetPosition(pos);
        }

        public Vector3 GetNodePosition(int i)
        {
            return nodes[i].GetPosition();
        }

        public void NotifyPathModified()
        {
            throw new NotImplementedException();
        }

        public void SplitSegment(Vector3 newPathPoint, int selectedSegmentIndex)
        {
            Edge splitEdge = edges[selectedSegmentIndex];
            Node vertex1 = splitEdge.vertex1 as Node;
            Node vertex2 = splitEdge.vertex2 as Node;
            edges.Remove(splitEdge);

            vertex1.RemoveNeighbor(vertex2);
            vertex2.RemoveNeighbor(vertex1);

            Node newNode = new Node(newPathPoint);

            nodes.Add(newNode);
            vertex1.AddNeighbor(newNode);
            newNode.AddNeighbor(vertex2);
            edges.Add(new Edge(vertex1, newNode));
            edges.Add(new Edge(newNode, vertex2));
        }

        public void AddSegmentToEnd(Vector3 newPathPoint)
        {
            IGraphNode newNode = new Node(newPathPoint);

            if(nodes.Count > 0)
            {
                IGraphNode tailNode = nodes[nodes.Count - 1];
                tailNode.AddNeighbor(newNode);
                edges.Add(new Edge(tailNode, newNode));
            }
            nodes.Add(newNode);
        }

        public void AddSegmentToStart(Vector3 newPathPoint)
        {
            IGraphNode newNode = new Node(newPathPoint);

            if (nodes.Count > 0)
            {
                IGraphNode headNode = nodes[0];
                headNode.AddNeighbor(newNode);
                edges.Add(new Edge(headNode, newNode));
            }

            nodes.Add(newNode);

        }

        public void DeleteSegment(int mouseOverHandleIndex)
        {
            Edge removedEdge = edges[mouseOverHandleIndex];
            edges.Remove(removedEdge);
        }

        public Bounds CalculateBoundsWithTransform(Transform transform)
        {
            Bounds ourBounds = new Bounds();
            return ourBounds;
        }
    }

    public struct Edge
    {
        public IGraphNode vertex1;
        public IGraphNode vertex2;

        public Edge(IGraphNode vertex1, IGraphNode vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }
    }

    public interface IGraphNode
    {
        Vector3 GetPosition();
        void SetPosition(Vector3 position);
        List<Node> GetNeighbors();
        void AddNeighbor(IGraphNode neighbor);
        void RemoveNeighbor(IGraphNode neighbor);
    }

    public class Node:IGraphNode
    {
        Vector3 position;
        List<Node> neighbors;

        public Node()
        {
            neighbors = new List<Node>();
        }

        public Node(Vector3 position)
        {
            SetPosition(position);
            neighbors = new List<Node>();
        }

        public void AddNeighbor(IGraphNode neighbor)
        {
            neighbors.Add(neighbor as Node);
        }

        public List<Node> GetNeighbors()
        {
            return neighbors;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void RemoveNeighbor(IGraphNode neighbor)
        {
            neighbors.Remove(neighbor as Node);
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }
    }

    [System.Serializable]
    public class GraphCreatorData {
        public event System.Action graphModified;
        public event System.Action graphCreated;

        [SerializeField]
        bool vertexPathUpToDate;

        // actual graph
        public Graph graph;

        // vertex path settings
        public float vertexPathMaxAngleError = .3f;
        public float vertexPathMinVertexSpacing = 0.01f;

        // bezier display settings
        public bool showTransformTool = true;
        public bool showPathBounds;
        public bool showPerSegmentBounds;
        public bool displayAnchorPoints = true;
        public bool displayControlPoints = true;
        public float bezierHandleScale = 1;
        public bool globalDisplaySettingsFoldout;
        public bool keepConstantHandleSize;

        // vertex display settings
        public bool showNormalsInVertexMode;
        public bool showBezierPathInVertexMode;

        // Editor display states
        public bool showDisplayOptions;
        public bool showPathOptions = true;
        public bool showVertexPathDisplayOptions;
        public bool showVertexPathOptions = true;
        public bool showNormals;
        public bool showNormalsHelpInfo;
        public int tabIndex;

        public void Initialize (bool in2DEditorMode) {
            if(graph == null)
                graph = new Graph();
        }

        public void ResetBezierPath (Vector3 centre, bool defaultIs2D = false) {
            CreateBezier (centre, defaultIs2D);
        }

        void CreateBezier (Vector3 centre, bool defaultIs2D = false) {

            if (graphModified != null) {
                graphModified ();
            }
            if (graphCreated != null) {
                graphCreated ();
            }
        }
        

        public void PathTransformed () {
            if (graphModified != null) {
                graphModified ();
            }
        }

        public void VertexPathSettingsChanged () {
            vertexPathUpToDate = false;
            if (graphModified != null) {
                graphModified ();
            }
        }

        public void PathModifiedByUndo () {
            vertexPathUpToDate = false;
            if (graphModified != null) {
                graphModified ();
            }
        }

        void BezierPathEdited () {
            vertexPathUpToDate = false;
            if (graphModified != null) {
                graphModified ();
            }
        }

    }
}