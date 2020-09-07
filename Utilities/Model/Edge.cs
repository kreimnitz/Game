using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Model
{
    [Serializable]
    public class Edge
    {
        public Node Node1 { get; set; }
        public Node Node2 { get; set; }

        public override bool Equals(object other)
        {
            if (other is Edge otherEdge)
            {
                return GetHashCode() == otherEdge.GetHashCode();
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Node1.Id, Node2.Id);
        }
    }
}
