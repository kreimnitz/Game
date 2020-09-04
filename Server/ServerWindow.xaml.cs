using Utilities;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;

namespace Server
{
    /// <summary>
    /// Interaction logic for ServerWindow.xaml
    /// </summary>
    public partial class ServerWindow : Window, IAutoNotifyPropertyChanged, IClientMessageRecievedHandler
    {
        private int _deployIncome = 5;
        private int _swordsmanTrainingCost = 50;
        private object _modifyPlayerLock = new object();
        private int _startingGlory = 100;
        private int _startingGloryIncome = 10;
        private ServerMessageTransmitter _messageTransmitter;
        private Timer _gameLoopTimer;
        private Player _player0;
        private Player _player1;

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
            var gloryWindow1 = new Glory.MainWindow();
            gloryWindow1.Show();
            var gloryWindow2 = new Glory.MainWindow();
            gloryWindow2.Show();
            var ignoredTask = WaitForClientAndStartGameAsync();
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
                Fight(_player0, _player1);
                Fight(_player1, _player0);
            }

            _messageTransmitter.SendStatMessage(_player0);
            _messageTransmitter.SendStatMessage(_player1);
        }

        private void Fight(Player attacker, Player defender)
        {
            var oldAttackerCount = attacker.SwordsmanAttackerCount;
            attacker.SwordsmanAttackerCount = Math.Max(oldAttackerCount - defender.SwordsmanDefenderCount, 0);
            defender.SwordsmanDefenderCount = Math.Max(defender.SwordsmanDefenderCount - oldAttackerCount, 0);
            defender.EnemyAttackerCount = attacker.SwordsmanAttackerCount;
            defender.MonumentHealth -= attacker.SwordsmanAttackerCount;
        }

        public void HandleRequestMessage(Request request, int playerId)
        {
            var player = playerId == 0 ? _player0 : _player1;
            lock (_modifyPlayerLock)
            {
                switch (request)
                {
                    case Request.DeployAttackSwordsman:
                        DeployAttackingSwordsman(player);
                        break;
                    case Request.DeployDefenceSwordsman:
                        DeployDefenceSwordsman(player);
                        break;
                    case Request.TrainSwordsman:
                        TrainSwordsman(player);
                        break;
                    default:
                        break;
                }                  
            }
            _messageTransmitter.SendStatMessage(player);
        }

        private void TrainSwordsman(Player player)
        {
            if (player.Glory < _swordsmanTrainingCost)
            {
                return;
            }
            player.Glory -= _swordsmanTrainingCost;
            player.SwordsmanGarrisonCount++;
        }

        private void DeployDefenceSwordsman(Player player)
        {
            if (player.SwordsmanGarrisonCount == 0)
            {
                return;
            }
            player.SwordsmanGarrisonCount--;
            player.SwordsmanDefenderCount++;
        }

        private void DeployAttackingSwordsman(Player player)
        {
            if (player.SwordsmanGarrisonCount == 0)
            {
                return;
            }
            player.SwordsmanGarrisonCount--;
            player.SwordsmanAttackerCount++;
            player.Income += _deployIncome;
        }
    }  
}
