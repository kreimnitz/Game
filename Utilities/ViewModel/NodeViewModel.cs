using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utilities.Model;

namespace Utilities.ViewModel
{
    public class NodeViewModel
    {
        public NodeViewModel()
        {
        }

        public NodeViewModel(Node node)
        {
            FillColor = NodeStateToFillColor(node.State);
            MainLabel = NodeTypeToLabel(node.Type);
            SubLabel = string.IsNullOrEmpty(MainLabel) ? string.Empty : node.Population.ToString();
        }

        public Brush FillColor { get; set; } = Brushes.DarkBlue;

        public Brush BorderColor { get; set; } = Brushes.Black;

        public string MainLabel { get; set; } = "A";

        public string SubLabel { get; set; } = "1000";

        public int Left { get; set; } = 0;

        public int Top { get; set; } = 0;

        private Brush NodeStateToFillColor(NodeState state)
        {
            switch (state)
            {
                case NodeState.P0Controlled:
                    return Brushes.DarkBlue;
                case NodeState.P1Controlled:
                    return Brushes.DarkRed;
                default:
                    return Brushes.DarkGray;
            }              
        }

        private string NodeTypeToLabel(NodeType type)
        {
            switch (type)
            {
                case NodeType.Monster:
                    return "M";
                case NodeType.Village:
                    return "V";
                default:
                    return string.Empty;
            }
        }
    }
}
