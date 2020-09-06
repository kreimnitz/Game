using System;
using System.Collections.Generic;

namespace Utilities.Model
{
    [Serializable]
    public class NodeMap
    {
        private Dictionary<int, Node> _nodeDictionary = new Dictionary<int, Node>();

        public NodeMap()
        {
        }

        public bool ContainsNode(Node node)
        {
            return _nodeDictionary.ContainsKey(node.ID);
        }

        public void AddNode(Node node)
        {
            _nodeDictionary.Add(node.ID, node);
        }

        public void CopyFrom(NodeMap otherMap)
        {
            foreach (var otherNode in otherMap.Nodes)
            {
                if (_nodeDictionary.ContainsKey(otherNode.ID))
                {
                    _nodeDictionary[otherNode.ID].CopyFrom(otherNode);
                }
                else
                {
                    _nodeDictionary.Add(otherNode.ID, otherNode);
                }        
            }
        }

        public IEnumerable<Node> Nodes => _nodeDictionary.Values;
    }
}
