using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Utilities.Comms
{
    public enum MessageType
    {
        GameState = 0,
        NodeUpgradeRequest = 1,
    }

    public class Message
    {
        public static int ChunkSize = 1024;
        public static int MessageTypeSectionSize = 4;
        public static int DataSizeSectionSize = 4;
        public static int MessageTypeSectionIndex = 0;
        public static int DataSizeSectionIndex = 4;
        public static int DataSectionIndex = 8;
        public static int HeaderSize = MessageTypeSectionSize + DataSizeSectionSize;

        public MessageType MessageType { get; }
        public byte[] Data { get; }

        public Message(MessageType type, byte[] data)
        {
            MessageType = type;
            Data = data;
        }

        public static void SendMessage(Message message, Socket socket)
        {
            var byteArrays = SerializeMessage(message);
            foreach (var byteArray in byteArrays)
            {
                socket.Send(byteArray);
            }
        }

        public static Message ReceiveMessage(Socket socket)
        {
            var readChunk = new byte[ChunkSize];
            socket.Receive(readChunk);

            var messageType = (MessageType)BitConverter.ToInt32(readChunk, MessageTypeSectionIndex);
            int dataSize = BitConverter.ToInt32(readChunk, DataSizeSectionIndex);

            byte[] data = new byte[dataSize];
            int firstDataCopySize = Math.Min(dataSize, ChunkSize - HeaderSize);
            Array.Copy(readChunk, DataSectionIndex, data, 0, firstDataCopySize);

            var dataToRead = dataSize - firstDataCopySize;
            while (dataToRead > 0)
            {
                socket.Receive(readChunk);
                int dataRead = dataSize - dataToRead;
                int copyLength = Math.Min(dataToRead, ChunkSize);
                Array.Copy(readChunk, 0, data, dataRead, copyLength);
                dataToRead -= ChunkSize;
            }

            return new Message(messageType, data);
        }

        private static List<byte[]> SerializeMessage(Message message)
        {
            List<byte[]> chunks = new List<byte[]>();
            int firstChuckDataSectionSize = ChunkSize - HeaderSize;
            byte[] messageTypeBytes = BitConverter.GetBytes((int)message.MessageType);
            byte[] dataSizeBytes = BitConverter.GetBytes(message.Data.Length);
            int firstDataCopySize = firstChuckDataSectionSize >= message.Data.Length
                ? message.Data.Length
                : firstChuckDataSectionSize;

            byte[] firstChunk = new byte[ChunkSize];
            Array.Copy(messageTypeBytes, 0, firstChunk, MessageTypeSectionIndex, messageTypeBytes.Length);
            Array.Copy(dataSizeBytes, 0, firstChunk, DataSizeSectionIndex, dataSizeBytes.Length);
            Array.Copy(message.Data, 0, firstChunk, DataSectionIndex, firstDataCopySize);
            chunks.Add(firstChunk);

            int dataToWrite = message.Data.Length - firstDataCopySize;
            while (dataToWrite > 0)
            {
                byte[] nextChunk = new byte[ChunkSize];
                int dataSourceIndex = firstChuckDataSectionSize + ChunkSize * (chunks.Count - 1);
                int copyLength = Math.Min(message.Data.Length - dataSourceIndex, ChunkSize);
                Array.Copy(message.Data, dataSourceIndex, nextChunk, 0, copyLength);
                chunks.Add(nextChunk);
                dataToWrite -= copyLength;
            }

            return chunks;
        }
    }
}