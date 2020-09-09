using Utilities.Model;
using Utilities.Comms;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;

namespace Utilities.ViewModel
{
    public class ClientWindowViewModel : IAutoNotifyPropertyChanged, IServerMessageReceivedHandler
    {
        private ClientMessageTransmitter _messageTransmitter;

        public Player PlayerStats { get; private set; }

        public NodeMapViewModel MapViewModel { get; set; } = new NodeMapViewModel();

        public event PropertyChangedEventHandler PropertyChanged;

        public Brush AttackBarBrush => PlayerStats.AttackPower == PlayerStats.AttackPowerMax ? Brushes.Green : Brushes.Red;

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ClientWindowViewModel()
        {
            PlayerStats = new Player(-1, 0, 0);
            
            _messageTransmitter = new ClientMessageTransmitter(this);
            PlayerStats.PropertyChanged += PlayerStats_PropertyChanged;
        }

        private void PlayerStats_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Player.AttackPower))
            {
                RaisePropertyChanged(nameof(AttackBarBrush));
            }
        }

        public void OnClosed()
        {
            _messageTransmitter.Close();
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                MapViewModel.Mode = MapInputMode.Attack;
            }
            if (e.Key == Key.F)
            {
                MapViewModel.Mode = MapInputMode.Fortify;
            }
        }

        public void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                MapViewModel.Mode = MapInputMode.None;
                return;
            }

            if (MapViewModel.Mode == MapInputMode.Attack)
            {
                if (MapViewModel.HoveredId != -1)
                {
                    _messageTransmitter.SendAttackRequest(new AttackNodeRequest(MapViewModel.HoveredId));
                }
            }
            if (MapViewModel.Mode == MapInputMode.Fortify)
            {
                if (MapViewModel.HoveredId != -1)
                {
                    _messageTransmitter.SendFortifyRequest(new FortifyNodeRequest(MapViewModel.HoveredId));
                }
            }

            MapViewModel.Mode = MapInputMode.None;
        }

        public void HandleGameStateMessage(GameState state)
        {
            PlayerStats.CopyFrom(state.Player);
            MapViewModel.CopyToModel(state.NodeMap);

            Application.Current.Dispatcher.Invoke(delegate
            {
                MapViewModel.SyncToModel();
            });
        }
    }
}
