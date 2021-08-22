using System;

namespace Utilities.Comms
{
    [Serializable]
    public class NodeUpgradeRequest
    {
        public NodeUpgradeRequest(int nodeId)
        {
            NodeId = nodeId;
        }

        public int NodeId { get; set; }
    }

    public enum NodeUpgradeType
    {
        Attack,
        Defense,
        Growth
    }
}
