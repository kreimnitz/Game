using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Utilities.Model
{
    public class NodeMap
    {
        public NodeMap()
        {
            Nodes = new List<Node>();
            Nodes.Add(new Node(new Point(0.5, 0)));
            Nodes.Add(new Node(new Point(0.33, 0.25)));
            Nodes.Add(new Node(new Point(0.66, 0.25)));
            Nodes.Add(new Node(new Point(0.25, 0.5)));
            Nodes.Add(new Node(new Point(0.5, 0.5)));
            Nodes.Add(new Node(new Point(0.75, 0.5)));
            Nodes.Add(new Node(new Point(0.33, 0.75)));
            Nodes.Add(new Node(new Point(0.66, 0.75)));
            Nodes.Add(new Node(new Point(0.5, 1)));
        }

        public List<Node> Nodes { get; set; }
    }
}
