using System.Windows;
using System.ComponentModel;
using System;
using Utilities.ViewModel;
using Utilities.Model;
using Utilities.Comms;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Controls;

namespace Glory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ClientWindowViewModel ViewModel => DataContext as ClientWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ClientWindowViewModel();
            _nodeMapView.SetDataContext(ViewModel.MapViewModel);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            ViewModel.OnKeyDown(e);
            base.OnKeyDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            ViewModel.OnMouseUp(e);
            base.OnMouseUp(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            ViewModel.OnClosed();
            base.OnClosed(e);
        }        
    }
}
