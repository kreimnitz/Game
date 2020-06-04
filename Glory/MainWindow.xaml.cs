using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using Utilities;

namespace Glory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged, IMessageReceivedHandler
    {
        private SocketMessageTransmitter _messageTransmitter;
        private int _gloryCount = 0;
        private int _gloryIncome = 100;
        private string _message = "none";

        public int GloryCount
        {
            get { return _gloryCount; }
            set
            {
                if (value != _gloryCount)
                {
                    _gloryCount = value;
                    OnPropertyChanged(nameof(GloryCount));
                }
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (value != _message)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        private Timer _incomeTimer = new Timer(1000);

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _incomeTimer.AutoReset = true;
            _incomeTimer.Elapsed += IncomeTimer_Elapsed;
            _incomeTimer.Start();

            _messageTransmitter = new ClientMessageTransmitter(this);
        }

        private void IncomeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            GloryCount += _gloryIncome;
        }

        public void HandleStateMessage(State state, object sender)
        {
            Message = state.Text;
        }
    }
}
