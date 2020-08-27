using System.Windows;
using System.ComponentModel;
using Utilities;
using System;

namespace Glory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAutoNotifyPropertyChanged, IMessageReceivedHandler
    {
        private ClientMessageTransmitter _messageTransmitter;

        private int _goldCount;
        public int GoldCount
        {
            get { return _goldCount; }
            set { NotifyHelpers.SetProperty(this, ref _goldCount, value); }
        }

        private int _goldMax;
        public int GoldMax
        {
            get { return _goldMax; }
            set { NotifyHelpers.SetProperty(this, ref _goldMax, value); }
        }

        private int _goldMaxUpgradeCost;
        public int GoldMaxUpgradeCost
        {
            get { return _goldMaxUpgradeCost; }
            set { NotifyHelpers.SetProperty(this, ref _goldMaxUpgradeCost, value); }
        }


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
        }

        protected override void OnClosed(EventArgs e)
        {
            _messageTransmitter.CloseConnection();
            base.OnClosed(e);
        }


        public void HandleStateMessage(State state, object sender)
        {
            GoldCount = state.Gold;
            GoldMax = state.GoldMax;
            GoldMaxUpgradeCost = state.GoldMaxUpgradeCost;
        }

        public void HandleRequestMessage(Request request, object sender)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _messageTransmitter.SendRequestMessage(Request.UpgradeGoldMax);
        }
    }
}
