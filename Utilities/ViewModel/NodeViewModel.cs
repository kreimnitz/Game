using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Utilities.Model;

namespace Utilities.ViewModel
{
    public class NodeViewModel : IAutoNotifyPropertyChanged
    {
        public NodeViewModel()
        {
            Model = new Node();
        }

        public NodeViewModel(Node node)
        {
            Model = node;
            FillColor = NodeStateToFillColor(node.State);
            MainLabel = NodeTypeToLabel(node.Type);
            SubLabel = string.IsNullOrEmpty(MainLabel) ? string.Empty : node.Population.ToString();
        }

        public Node Model { get; set; }

        public Brush FillColor { get; set; } = Brushes.DarkGray;

        public Brush BorderColor { get; set; } = Brushes.Black;

        public string MainLabel { get; set; } = "";

        public string SubLabel { get; set; } = "";

        public int Left { get; set; } = 0;

        public int Top { get; set; } = 0;

        public int Size { get; set; } = 80;

        public int InternalCircleSize { get; set; } = 60;

        public int CircleMargin => (Size - InternalCircleSize) / 2;

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
            double yPoint = 1 - Model.Position.Y;
            Top = (int)(height * yPoint);
            RaisePropertyChanged(nameof(Left));
            RaisePropertyChanged(nameof(Top));
        }

        private Brush NodeStateToFillColor(NodeState state)
        {
            switch (state)
            {
                case NodeState.P0Controlled:
                    return Brushes.DarkBlue;
                case NodeState.P1Controlled:
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
    }
}
