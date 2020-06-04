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

namespace Server
{
    /// <summary>
    /// Interaction logic for ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window, INotifyPropertyChanged, IMessageReceivedHandler
    {
        private SocketMessageTransmitter _messageTransmitter;

        private string _displayText = "None";
        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                if (value != _displayText)
                {
                    _displayText = value;
                    OnPropertyChanged(nameof(DisplayText));
                }
            }
        }

            
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ServerWindow()
        {
            DataContext = this;
            InitializeComponent();
            var gloryWindow = new Glory.MainWindow();
            gloryWindow.Show();
            _messageTransmitter = new ServerMessageTransmitter(this);
        }

        public void HandleStateMessage(State state, object sender)
        {
            DisplayText = state.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _messageTransmitter.SendStateMessage(new State(DisplayText));
        }
    }  
}
