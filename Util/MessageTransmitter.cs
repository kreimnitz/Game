using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    public class MessageTrasmitter
    {
        public void SendStateMessage(State state)
        {

        }
    }

    public interface IMessageReceivedHandler
    {
        void HandleStateMessage(State state);
    }

    public class State
    {
        public int Number1 { get; set; }
        public int Number2 { get; set; }
        public int Number3 { get; set; }

        public State(int num1, int num2, int num3)
        {

        }
    }
}
