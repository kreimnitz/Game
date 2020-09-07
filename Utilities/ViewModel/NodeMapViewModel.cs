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

        public ObservableCollection<EdgeViewModel> EdgeViewModels { get; set; } = new ObservableCollection<EdgeViewModel>();

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
            SyncNodes();
            SyncEdges();

            RaisePropertyChanged(string.Empty);
            foreach (var nodeViewModel in NodeViewModels)
            {
                nodeViewModel.UpdatePosition(_mapSize);
                nodeViewModel.RaisePropertyChanged(string.Empty);
            }
        }

        private void SyncNodes()
        {
            var toRemove = new List<NodeViewModel>();
            foreach (var viewModel in NodeViewModels)
            {
                if (!Model.ContainsNode(viewModel.Model))
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
                if (NodeViewModels.Any(vm => vm.Model.Id == node.Id))
                {
                    continue;
                }
                else
                {
                    NodeViewModels.Add(new NodeViewModel(node));
                }
            }
        }

        private void SyncEdges()
        {
            var toRemove = new List<EdgeViewModel>();
            foreach (var viewModel in EdgeViewModels)
            {
                if (!Model.ContainsEdge(viewModel.Model))
                {
                    toRemove.Add(viewModel);
                }
            }

            foreach (var viewModelToRemove in toRemove)
            {
                EdgeViewModels.Remove(viewModelToRemove);
            }

            foreach (var edge in Model.Edges)
            {
                if (EdgeViewModels.Any(vm => vm.Model.GetHashCode() == edge.GetHashCode()))
                {
                    continue;
                }
                else
                {
                    var nodeVm1 = NodeViewModels.First(n => n.Model.Id == edge.Node1.Id);
                    var nodeVm2 = NodeViewModels.First(n => n.Model.Id == edge.Node2.Id);
                    EdgeViewModels.Add(new EdgeViewModel(edge, nodeVm1, nodeVm2));
                }
            }
        }

        public void SizedChanged(Size newSize)
        {
            _mapSize = newSize;
            Arrange();
        }

        public void Arrange()
        {
            foreach (var nodeViewModel in NodeViewModels)
            {
                nodeViewModel.UpdatePosition(_mapSize);
            }

            foreach (var edgeViewModel in EdgeViewModels)
            {
                edgeViewModel.UpdatePosition();
            }
        }
    }
}
