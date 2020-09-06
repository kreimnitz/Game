using System;
using Utilities.Model;

namespace Utilities.Comms
{
    public interface IServerMessageReceivedHandler
    {
        void HandleGameStateMessage(GameState state);       
    }

    public interface IClientMessageRecievedHandler
    {
        void HandleRequestMessage(Request request, int playerId);
    }

    public enum Request
    {
        AttackNode,
    }

    public static class RequestUtilities
    {
        public static byte[] ToByteArray(Request request)
        {
            return BitConverter.GetBytes((int)request);
        }

        public static Request FromByteArray(byte[] bytes)
        {
            return (Request)BitConverter.ToInt32(bytes, 0);
        }
    }
}
