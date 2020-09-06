using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using Utilities.Model;

namespace Utilities.ViewModel
{
    public class NodeMapViewModel : IAutoNotifyPropertyChanged
    {
        public NodeMapViewModel()
        {
            Model = new NodeMap();
            foreach (var node in Model.Nodes)
            {
                NodeViewModels.Add(new NodeViewModel(node));
            }
        }

        public NodeMap Model { get; set; }

        public ObservableCollection<NodeViewModel> NodeViewModels { get; set; } = new ObservableCollection<NodeViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SizedChanged(Size newSize)
        {
            ArrangeNodes(newSize);
        }

        private void ArrangeNodes(Size mapSize)
        {
            foreach (var nodeViewModel in NodeViewModels)
            {
                nodeViewModel.UpdatePosition(mapSize);
            }
        }
    }
}
