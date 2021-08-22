using System.ComponentModel;
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
            FillColor = NodeStateToFillColor(node.State);
            MainLabel = NodeTypeToLabel(node.Type);
            SubLabel1 = string.IsNullOrEmpty(MainLabel) ? string.Empty : node.DefenseLevel.ToString();
            SubLabel2 = string.IsNullOrEmpty(MainLabel) ? string.Empty : node.Population.ToString();
        }

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Node.DefenseLevel))
            {
                SubLabel1 = Model.DefenseLevel.ToString();
                RaisePropertyChanged(nameof(SubLabel1));
            }
            if (e.PropertyName == nameof(Node.Population))
            {
                SubLabel2 = Model.Population.ToString();
                RaisePropertyChanged(nameof(SubLabel2));
            }
            if (e.PropertyName == nameof(Node.State))
            {
                FillColor = NodeStateToFillColor(Model.State);
                RaisePropertyChanged(nameof(FillColor));
            }
        }

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
