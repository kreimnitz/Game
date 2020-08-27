using System;

namespace Utilities
{
    public interface IMessageReceivedHandler
    {
        void HandleStateMessage(State state, object sender);

        void HandleRequestMessage(Request request, object sender);
    }

    public enum Request
    {
        UpgradeGoldMax,
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
