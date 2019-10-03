using CheatSheets.Leagues;
using ExileCore.Shared.AtlasHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatSheets
{
    public class Sheet
    {
        public string Name { get; protected set; }
        public List<string> Preloads { get; protected set; }
        public bool AllowIconDrawing { get; set; } = false;
        public static AtlasTexture _iconUnknown { private get; set; }
        public AtlasTexture Icon { get; protected set; }

        public Sheet()
        {
            Icon = _iconUnknown;
        }
    }
}
