using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Utilities.ViewModel;

namespace Utilities.View
{
    /// <summary>
    /// Interaction logic for NodeMap.xaml
    /// </summary>
    public partial class NodeMapView : UserControl
    {
        public NodeMapView()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
        }

        public NodeMapViewModel ViewModel => DataContext as NodeMapViewModel;

        public void SetDataContext(NodeMapViewModel viewModel)
        {
            DataContext = viewModel;
            viewModel.MapChanged += OnMapChanged;
            ViewModel.Arrange();
        }

        private void OnMapChanged(object sender, MapChangedEventArgs e)
        {
            foreach (var newNodeViewModel in e.AddedNodes)
            {
                var newNodeView = new NodeView(newNodeViewModel);
                newNodeView.MouseOverChanged += OnNodeHoveredChanged;
                _canvas.Children.Add(newNodeView);
            }

            foreach (var removedNodeViewModel in e.RemovedNodes)
            {
                NodeView toRemove = GetNodeViewFromViewModel(removedNodeViewModel);
                if (toRemove != null)
                {
                    toRemove.MouseOverChanged -= OnNodeHoveredChanged;
                    _canvas.Children.Remove(toRemove);
                }
            }

            foreach (var newEdgeViewModel in e.AddedEdges)
            {
                var line = CreateLine(newEdgeViewModel);
                _canvas.Children.Add(line);
            }

            foreach (var removedEdgeViewModel in e.RemovedEdges)
            {
                var line = GetLineFromViewModel(removedEdgeViewModel);
                _canvas.Children.Remove(line);
            }

            ViewModel.Arrange();
        }

        private NodeView GetNodeViewFromViewModel(NodeViewModel nodeViewModel)
        {
            NodeView foundNodeView = null;
            foreach (var child in _canvas.Children)
            {
                if (child is NodeView nodeView && nodeView.ViewModel == nodeViewModel)
                {
                    foundNodeView = nodeView;
                }
            }
            return foundNodeView;
        }

        private Line GetLineFromViewModel(EdgeViewModel edgeViewModel)
        {
            Line foundLine = null;
            foreach (var child in _canvas.Children)
            {
                if (child is Line line && line.DataContext == edgeViewModel)
                {
                    foundLine = line;
                }
            }
            return foundLine;
        }

        private void OnNodeHoveredChanged(object sender, bool hovered)
        {
            if (sender is NodeView nodeView)
            {
                if (hovered)
                {
                    ViewModel.HoveredNode = nodeView.ViewModel;
                }
                else if (ViewModel.HoveredNode == nodeView.ViewModel)
                {
                    ViewModel.HoveredNode = null;
                }
            }
        }

        private Line CreateLine(EdgeViewModel edgeViewModel)
        {
            var line = new Line();
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 4;
            line.SetValue(Panel.ZIndexProperty, -1);
            Binding x1Binding = CreateBinding("X1", edgeViewModel);
            BindingOperations.SetBinding(line, Line.X1Property, x1Binding);

            Binding y1Binding = CreateBinding("Y1", edgeViewModel);
            BindingOperations.SetBinding(line, Line.Y1Property, y1Binding);

            Binding x2Binding = CreateBinding("X2", edgeViewModel);
            BindingOperations.SetBinding(line, Line.X2Property, x2Binding);

            Binding y2Binding = CreateBinding("Y2", edgeViewModel);
            BindingOperations.SetBinding(line, Line.Y2Property, y2Binding);

            return line;
        }

        private Binding CreateBinding(string propertyName, object source)
        {
            return new Binding(propertyName)
            {
                Source = source,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size = e.NewSize;
            ViewModel?.SizedChanged(Size);
        }

        public Size Size
        {
            get { return (Size)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(Size), typeof(NodeMapView), new PropertyMetadata(new Size()));
    }
}
