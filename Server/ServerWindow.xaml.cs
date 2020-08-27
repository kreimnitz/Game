using Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using System.Timers;
using System.Threading;

namespace Server
{
    /// <summary>
    /// Interaction logic for ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window, IAutoNotifyPropertyChanged, IMessageReceivedHandler
    {
        private int _goldIncome = 100;
        private SocketMessageTransmitter _messageTransmitter;
        private System.Timers.Timer _gameLoopTimer;

        private int _goldCount = 0;
        public int GoldCount
        {
            get { return _goldCount; }
        }

        private int _goldMax = 1000;
        public int GoldMax
        {
            get { return _goldMax; }
        }

        private int _goldMaxUpgradeCost = 500;
        public int GoldMaxUpgradeCost
        {
            get { return _goldMaxUpgradeCost; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ServerWindow()
        {
            DataContext = this;
            InitializeComponent();
            _messageTransmitter = new ServerMessageTransmitter(this);
            var gloryWindow = new Glory.MainWindow();
            gloryWindow.Show();
            var ignoredTask = WaitForClientAndStartGameAsync();
        }

        protected override void OnClosed(EventArgs e)
        {
            _gameLoopTimer.Stop();
            _messageTransmitter.CloseConnection();
            base.OnClosed(e);
        }

        private async Task WaitForClientAndStartGameAsync()
        {
            await _messageTransmitter.WaitForReady;
            _gameLoopTimer = new System.Timers.Timer(1000);
            _gameLoopTimer.Elapsed += GameLoop;
            _gameLoopTimer.AutoReset = true;
            _gameLoopTimer.Enabled = true;
            _gameLoopTimer.Start();
        }

        private void GameLoop(object source, ElapsedEventArgs e)
        {
            if (_goldMax >= _goldCount + _goldIncome)
            {
                Interlocked.Add(ref _goldCount, _goldIncome);
                RaisePropertyChanged(nameof(GoldCount));

                _messageTransmitter.SendStateMessage(new State(_goldCount, _goldMax, _goldMaxUpgradeCost));
            }
        }

        public void HandleStateMessage(State state, object sender)
        {
        }

        public void HandleRequestMessage(Request request, object sender)
        {
            if (request == Request.UpgradeGoldMax)
            {
                if (_goldCount >= _goldMaxUpgradeCost)
                {
                    Interlocked.Add(ref _goldCount, -_goldMaxUpgradeCost);
                    Interlocked.Add(ref _goldMaxUpgradeCost, _goldMaxUpgradeCost);
                    Interlocked.Add(ref _goldMax, _goldMax);
                    RaisePropertyChanged(nameof(GoldCount));
                    RaisePropertyChanged(nameof(GoldMax));
                    RaisePropertyChanged(nameof(GoldMaxUpgradeCost));
                }
            }
        }
    }  
}
