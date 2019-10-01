using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatSheets.Leagues
{
    public class Incursion : Sheet
    {
        public Incursion()
        {
            Name = "Incursion";

            Preloads = new List<string>
            {
                "Metadata/NPC/League/Incursion/TreasureHunterWild"
            };

            // Sheets.Preloads.Add(Name, Preloads);
        }
    }
}
