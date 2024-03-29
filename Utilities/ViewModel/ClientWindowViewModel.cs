﻿using Utilities.Model;
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

        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ClientWindowViewModel()
        {
            PlayerStats = new Player(-1, 0, 0, 0);

            _messageTransmitter = new ClientMessageTransmitter(this);
            PlayerStats.PropertyChanged += PlayerStats_PropertyChanged;
        }

        private void PlayerStats_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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

            MapViewModel.Mode = MapInputMode.None;
        }

        public void HandleGameStateMessage(GameState state)
        {
            if (MapViewModel.PlayerId == -1)
            {
                MapViewModel.PlayerId = state.Player.ID;
            }
            PlayerStats.CopyFrom(state.Player);
            MapViewModel.CopyToModel(state.NodeMap);


            Application.Current.Dispatcher.Invoke(delegate
            {
                MapViewModel.SyncToModel();
            });
        }
    }
}
