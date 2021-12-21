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

        private Player _controllingPlayer;
        public Player ControllingPlayer
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

        private double _reserve = 0;
        public double Reserve
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

        private int _flatIncome = 10;
        public int FlatIncome
        {
            get { return _flatIncome; }
            set { NotifyHelpers.SetProperty(this, ref _flatIncome, value); }
        }

        private Upgrades _upgrades = 0;
        public Upgrades Upgrades
        {
            get { return _upgrades;  }
            set { NotifyHelpers.SetProperty(this, ref _upgrades, value); }
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
            FlatIncome = node.FlatIncome;
            Upgrades = node.Upgrades;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ApplyIncome()
        {
            if (ControllingPlayer is null)
            {
                return;
            }

            var income = FlatIncome + ControllingPlayer.IncomeRate * Reserve;
            Reserve = Math.Min(Reserve + income, Capacity);
        }
    }

    public enum NodeType
    {
        Village,
        Monster,
        Vacant
    }

    [Flags]
    public enum Upgrades
    {

    }

    public enum NodeState
    {
        P0Controlled,
        P1Controlled,
        Neutral
    }
}
