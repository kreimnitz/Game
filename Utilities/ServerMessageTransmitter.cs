using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Utilities
{
    public class ServerMessageTransmitter : SocketMessageTransmitter
    {
        private TaskCompletionSource<bool> _waitForReady;
        protected override Socket CommunicationSocket { get; set; }
        public override Task WaitForReady => _waitForReady.Task;

        public ServerMessageTransmitter(IMessageReceivedHandler handler) : base(handler)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            Socket acceptingSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _waitForReady = new TaskCompletionSource<bool>();

            try
            {
                acceptingSocket.Bind(localEndPoint);
                acceptingSocket.Listen(10);
                acceptingSocket.BeginAccept(new AsyncCallback(AcceptCallback), acceptingSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            CommunicationSocket = handler;
            _waitForReady.SetResult(true);
            var ignoredTask = StartReceiverAsync();
        }
    }
}
