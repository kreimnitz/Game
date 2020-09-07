using System.Collections.Generic;

namespace Utilities.ViewModel
{
    public class MapChangedEventArgs
    {
        public List<NodeViewModel> AddedNodes { get; set; }

        public List<NodeViewModel> RemovedNodes { get; set; }

        public List<EdgeViewModel> AddedEdges { get; set; }

        public List<EdgeViewModel> RemovedEdges { get; set; }
    }
}