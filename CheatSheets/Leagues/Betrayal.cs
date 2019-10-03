using ExileCore.Shared.AtlasHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatSheets.Leagues
{
    public class Betrayal : Sheet
    {
        public static AtlasTexture _iconBetrayal { private get; set; }
        public Betrayal()
        {
            Name = "Betrayal";
            Icon = _iconBetrayal;

            Preloads = new List<string>
            {
                "Metadata/NPC/League/Betrayal/BetrayalNinjaCopRaid",
                "Metadata/Monsters/LeagueBetrayal/FortWall/FortWall",
                "Metadata/Monsters/LeagueBetrayal/BetrayalOriathBlackguardMeleeChampionCartGuard",
                "Metadata/Monsters/LeagueBetrayal/BetrayalCatarina"
            };
        }
    }
}
