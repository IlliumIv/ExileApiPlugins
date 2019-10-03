using CheatSheets.Leagues;
using ExileCore.Shared.AtlasHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatSheets
{
    partial class CheatSheetsCore
    {

        public bool InitialiseTexture()
        {
            try
            {
                Incursion._iconIncursion = GetAtlasTexture("IconIncursion");
                Betrayal._iconBetrayal = GetAtlasTexture("IconBetrayal");
                // _iconDelve = GetAtlasTexture("IconDelve");
                Sheet._iconUnknown = GetAtlasTexture("IconUnknown");
            }
            catch(Exception e)
            {
                LogMessage("Cannot finish InitialiseTexture(): \n" + e.Message);
                if (Settings.Debug)
                    LogError(e.StackTrace);
                return false;
            }

            return true;
        }
    }
}
