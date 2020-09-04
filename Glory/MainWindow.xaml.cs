using System.Windows;
using System.ComponentModel;
using Utilities;
using System;
using Utilities.ViewModel;

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
            _messageTransmitter = new ClientMessageTransmitter(this);
            PlayerStats = new Player(-1, 0, 0);
        }

        public NodeMapViewModel MapViewModel { get; set; } = new NodeMapViewModel();

        protected override void OnClosed(EventArgs e)
        {
            _messageTransmitter.Close();
            base.OnClosed(e);
        }


        public void HandlePlayerMessage(Player playerStats)
        {
            PlayerStats.CopyFrom(playerStats);
        }

        private void TrainSwordsman(object sender, RoutedEventArgs e)
        {
            _messageTransmitter.SendRequest(Request.TrainSwordsman);
        }

        private void DeploySwordsmanDefender(object sender, RoutedEventArgs e)
        {
            _messageTransmitter.SendRequest(Request.DeployDefenceSwordsman);
        }

        private void DeploySwordsmanAttaker(object sender, RoutedEventArgs e)
        {
            _messageTransmitter.SendRequest(Request.DeployAttackSwordsman);
        }
    }
}
