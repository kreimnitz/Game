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
            Id = id;
            Position = position;
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set { NotifyHelpers.SetProperty(this, ref _id, value); }
        }

        private int _controllingPlayer;
        public int ControllingPlayer
        {
            get { return _controllingPlayer; }
            set { NotifyHelpers.SetProperty(this, ref _controllingPlayer, value); }
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

        private int _reserve = 0;
        public int Reserve
        {
            get { return _reserve; }
            set { NotifyHelpers.SetProperty(this, ref _reserve, value); }
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

        private int _income = 10;
        public int Income
        {
            get { return _income; }
            set { NotifyHelpers.SetProperty(this, ref _income, value); }
        }

        public void CopyFrom(Node node)
        {
            Id = node.Id;
            ControllingPlayer = node.ControllingPlayer;
            Type = node.Type;
            Reserve = node.Reserve;
            Position = node.Position;
            Capacity = node.Capacity;
            DefenseLevel = node.DefenseLevel;
            Income = node.Income;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ApplyIncome()
        {
            Reserve = Math.Min(Reserve + Income, Capacity);
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
