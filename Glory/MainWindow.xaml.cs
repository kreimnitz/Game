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
        private RadialMeterViewModel _gloryMeterViewModel = new RadialMeterViewModel();

        public ClientWindowViewModel ViewModel => DataContext as ClientWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ClientWindowViewModel();
            _nodeMapView.SetDataContext(ViewModel.MapViewModel);
            _gloryMeterViewModel = _gloryMeter.ViewModel;

            ViewModel.PlayerStats.PropertyChanged += PlayerStats_PropertyChanged;
            _gloryMeterViewModel.Value = ViewModel.PlayerStats.Glory;
            _gloryMeterViewModel.Maximum = ViewModel.PlayerStats.GloryMax;
        }

        private void PlayerStats_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var player = (Player)sender;
            if (e.PropertyName == nameof(Player.Glory))
            {
                _gloryMeterViewModel.Value = player.Glory;
            }
            else if (e.PropertyName == nameof(Player.GloryMax))
            {
                _gloryMeterViewModel.Maximum = player.GloryMax;
            }
            else if (e.PropertyName == nameof(Player.Income))
            {
                _gloryMeterViewModel.ValueSubtext = $"+{player.Income}";
            }
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
