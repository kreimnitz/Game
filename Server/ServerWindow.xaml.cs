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
            _player0 = new Player(0, GameConstants.StartingGlory, GameConstants.StartingIncome);
            _player1 = new Player(1, GameConstants.StartingGlory, GameConstants.StartingIncome);
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
            _nodeMap.AddNode(new Node(1, new Point(0.375, 0.25)));
            _nodeMap.AddNode(new Node(2, new Point(0.625, 0.25)));
            _nodeMap.AddNode(new Node(3, new Point(0.25, 0.5)));
            _nodeMap.AddNode(new Node(4, new Point(0.5, 0.5)));
            _nodeMap.AddNode(new Node(5, new Point(0.75, 0.5)));
            _nodeMap.AddNode(new Node(6, new Point(0.375, 0.75)));
            _nodeMap.AddNode(new Node(7, new Point(0.625, 0.75)));
            var bottomBase = new Node(8, new Point(0.5, 1));
            bottomBase.State = NodeState.P1Controlled;
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
                _player0.Income = _nodeMap.GetPlayerIncome(0);
                _player1.Income = _nodeMap.GetPlayerIncome(1);

                _player0.Glory += _player0.Income;
                _player1.Glory += _player1.Income;
                _player0.AttackPower = Math.Min(_player0.AttackPower + GameConstants.BaseAttackRegenRate, _player0.AttackPowerMax);
                _player1.AttackPower = Math.Min(_player1.AttackPower + GameConstants.BaseAttackRegenRate, _player1.AttackPowerMax);
            }

            SendStateMessage();
        }

        private void SendStateMessage()
        {
            lock (_sendGameStateLock)
            {
                _messageTransmitter.SendGameStateMessage(new GameState(_player0, _nodeMap));
                _messageTransmitter.SendGameStateMessage(new GameState(_player1, _nodeMap));
            }
        }

        public void HandleAttackRequestMessage(AttackNodeRequest request, int playerId)
        {
            var player = playerId == 0 ? _player0 : _player1;
            var node = _nodeMap.GetNode(request.NodeId);
            var playerState = playerId == 0 ? NodeState.P0Controlled : NodeState.P1Controlled;
            var maxAttack = player.AttackPower == player.AttackPowerMax;
            if (node.State == playerState || player.Glory < GameConstants.AttackGloryCost || !maxAttack)
            {
                return;
            }

            lock (_modifyModelLock)
            {
                player.Glory -= GameConstants.AttackGloryCost;
                if (node.DefenseLevel <= player.AttackPower)
                {
                    node.State = playerId == 0 ? NodeState.P0Controlled : NodeState.P1Controlled;
                    player.AttackPower -= node.DefenseLevel * GameConstants.VictoryAttackLossPercentModifier / 100;
                    node.DefenseLevel = GameConstants.NodeBaseDefense;
                }
                else
                {
                    node.DefenseLevel -= player.AttackPower;
                    player.AttackPower = 0;
                }
            }


            SendStateMessage();
        }

        public void HandleFortifyRequestMessage(FortifyNodeRequest request, int playerId)
        {
            var player = playerId == 0 ? _player0 : _player1;
            var playerState = playerId == 0 ? NodeState.P0Controlled : NodeState.P1Controlled;
            var node = _nodeMap.GetNode(request.NodeId);
            if (node.State != playerState || player.Glory < GameConstants.FortifyGloryCost)
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
