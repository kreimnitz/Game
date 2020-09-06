using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Model
{
    [Serializable]
    public class GameState
    {
        public GameState(Player player, NodeMap map)
        {
            Player = player;
            NodeMap = map;
        }

        public Player Player { get; set; }
        public NodeMap NodeMap { get; set; }

        public void CopyFrom(GameState state)
        {
            Player.CopyFrom(state.Player);
            NodeMap.CopyFrom(state.NodeMap);
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

        public static GameState FromByteArray(byte[] bytes)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream(bytes))
            {
                return (GameState)bf.Deserialize(ms);
            }
        }
    }
}
