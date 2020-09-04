using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Utilities.ViewModel
{
    public class NodeViewModel
    {
        public Brush FillColor { get; set; } = Brushes.DarkBlue;

        public Brush BorderColor { get; set; } = Brushes.Black;

        public string MainLabel { get; set; } = "A";

        public string SubLabel { get; set; } = "1000";

        public int Left { get; set; } = 0;

        public int Top { get; set; } = 0;
    }
}
