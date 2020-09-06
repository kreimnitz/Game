using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Utilities.Model
{
    [Serializable]
    public class Node : IAutoNotifyPropertyChanged
    {
        public Node()
        {
        }

        public Node(int id, Point position)
        {
            ID = id;
            Position = position;
        }

        public int ID { get; set; }

        public NodeState State { get; set; }

        public NodeType Type { get; set; } = NodeType.Village;

        public Point Position { get; set; }

        public int Capacity { get; set; } = 100;

        public int DefenseLevel { get; set; } = 100;

        public int Population { get; set; } = 50;

        public void CopyFrom(Node node)
        {
            // implement
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum NodeType
    {
        Village,
        Monster,
        Vacant
    }

    public enum NodeState
    {
        P0Controlled,
        P1Controlled,
        Neutral
    }
}
