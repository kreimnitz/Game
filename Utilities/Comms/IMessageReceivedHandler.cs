using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Utilities.Model;

namespace Utilities.Comms
{
    public interface IServerMessageReceivedHandler
    {
        void HandleGameStateMessage(GameState state);       
    }

    public interface IClientMessageRecievedHandler
    {
        void HandleNodeUpgradeRequestMessage(NodeUpgradeRequest request, int playerId);
    }

    public static class SerializationUtilities
    {
        public static byte[] ToByteArray<T>(T toSerialize)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, toSerialize);
                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                return (T)bf.Deserialize(ms);
            }
        }
    }
}
