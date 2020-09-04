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
            ViewModel = new NodeMapViewModel();
            DataContext = ViewModel;
        }

        public NodeMapViewModel ViewModel { get; set; }
    }
}
