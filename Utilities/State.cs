using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utilities
{
    [Serializable]
    public class State
    {
        public int Gold { get; set; }
        public int GoldMax { get; set; }

        public int GoldMaxUpgradeCost { get; set; }

        public State(int gold, int goldMax, int goldMaxUpgradeCost)
        {
            Gold = gold;
            GoldMax = goldMax;
            GoldMaxUpgradeCost = goldMaxUpgradeCost;
        }

        public byte[] ToByteArray()
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
                return ms.ToArray();
            }
        }

        public static State FromByteArray(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                return (State)bf.Deserialize(ms);
            }
        }
    }
}
