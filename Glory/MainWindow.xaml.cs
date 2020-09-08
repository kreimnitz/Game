using System.Windows;
using System.ComponentModel;
using System;
using Utilities.ViewModel;
using Utilities.Model;
using Utilities.Comms;
using System.Windows.Input;
using System.IO;

namespace Glory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAutoNotifyPropertyChanged, IServerMessageReceivedHandler
    {
        private ClientMessageTransmitter _messageTransmitter;

        public Player PlayerStats { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _nodeMapView.SetDataContext(MapViewModel);
            _messageTransmitter = new ClientMessageTransmitter(this);
            PlayerStats = new Player(-1, 0, 0);
            
        }

        public NodeMapViewModel MapViewModel { get; set; } = new NodeMapViewModel();

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                MapViewModel.Mode = MapMode.Attack;
            }
            e.Handled = false;
            base.OnKeyDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            MapViewModel.Mode = MapMode.None;
        }

        protected override void OnClosed(EventArgs e)
        {
            _messageTransmitter.Close();
            base.OnClosed(e);
        }


        public void HandleGameStateMessage(GameState state)
        {
            PlayerStats.CopyFrom(state.Player);
            MapViewModel.CopyToModel(state.NodeMap);

            Application.Current.Dispatcher.Invoke(delegate
            {
                MapViewModel.SyncToModel();
            });
        }

        
    }
}
