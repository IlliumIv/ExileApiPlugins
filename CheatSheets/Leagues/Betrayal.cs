using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatSheets.Leagues
{
    public class Betrayal : Sheet
    {
        public Betrayal()
        {
            Name = "Betrayal";

            Preloads = new List<string>
            {
                "Metadata/NPC/League/Betrayal/BetrayalNinjaCopRaid",
                "Metadata/Monsters/LeagueBetrayal/FortWall/FortWall",
                "Metadata/Monsters/LeagueBetrayal/BetrayalOriathBlackguardMeleeChampionCartGuard",
                "Metadata/Monsters/LeagueBetrayal/BetrayalCatarina"
            };

            // Sheets.Preloads.Add(Name, Preloads);
        }
    }
}
