using System;

namespace Utilities
{
    public interface IServerMessageReceivedHandler
    {
        void HandlePlayerMessage(Player player);       
    }

    public interface IClientMessageRecievedHandler
    {
        void HandleRequestMessage(Request request, int playerId);
    }

    public enum Request
    {
        TrainSwordsman,
        DeployAttackSwordsman,
        DeployDefenceSwordsman
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
