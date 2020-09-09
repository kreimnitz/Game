using System;

namespace Utilities.Comms
{
    [Serializable]
    public class AttackNodeRequest
    {
        public AttackNodeRequest(int nodeId)
        {
            NodeId = nodeId;
        }

        public int NodeId { get; set; }
    }

    [Serializable]
    public class FortifyNodeRequest
    {
        public FortifyNodeRequest(int nodeId)
        {
            NodeId = nodeId;
        }

        public int NodeId { get; set; }
    }
}
