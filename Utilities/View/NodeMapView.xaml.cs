using System.Collections.Specialized;
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
            viewModel.NodeViewModels.CollectionChanged += NodesChanged;
            viewModel.EdgeViewModels.CollectionChanged += EdgesChanged;
            ViewModel.Arrange();
        }

        private void NodesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                var newNodeViewModel = (NodeViewModel)item;
                _canvas.Children.Add(new NodeView(newNodeViewModel));
            }

            ViewModel.Arrange();
        }

        private void EdgesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                var newEdgeViewModel = (EdgeViewModel)item;
                var line = CreateLine(newEdgeViewModel);
                _canvas.Children.Add(line);
            }

            ViewModel.Arrange();
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
