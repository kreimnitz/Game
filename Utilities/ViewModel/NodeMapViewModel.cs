using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Utilities.Model;

namespace Utilities.ViewModel
{
    public class NodeMapViewModel : IAutoNotifyPropertyChanged
    {
        private object _modelLock = new object();
        private Size _mapSize = new Size();

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

        public void CopyToModel(NodeMap nodeMap)
        {
            lock (_modelLock)
            {
                Model.CopyFrom(nodeMap);
            }
        }

        public void SyncToModel()
        {
            var toRemove = new List<NodeViewModel>();
            foreach (var viewModel in NodeViewModels)
            {
                if (!Model.ContainsNode(viewModel.Node))
                {
                    toRemove.Add(viewModel);
                }
            }

            foreach (var viewModelToRemove in toRemove)
            {
                NodeViewModels.Remove(viewModelToRemove);
            }

            foreach (var node in Model.Nodes)
            {
                if (NodeViewModels.Any(vm => vm.Node.ID == node.ID))
                {
                    continue;
                }
                else
                {
                    NodeViewModels.Add(new NodeViewModel(node));
                }
            }

            RaisePropertyChanged(string.Empty);
            foreach (var nodeViewModel in NodeViewModels)
            {
                nodeViewModel.UpdatePosition(_mapSize);
                nodeViewModel.RaisePropertyChanged(string.Empty);
            }
        }

        public void SizedChanged(Size newSize)
        {
            _mapSize = newSize;
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
