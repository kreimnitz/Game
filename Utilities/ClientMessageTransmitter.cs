using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Utilities
{
    public class ClientMessageTransmitter : SocketMessageTransmitter
    {
        private TaskCompletionSource<bool> _waitForReady;
        protected override Socket CommunicationSocket { get; set; }
        public override Task WaitForReady => _waitForReady.Task;

        public ClientMessageTransmitter(IMessageReceivedHandler handler) : base(handler)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
            Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _waitForReady = new TaskCompletionSource<bool>();

            try
            {
                socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), socket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);
            CommunicationSocket = client;
            _waitForReady.SetResult(true);
            var ignoredTask = StartReceiverAsync();
        }
    }
}
