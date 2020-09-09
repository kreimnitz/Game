using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Model
{
    [Serializable]
    public class NodeMap
    {
        private Dictionary<int, Node> _nodeDictionary = new Dictionary<int, Node>();
        private List<Edge> _edges = new List<Edge>();

        public NodeMap()
        {
        }

        public bool ContainsNode(Node node)
        {
            return _nodeDictionary.ContainsKey(node.Id);
        }

        public Node GetNode(int id)
        {
            if (_nodeDictionary.ContainsKey(id))
            {
                return _nodeDictionary[id];
            }
            return null;
        }

        public bool ContainsEdge(Edge edge)
        {
            return _edges.Any(e => e.GetHashCode() == edge.GetHashCode());
        }

        public void AddNode(Node node)
        {
            _nodeDictionary.Add(node.Id, node);
        }

        public void AddEdge(int nodeId1, int nodeId2)
        {
            AddEdge(_nodeDictionary[nodeId1], _nodeDictionary[nodeId2]);
        }

        public void AddEdge(Node node1, Node node2)
        {
            var edge = new Edge()
            {
                Node1 = node1,
                Node2 = node2
            };
            _edges.Add(edge);
        }

        public void CopyFrom(NodeMap otherMap)
        {
            foreach (var otherNode in otherMap.Nodes)
            {
                if (_nodeDictionary.ContainsKey(otherNode.Id))
                {
                    _nodeDictionary[otherNode.Id].CopyFrom(otherNode);
                }
                else
                {
                    _nodeDictionary.Add(otherNode.Id, otherNode);
                }        
            }

            if (_edges.Count == 0)
            {
                foreach (var otherEdge in otherMap.Edges)
                {
                    _edges.Add(otherEdge);
                }
            }
        }

        public IEnumerable<Node> Nodes => _nodeDictionary.Values;

        public IEnumerable<Edge> Edges => _edges;

        public int GetPlayerIncome(int playerNumber)
        {
            int income = 0;
            foreach (var node in Nodes)
            {
                var owner = NodeStateUtilities.ToPlayerId(node.State);
                if (owner == playerNumber)
                {
                    income += node.Population;
                }
            }
            return income;
        }
    }
}
