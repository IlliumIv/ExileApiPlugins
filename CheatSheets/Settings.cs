using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatSheets
{
    public class Settings : ISettings
    {
        public Settings()
        {
            ShowInHideout = new ToggleNode(true);
        }
        public ToggleNode ShowInHideout { get; set; }
        public ToggleNode Enable { get; set; } = new ToggleNode(false);
        public ToggleNode ParallelParsing { get; set; } = new ToggleNode(true);
        public ToggleNode Debug { get; set; } = new ToggleNode(false);
        public RangeNode<float> X { get; set; } = new RangeNode<float>(1, 1, 500);
        public RangeNode<float> Y { get; set; } = new RangeNode<float>(1, 1, 500);
    }
}
