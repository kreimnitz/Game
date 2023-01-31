using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Utilities.Model;

namespace Utilities.ViewModel
{
    public class NodeMapViewModel : IAutoNotifyPropertyChanged
    {
        private Cursor _attackCursor;
        private Cursor _fortifyCursor;
        private object _modelLock = new object();
        private Size _mapSize = new Size();

        public NodeMapViewModel()
        {
            Model = new NodeMap();
            foreach (var node in Model.Nodes)
            {
                NodeViewModels.Add(new NodeViewModel(node));
            }
            var attackCursorStream = new MemoryStream(Properties.Resources.attackCursor);
            _attackCursor = new Cursor(attackCursorStream);
            var fortifyCursorStream = new MemoryStream(Properties.Resources.fortifyCursor);
            _fortifyCursor = new Cursor(fortifyCursorStream);
        }

        private Cursor _cursor;
        public Cursor Cursor
        {
            get { return _cursor; }
            set { NotifyHelpers.SetProperty(this, ref _cursor, value); }
        }

        private MapInputMode _mode = MapInputMode.None;
        public MapInputMode Mode
        {
            get { return _mode; }
            set
            {
                var updated = NotifyHelpers.SetProperty(this, ref _mode, value);
                if (updated)
                {
                    HandleModeSwitch(value);
                }
            }
        }

        private void HandleModeSwitch(MapInputMode newMode)
        {
            switch (newMode)
            {
                case MapInputMode.Attack:
                    Cursor = _attackCursor;
                    break;
                case MapInputMode.Fortify:
                    Cursor = _fortifyCursor;
                    break;
                default:
                    Cursor = Cursors.Arrow;
                    break;
            }
        }

        public NodeMap Model { get; set; }

        public int HoveredId => HoveredNode?.Model.Id ?? -1;

        private NodeViewModel _hoveredNode = null;
        public NodeViewModel HoveredNode
        { 
            get { return _hoveredNode; } 
            set 
            {
                var updated = NotifyHelpers.SetProperty(this, ref _hoveredNode, value);
                if (updated)
                {
                    RaisePropertyChanged(nameof(HoveredId));
                }
            }
        }

        public List<NodeViewModel> NodeViewModels { get; set; } = new List<NodeViewModel>();

        public List<EdgeViewModel> EdgeViewModels { get; set; } = new List<EdgeViewModel>();

        public event EventHandler<MapChangedEventArgs> MapChanged;

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
            (List<NodeViewModel> addedNodes, List<NodeViewModel> removedNodes) = SyncNodes();
            (List<EdgeViewModel> addedEdges, List<EdgeViewModel> removedEdges) = SyncEdges();

            Arrange();
            if (addedNodes.Count == 0 && removedNodes.Count == 0 && addedEdges.Count == 0 && removedEdges.Count == 0)
            {
                return;
            }

            var mapChangedArgs = new MapChangedEventArgs()
            {
                AddedNodes = addedNodes,
                RemovedNodes = removedNodes,
                AddedEdges = addedEdges,
                RemovedEdges = removedEdges
            };
            MapChanged.Invoke(this, mapChangedArgs);
        }

        private (List<NodeViewModel> addedNodes, List<NodeViewModel> removedNodes) SyncNodes()
        {
            var addedNodes = new List<NodeViewModel>();
            var removedNodes = new List<NodeViewModel>();
            foreach (var viewModel in NodeViewModels)
            {
                if (!Model.ContainsNode(viewModel.Model))
                {
                    removedNodes.Add(viewModel);
                }
            }

            foreach (var viewModelToRemove in removedNodes)
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
                    var nodeToAdd = new NodeViewModel(node);
                    nodeToAdd.PlayerId = PlayerId;
                    NodeViewModels.Add(nodeToAdd);
                    addedNodes.Add(nodeToAdd);
                }
            }
            return (addedNodes, removedNodes);
        }

        private (List<EdgeViewModel> addedEdges, List<EdgeViewModel> removedEdges) SyncEdges()
        {
            var removedEdges = new List<EdgeViewModel>();
            var addedEdges = new List<EdgeViewModel>();
            foreach (var viewModel in EdgeViewModels)
            {
                if (!Model.ContainsEdge(viewModel.Model))
                {
                    removedEdges.Add(viewModel);
                }
            }

            foreach (var viewModelToRemove in removedEdges)
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
                    var newEdge = new EdgeViewModel(edge, nodeVm1, nodeVm2);
                    EdgeViewModels.Add(newEdge);
                    addedEdges.Add(newEdge);
                }
            }

            return (addedEdges, removedEdges);
        }

        public int PlayerId { get; set; } = -1;

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

    public enum MapInputMode
    {
        Attack,
        Fortify,
        None
    }
}
