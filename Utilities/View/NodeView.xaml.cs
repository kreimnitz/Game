using System.Windows.Controls;
using System.Windows.Data;
using Utilities.ViewModel;

namespace Utilities.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class NodeView : UserControl
    {
        public NodeView(NodeViewModel nodeViewModel)
        {
            InitializeComponent();
            DataContext = nodeViewModel;

            Binding leftBinding = new Binding("Left");
            leftBinding.Source = nodeViewModel;
            leftBinding.Mode = BindingMode.OneWay;
            leftBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(this, Canvas.LeftProperty, leftBinding);

            Binding topBinding = new Binding("Top");
            topBinding.Source = nodeViewModel;
            topBinding.Mode = BindingMode.OneWay;
            topBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(this, Canvas.TopProperty, topBinding);
        }

        public NodeViewModel ViewModel => DataContext as NodeViewModel;
    }
}
