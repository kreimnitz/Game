using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Utilities
{
    public class ClientMessageTransmitter
    {
        private bool _runReceiver = true;
        private Socket _initialSocket;
        private Socket _commsSocket;
        private TaskCompletionSource<bool> _waitForReady;
        IServerMessageReceivedHandler _handler;

        public Task WaitForReady => _waitForReady.Task;

        public ClientMessageTransmitter(IServerMessageReceivedHandler handler)
        {
            _handler = handler;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            _initialSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _waitForReady = new TaskCompletionSource<bool>();

            try
            {
                _initialSocket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), _initialSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Close()
        {
            _runReceiver = false;
            _commsSocket.Close();
            _initialSocket.Close();
        }

        public void SendRequest(Request request)
        {
            var message = new Message(MessageType.Request, RequestUtilities.ToByteArray(request));
            Message.SendMessage(message, _commsSocket);
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            _commsSocket = client;
            _waitForReady.SetResult(true);
            var ignoredTask = StartReceiverAsync(_commsSocket);
        }

        protected async Task StartReceiverAsync(Socket socket)
        {
            while (_runReceiver)
            {
                await Task.Delay(1);
                var message = Message.ReceiveMessage(socket);
                if (message.MessageType == MessageType.PlayerState)
                {
                    var player = Player.FromByteArray(message.Data);
                    _handler.HandlePlayerMessage(player);
                }
            }
        }
    }
}
