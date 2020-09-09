using System;

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
    }
}
