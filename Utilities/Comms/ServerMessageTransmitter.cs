using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Utilities.Comms;
using Utilities.Model;

namespace Utilities
{
    public class ServerMessageTransmitter
    {
        private bool _runReceivers = true;
        private Socket _acceptingSocket;
        private TaskCompletionSource<bool> _waitForReady;
        private Socket[] _communicationSockets { get; set; } = new Socket[2];
        private IClientMessageRecievedHandler _handler;

        public Task WaitForReady => _waitForReady.Task;

        public bool Connected => _waitForReady.Task.IsCompleted;

        public ServerMessageTransmitter(IClientMessageRecievedHandler handler)
        {
            _handler = handler;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            _acceptingSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _waitForReady = new TaskCompletionSource<bool>();

            try
            {
                _acceptingSocket.Bind(localEndPoint);
                _acceptingSocket.Listen(10);
                _acceptingSocket.BeginAccept(new AsyncCallback(AcceptCallback), _acceptingSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Close()
        {
            _runReceivers = false;
            _communicationSockets[0].Close();
            _communicationSockets[1].Close();
            _acceptingSocket.Close();
        }

        public bool SendGameStateMessage(GameState state)
        {
            var socket = _communicationSockets[state.Player.ID];
            if (!socket.Connected)
            {
                return false;
            }
            var message = new Message(MessageType.PlayerState, state.ToByteArray());
            Message.SendMessage(message, socket);
            return true;
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            if (_communicationSockets[0] == null)
            {
                _communicationSockets[0] = handler;
                var ignoredTask = StartReceiverAsync(handler, 0);
                _acceptingSocket.BeginAccept(new AsyncCallback(AcceptCallback), _acceptingSocket);
            }
            else
            {
                _communicationSockets[1] = handler;
                var ignoredTask = StartReceiverAsync(handler, 1);
                _waitForReady.SetResult(true);
            }
        }

        protected async Task StartReceiverAsync(Socket socket, int playerNumber)
        {           
            while (_runReceivers)
            {
                await Task.Delay(1);
                var message = Message.ReceiveMessage(socket);
                if (message.MessageType == MessageType.Request)
                {
                    var request = RequestUtilities.FromByteArray(message.Data);
                    _handler.HandleRequestMessage(request, playerNumber);
                }
            }
        }
    }
}
