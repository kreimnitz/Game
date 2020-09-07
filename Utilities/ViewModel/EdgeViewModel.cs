using System.ComponentModel;
using Utilities.Model;

namespace Utilities.ViewModel
{
    public class EdgeViewModel : IAutoNotifyPropertyChanged
    {
        public EdgeViewModel(Edge edge, NodeViewModel node1, NodeViewModel node2)
        {
            Model = edge;
            Node1 = node1;
            Node2 = node2;
        }

        public Edge Model { get; set; }

        public NodeViewModel Node1 { get; set; }

        public NodeViewModel Node2 { get; set; }

        public int X1 { get; set; }

        public int Y1 { get; set; }

        public int X2 { get; set; }

        public int Y2 { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdatePosition()
        {
            var offset1 = Node1.Size / 2;
            var offset2 = Node2.Size / 2;
            X1 = Node1.Left + offset1;
            X2 = Node2.Left + offset2;
            Y1 = Node1.Top + offset1;
            Y2 = Node2.Top + offset2;
            RaisePropertyChanged(nameof(X1));
            RaisePropertyChanged(nameof(X2));
            RaisePropertyChanged(nameof(Y1));
            RaisePropertyChanged(nameof(Y2));
        }
    }
}
