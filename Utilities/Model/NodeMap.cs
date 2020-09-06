using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Utilities.Model
{
    [Serializable]
    public class NodeMap
    {
        private Dictionary<int, Node> _nodeDictionary = new Dictionary<int, Node>();

        public NodeMap()
        {
            _nodeDictionary.Add(0, new Node(0, new Point(0.5, 0)));
            _nodeDictionary.Add(1, new Node(1, new Point(0.33, 0.25)));
            _nodeDictionary.Add(2, new Node(2, new Point(0.66, 0.25)));
            _nodeDictionary.Add(3, new Node(3, new Point(0.25, 0.5)));
            _nodeDictionary.Add(4, new Node(4, new Point(0.5, 0.5)));
            _nodeDictionary.Add(5, new Node(5, new Point(0.75, 0.5)));
            _nodeDictionary.Add(6, new Node(6, new Point(0.33, 0.75)));
            _nodeDictionary.Add(7, new Node(7, new Point(0.66, 0.75)));
            _nodeDictionary.Add(8, new Node(8, new Point(0.5, 1)));
        }

        public void CopyFrom(NodeMap otherMap)
        {
            foreach (var otherNode in otherMap.Nodes)
            {
                if (_nodeDictionary.ContainsKey(otherNode.ID))
                {
                    _nodeDictionary[otherNode.ID].CopyFrom(otherNode);
                }
            }
        }

        public IEnumerable<Node> Nodes => _nodeDictionary.Values;
    }
}
