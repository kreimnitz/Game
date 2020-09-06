using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
