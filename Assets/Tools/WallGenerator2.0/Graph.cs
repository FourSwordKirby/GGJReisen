using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorTools {
    /// Stores state data for the path creator editor
    
    public class Graph: MonoBehaviour
    {
        public GameObject nodeGameObject;

        public GraphNode lastActiveNode;

        public List<GraphNode> nodes;
        public List<Edge> edges;

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
            GraphNode vertex1 = splitEdge.vertex1;
            GraphNode vertex2 = splitEdge.vertex2;
            edges.Remove(splitEdge);

            vertex1.RemoveNeighbor(vertex2);
            vertex2.RemoveNeighbor(vertex1);

            GraphNode newNode = (PrefabUtility.InstantiatePrefab(nodeGameObject) as GameObject).GetComponent<GraphNode>();
            Undo.RecordObject(newNode, "Created instance");
            newNode.parentGraph = this;
            newNode.transform.parent = transform;
            newNode.SetPosition(newPathPoint);

            nodes.Add(newNode);
            newNode.AddNeighbor(vertex1);
            newNode.AddNeighbor(vertex2);
            edges.Add(new Edge(vertex1, newNode));
            edges.Add(new Edge(newNode, vertex2));
        }

        public void AddNodeToLastSelected(Vector3 newPathPoint)
        {
            GraphNode newNode = (PrefabUtility.InstantiatePrefab(nodeGameObject) as GameObject).GetComponent<GraphNode>();
            Undo.RecordObject(newNode, "Created instance");
            newNode.parentGraph = this;
            newNode.transform.parent = transform;
            newNode.SetPosition(newPathPoint);

            if (nodes.Count > 0)
            {
                GraphNode tailNode;
                if (lastActiveNode == null)
                    tailNode = nodes[nodes.Count - 1];
                else
                    tailNode = lastActiveNode;
                Undo.RecordObject(tailNode, "Added Edge");
                newNode.AddNeighbor(tailNode);
                edges.Add(new Edge(tailNode, newNode));
            }
            nodes.Add(newNode);
            lastActiveNode = newNode;
        }

        //public void AddNodeToStart(Vector3 newPathPoint)
        //{
        //    GraphNode newNode = (PrefabUtility.InstantiatePrefab(nodeGameObject) as GameObject).GetComponent<GraphNode>();
        //    newNode.parentGraph = this;
        //    newNode.transform.parent = transform;
        //    newNode.SetPosition(newPathPoint);

        //    if (nodes.Count > 0)
        //    {
        //        GraphNode headNode = nodes[0];
        //        headNode.AddNeighbor(newNode);
        //        edges.Add(new Edge(headNode, newNode));
        //    }

        //    nodes.Add(newNode);
        //}

        public void DeleteSegment(int mouseOverHandleIndex)
        {
            Edge removedEdge = edges[mouseOverHandleIndex];
            edges.Remove(removedEdge);
        }

        public void CleanupNode(GraphNode removedNode)
        {
            List<Edge> edgesToRemove = new List<Edge>();
            foreach (Edge edge in edges)
            {
                if (edge.vertex1 == removedNode || edge.vertex2 == removedNode)
                    edgesToRemove.Add(edge);
            }

            edges.RemoveAll(x => edgesToRemove.Contains(x));
            nodes.Remove(removedNode);

            foreach (GraphNode node in nodes)
            {
                if(node.neighboringNodes.Contains(removedNode))
                {
                    Undo.RecordObject(node, "DeletedNeighbor");
                    node.neighboringNodes.Remove(removedNode);
                }
            }
        }

        public Bounds CalculateBoundsWithTransform(Transform transform)
        {
            Bounds ourBounds = new Bounds();
            return ourBounds;
        }
        
        public void Publish()
        {
            foreach(GraphNode node in nodes)
            {
                node.parentGraph = null;
                node.transform.parent = this.transform.parent;
            }

            this.nodes = new List<GraphNode>();
            this.edges = new List<Edge>();
        }
    }

    [Serializable]
    public struct Edge
    {
        public GraphNode vertex1;
        public GraphNode vertex2;

        public Edge(GraphNode vertex1, GraphNode vertex2)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
        }
    }

    public interface IGraphNode
    {
        Vector3 GetPosition();
        void SetPosition(Vector3 position);
        void AddNeighbor(IGraphNode neighbor);
        void RemoveNeighbor(IGraphNode neighbor);
    }

    public abstract class GraphNode : MonoBehaviour, IGraphNode
    {
        public Graph parentGraph;
        public List<GraphNode> neighboringNodes;
        public abstract Vector3 GetPosition();
        public abstract void SetPosition(Vector3 position);
        public abstract void AddNeighbor(IGraphNode neighbor);
        public abstract void RemoveNeighbor(IGraphNode neighbor);
    }

    //public class Node:GraphNode
    //{
    //    Vector3 position;
    //    List<Node> neighbors;

    //    public Node()
    //    {
    //        neighbors = new List<Node>();
    //    }

    //    public Node(Vector3 position)
    //    {
    //        SetPosition(position);
    //        neighbors = new List<Node>();
    //    }

    //    public void AddNeighbor(GraphNode neighbor)
    //    {
    //        neighbors.Add(neighbor as Node);
    //    }

    //    public List<Node> GetNeighbors()
    //    {
    //        return neighbors;
    //    }

    //    public Vector3 GetPosition()
    //    {
    //        return position;
    //    }

    //    public void RemoveNeighbor(GraphNode neighbor)
    //    {
    //        neighbors.Remove(neighbor as Node);
    //    }

    //    public void SetPosition(Vector3 position)
    //    {
    //        this.position = position;
    //    }
    //}
}