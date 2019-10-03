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
        private AtlasTexture _iconIncursion;
        private AtlasTexture _iconBetrayal;
        private AtlasTexture _iconDelve;
        private AtlasTexture _iconUnknown;

        public bool InitialiseTexture()
        {
            try
            {
                _iconIncursion = GetAtlasTexture("IconIncursion");
                _iconBetrayal = GetAtlasTexture("IconBetrayal");
                _iconDelve = GetAtlasTexture("IconDelve");
                _iconUnknown = GetAtlasTexture("IconUnknown");
            }
            catch(Exception e)
            {
                LogMessage(e.Message);
                return false;
            }

            return true;
        }
    }
}
