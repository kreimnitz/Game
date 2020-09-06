using Utilities;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using Utilities.Comms;
using Utilities.Model;

namespace Server
{
    /// <summary>
    /// Interaction logic for ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window, IAutoNotifyPropertyChanged, IClientMessageRecievedHandler
    {
        private object _modifyPlayerLock = new object();
        private int _startingGlory = 100;
        private int _startingGloryIncome = 10;
        private ServerMessageTransmitter _messageTransmitter;
        private Timer _gameLoopTimer;
        private Player _player0;
        private Player _player1;
        private NodeMap _nodeMap = new NodeMap();

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
            _player0 = new Player(0, _startingGlory, _startingGloryIncome);
            _player1 = new Player(1, _startingGlory, _startingGloryIncome);
            InitializeMap();
            var gloryWindow1 = new Glory.MainWindow();
            gloryWindow1.Show();
            var gloryWindow2 = new Glory.MainWindow();
            gloryWindow2.Show();
            var ignoredTask = WaitForClientAndStartGameAsync();
        }

        private void InitializeMap()
        {
            var topBase = new Node(0, new Point(0.5, 0));
            topBase.State = NodeState.P0Controlled;
            _nodeMap.AddNode(topBase);
            _nodeMap.AddNode(new Node(1, new Point(0.33, 0.25)));
            _nodeMap.AddNode(new Node(2, new Point(0.66, 0.25)));
            _nodeMap.AddNode(new Node(3, new Point(0.25, 0.5)));
            _nodeMap.AddNode(new Node(4, new Point(0.5, 0.5)));
            _nodeMap.AddNode(new Node(5, new Point(0.75, 0.5)));
            _nodeMap.AddNode(new Node(6, new Point(0.33, 0.75)));
            _nodeMap.AddNode(new Node(7, new Point(0.66, 0.75)));
            var bottomBase = new Node(8, new Point(0.5, 1));
            bottomBase.State = NodeState.P1Controlled;
            _nodeMap.AddNode(bottomBase);
        }

        protected override void OnClosed(EventArgs e)
        {
            _gameLoopTimer.Stop();
            _messageTransmitter.Close();
            base.OnClosed(e);
        }

        private async Task WaitForClientAndStartGameAsync()
        {           
            await _messageTransmitter.WaitForReady;
            _gameLoopTimer = new Timer(1000);
            _gameLoopTimer.Elapsed += GameLoop;
            _gameLoopTimer.AutoReset = true;
            _gameLoopTimer.Enabled = true;
            _gameLoopTimer.Start();
        }

        private void GameLoop(object source, ElapsedEventArgs e)
        {
            lock (_modifyPlayerLock)
            {
                _player0.Glory += _player0.Income;
                _player1.Glory += _player1.Income;
            }

            _messageTransmitter.SendGameStateMessage(new GameState(_player0, _nodeMap));
            _messageTransmitter.SendGameStateMessage(new GameState(_player1, _nodeMap));
        }

        public void HandleRequestMessage(Request request, int playerId)
        {
            var player = playerId == 0 ? _player0 : _player1;
            lock (_modifyPlayerLock)
            {
                switch (request)
                {
                    case Request.AttackNode:
                        break;
                    default:
                        break;
                }                  
            }

            _messageTransmitter.SendGameStateMessage(new GameState(player, _nodeMap));
        }
    }  
}
