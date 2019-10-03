using ExileCore.Shared.AtlasHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatSheets.Leagues
{
    public class Incursion : Sheet
    {
        public static AtlasTexture _iconIncursion { private get; set; }
        public Incursion()
        {
            Name = "Incursion";
            Icon = _iconIncursion;

            Preloads = new List<string>
            {
                "Metadata/NPC/League/Incursion/TreasureHunterWild"
            };
        }
    }
}
