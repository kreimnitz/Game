using System;
using System.Collections.Generic;

namespace Util
{
    public enum MessageType
    {
        State = 0,
    }

    public class Message
    {
        public static int ChunkSize = 1024;
        public static int MessageTypeSectionSize = 16;
        public static int ChunkCountSectionSize = 16;
        public static int DataSizeSectionSize = 16;
        public static int MessageTypeSectionIndex = 0;
        public static int ChunkCountSectionIndex = 16;
        public static int DataSizeSectionIndex = 32;
        public static int DataSectionIndex = 48;
        public static int HeaderSize = MessageTypeSectionSize + ChunkCountSectionSize + DataSizeSectionSize;

        public MessageType MessageType { get; }
        public byte[] Data { get; }

        public Message()
        {
        }

        /// <summary>
        /// Serializes message into byte arrays of size <see cref="ChunkSize"/> with the first chunk including the header
        /// </summary>
        /// <returns>List of serialized chunks</returns>
        public List<byte[]> SerializeMessage()
        {
            List<byte[]> chunks = new List<byte[]>();
            int totalSize = HeaderSize + Data.Length;
            int totalChunkCount = totalSize / ChunkSize + 1;
            int firstChuckDataSecionSize = ChunkSize - HeaderSize;
            byte[] messageTypeBytes = BitConverter.GetBytes((int)MessageType);
            byte[] chunkCountBytes = BitConverter.GetBytes(totalChunkCount);
            byte[] dataSizeBytes = BitConverter.GetBytes(Data.Length);
            int dataToCopyCount = firstChuckDataSecionSize >= Data.Length
                ? Data.Length
                : firstChuckDataSecionSize;

            byte[] firstChunk = new byte[ChunkSize];
            Array.Copy(messageTypeBytes, 0, firstChunk, MessageTypeSectionIndex, messageTypeBytes.Length);
            Array.Copy(chunkCountBytes, 0, firstChunk, ChunkCountSectionIndex, chunkCountBytes.Length);
            Array.Copy(dataSizeBytes, 0, firstChunk, DataSizeSectionIndex, chunkCountBytes.Length);
            Array.Copy(Data, 0, firstChunk, DataSectionIndex, dataToCopyCount);
            chunks.Add(firstChunk);

            while (chunks.Count < totalSize)
            {
                byte[] nextChunk = new byte[ChunkSize];
                int dataSourceIndex = firstChuckDataSecionSize + ChunkSize * (chunks.Count - 1);
                int copyLength = Math.Max(Data.Length - dataSourceIndex, ChunkSize);
                Array.Copy(Data, dataSourceIndex, nextChunk, 0, copyLength);
                chunks.Add(nextChunk);
            }

            return chunks;
        }
    }
}