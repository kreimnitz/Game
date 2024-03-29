﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Utilities.Model;

namespace Utilities.ViewModel
{
    public class NodeViewModel : IAutoNotifyPropertyChanged
    {
        public NodeViewModel()
            : this(new Node())
        {
        }

        public NodeViewModel(Node node)
        {
            Model = node;
            Model.PropertyChanged += Model_PropertyChanged;
            FillColor = NodeStateToFillColor(node);
            MainLabel = NodeTypeToLabel(node.Type);
            SubLabel1 = string.IsNullOrEmpty(MainLabel) ? string.Empty : node.DefenseLevel.ToString();
            SubLabel2 = string.IsNullOrEmpty(MainLabel) ? string.Empty : node.FlatIncome.ToString();
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Node.DefenseLevel))
            {
                SubLabel1 = Model.DefenseLevel.ToString();
                RaisePropertyChanged(nameof(SubLabel1));
            }
            if (e.PropertyName == nameof(Node.FlatIncome))
            {
                SubLabel2 = Model.FlatIncome.ToString();
                RaisePropertyChanged(nameof(SubLabel2));
            }
            if (e.PropertyName == nameof(Node.ControllingPlayer))
            {
                FillColor = NodeStateToFillColor(Model);
                RaisePropertyChanged(nameof(FillColor));
            }
            if (e.PropertyName == nameof(Node.Reserve))
            {
                RaisePropertyChanged(nameof(ClipRect));
            }
        }

        public int PlayerId { get; set; } = -1;

        private bool _hovered = false;
        public bool Hovered
        {
            get { return _hovered; }
            set { NotifyHelpers.SetProperty(this, ref _hovered, value); }
        }

        public Node Model { get; set; }

        public Brush FillColor { get; set; } = Brushes.DarkGray;

        public Brush BorderColor { get; set; } = Brushes.Black;

        public string MainLabel { get; set; } = "";

        public string SubLabel1 { get; set; } = "";

        public string SubLabel2 { get; set; } = "";

        public int Left { get; set; } = 0;

        public int Top { get; set; } = 0;

        public int Size { get; set; } = 80;

        public int InternalCircleSize { get; set; } = 60;

        public int CircleMargin => (Size - InternalCircleSize) / 2;

        public Rect ClipRect => CreateClipRect();

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdatePosition(Size mapSize)
        {
            int width = (int)mapSize.Width - Size;
            int height = (int)mapSize.Height - Size;
            Left = (int)(width * Model.Position.X);
            double yPoint = PlayerId == 1
                ? Model.Position.Y
                : 1 - Model.Position.Y;
            Top = (int)(height * yPoint);
            RaisePropertyChanged(nameof(Left));
            RaisePropertyChanged(nameof(Top));
        }

        private Brush NodeStateToFillColor(Node node)
        {
            switch (node.ControllingPlayer)
            {
                case 0:
                    return Brushes.DarkBlue;
                case 1:
                    return Brushes.DarkRed;
                default:
                    return Brushes.DarkGray;
            }
        }

        private string NodeTypeToLabel(NodeType type)
        {
            switch (type)
            {
                case NodeType.Monster:
                    return "M";
                case NodeType.Village:
                    return "V";
                default:
                    return string.Empty;
            }
        }

        private Rect CreateClipRect()
        {
            var maskRatio = 1 - (Model.Reserve / Model.Capacity);
            var maskY = InternalCircleSize * maskRatio;
            return new Rect(0, maskY, InternalCircleSize, InternalCircleSize - maskY);
        }
    }
}
