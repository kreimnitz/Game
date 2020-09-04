﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ViewModel
{
    public class NodeMapViewModel
    {
        public NodeMapViewModel()
        {
            NodeViewModels = new ObservableCollection<NodeViewModel>();
            NodeViewModels.Add(new NodeViewModel());
            var secondNode = new NodeViewModel();
            secondNode.Left = 200;
            secondNode.Top = 200;
            NodeViewModels.Add(secondNode);
        }

        public ObservableCollection<NodeViewModel> NodeViewModels { get; set;}
    }
}
