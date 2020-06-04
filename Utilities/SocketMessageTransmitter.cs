using System.Net.Sockets;
using System.Threading.Tasks;

namespace Utilities
{
    public abstract class SocketMessageTransmitter
    {
        private bool _runReceiver = true;
        protected IMessageReceivedHandler Handler { get; }
        public abstract Task WaitForReady { get; }
        protected abstract Socket CommunicationSocket { get; set; }

        protected SocketMessageTransmitter(IMessageReceivedHandler handler)
        {
            Handler = handler;
        }

        protected async Task StartReceiverAsync()
        {
            while (_runReceiver)
            {
                var message = Message.ReceiveMessage(CommunicationSocket);
                if (message.MessageType == MessageType.State)
                {
                    var state = State.FromByteArray(message.Data);
                    Handler.HandleStateMessage(state, this);
                }
            }

            await Task.Delay(1);
        }

        public void SendStateMessage(State state)
        {
            var message = new Message(MessageType.State, state.ToByteArray());
            Message.SendMessage(message, CommunicationSocket);
        }

        public void StopListening()
        {
            _runReceiver = false;
        }
    }
}
