using System;
using System.ComponentModel;
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

        private int _id;
        public int ID
        {
            get { return _id; }
            set { NotifyHelpers.SetProperty(this, ref _id, value); }
        }

        private NodeState _state = NodeState.Neutral;
        public NodeState State
        {
            get { return _state; }
            set { NotifyHelpers.SetProperty(this, ref _state, value); }
        }

        private NodeType _type = NodeType.Village;
        public NodeType Type
        {
            get { return _type; }
            set { NotifyHelpers.SetProperty(this, ref _type, value); }
        }

        private Point _position;
        public Point Position
        {
            get { return _position; }
            set { NotifyHelpers.SetProperty(this, ref _position, value); }
        }

        private int _capacity = 100;
        public int Capacity
        {
            get { return _capacity; }
            set { NotifyHelpers.SetProperty(this, ref _capacity, value); }
        }

        private int _defenseLevel = 100;
        public int DefenseLevel
        {
            get { return _defenseLevel; }
            set { NotifyHelpers.SetProperty(this, ref _defenseLevel, value); }
        }

        private int _population = 20;
        public int Population
        {
            get { return _population; }
            set { NotifyHelpers.SetProperty(this, ref _population, value); }
        }

        public void CopyFrom(Node node)
        {
            ID = node.ID;
            State = node.State;
            Type = node.Type;
            Position = node.Position;
            Capacity = node.Capacity;
            DefenseLevel = node.DefenseLevel;
            Population = node.Population;
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
