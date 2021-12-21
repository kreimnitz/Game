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
        private object _modifyModelLock = new object();
        private object _sendGameStateLock = new object();
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
            _player0 = new Player(0);
            _player1 = new Player(1);
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
            topBase.ControllingPlayer = _player0;
            _nodeMap.AddNode(topBase);
            _nodeMap.AddNode(new Node(1, new Point(0.375, 0.25)));
            _nodeMap.AddNode(new Node(2, new Point(0.625, 0.25)));
            _nodeMap.AddNode(new Node(3, new Point(0.25, 0.5)));
            _nodeMap.AddNode(new Node(4, new Point(0.5, 0.5)));
            _nodeMap.AddNode(new Node(5, new Point(0.75, 0.5)));
            _nodeMap.AddNode(new Node(6, new Point(0.375, 0.75)));
            _nodeMap.AddNode(new Node(7, new Point(0.625, 0.75)));
            var bottomBase = new Node(8, new Point(0.5, 1));
            bottomBase.ControllingPlayer = _player1;
            _nodeMap.AddNode(bottomBase);

            _nodeMap.AddEdge(0, 1);
            _nodeMap.AddEdge(0, 2);
            _nodeMap.AddEdge(1, 3);
            _nodeMap.AddEdge(1, 4);
            _nodeMap.AddEdge(2, 4);
            _nodeMap.AddEdge(2, 5);
            _nodeMap.AddEdge(3, 6);
            _nodeMap.AddEdge(4, 6);
            _nodeMap.AddEdge(4, 7);
            _nodeMap.AddEdge(5, 7);
            _nodeMap.AddEdge(6, 8);
            _nodeMap.AddEdge(7, 8);
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
            _gameLoopTimer = new Timer(GameConstants.IncomeTimeMs);
            _gameLoopTimer.Elapsed += GameLoop;
            _gameLoopTimer.AutoReset = true;
            _gameLoopTimer.Enabled = true;
            _gameLoopTimer.Start();
        }

        private void GameLoop(object source, ElapsedEventArgs e)
        {
            lock (_modifyModelLock)
            {
                ApplyNodeIncome();
                _player0.IncomeRate = _nodeMap.GetPlayerIncome(0);
                _player1.IncomeRate = _nodeMap.GetPlayerIncome(1);

                _player0.ApplyIncome();
                _player1.ApplyIncome();
            }

            SendStateMessage();
        }

        private void ApplyNodeIncome()
        {
            foreach (var node in _nodeMap.Nodes)
            {
                node.ApplyIncome();
            }
        }

        private void SendStateMessage()
        {
            lock (_sendGameStateLock)
            {
                _messageTransmitter.SendGameStateMessage(new GameState(_player0, _nodeMap));
                _messageTransmitter.SendGameStateMessage(new GameState(_player1, _nodeMap));
            }
        }

        public void HandleNodeUpgradeRequestMessage(NodeUpgradeRequest request, int playerId)
        {

        }

        public void HandleAttackRequestMessage(AttackNodeRequest request, int playerId)
        {
            var player = playerId == 0 ? _player0 : _player1;
            var node = _nodeMap.GetNode(request.NodeId);
            if (node.ControllingPlayer == playerId || player.Glory < GameConstants.AttackGloryCost)
            {
                return;
            }

            lock (_modifyModelLock)
            {
                player.Glory -= GameConstants.AttackGloryCost;
            }


            SendStateMessage();
        }

        public void HandleFortifyRequestMessage(FortifyNodeRequest request, int playerId)
        {
            var player = playerId == 0 ? _player0 : _player1;
            var node = _nodeMap.GetNode(request.NodeId);
            if (node.ControllingPlayer != playerId || player.Glory < GameConstants.FortifyGloryCost)
            {
                return;
            }

            lock (_modifyModelLock)
            {
                if (player.Glory >= GameConstants.FortifyGloryCost)
                {
                    player.Glory -= GameConstants.FortifyGloryCost;
                    node.DefenseLevel += GameConstants.FortifyDefenseBoost;
                }
            }

            SendStateMessage();
        }
    }
}
