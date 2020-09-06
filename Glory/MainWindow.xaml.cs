using System.Windows;
using System.ComponentModel;
using Utilities;
using System;
using Utilities.ViewModel;
using Utilities.Model;
using Utilities.Comms;

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
            _nodeMapView.DataContext = MapViewModel;
            _messageTransmitter = new ClientMessageTransmitter(this);
            PlayerStats = new Player(-1, 0, 0);
        }

        public NodeMapViewModel MapViewModel { get; set; } = new NodeMapViewModel();

        protected override void OnClosed(EventArgs e)
        {
            _messageTransmitter.Close();
            base.OnClosed(e);
        }


        public void HandleGameStateMessage(GameState state)
        {
            PlayerStats.CopyFrom(state.Player);
            MapViewModel.CopyToModel(state.NodeMap);

            App.Current.Dispatcher.Invoke(delegate
            {
                MapViewModel.SyncToModel();
            });
        }
    }
}
