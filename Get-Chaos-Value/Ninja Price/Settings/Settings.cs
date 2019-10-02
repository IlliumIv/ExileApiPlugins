using System;
using System.Configuration;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace Ninja_Price.Settings
{
    public class Settings : ISettings
    {
        public Settings()
        {
            LeagueList = new ListNode();
            ReloadButton = new ButtonNode();
            AutoReload = new ToggleNode();
            AutoReloadTimer = new RangeNode<int>(15, 1, 60);
            UniTextColor = Color.White;
            Debug = new ToggleNode(false);

            // Visible stash tab
            VisibleStashValue = new ToggleNode(true);
            StashValueX = new RangeNode<int>(100, 0, 1920);
            StashValueY = new RangeNode<int>(800, 0, 1080);
            StashValueColorNode = new ColorNode(Color.AliceBlue);
            StashValueFontSize = new RangeNode<int>(20, 0, 200);
            StashValueSignificantDigits = new RangeNode<int>(5, 0, 10);

            // Inventory Value
            HighlightUniqueJunk = new ToggleNode(true);
            HighlightColor = new ColorNode(Color.AliceBlue);
            HighlightFontSize = new RangeNode<int>(20, 0, 200);
            HighlightSignificantDigits = new RangeNode<int>(5, 0, 10);
            InventoryValueCutOff = new RangeNode<int>(1, 0, 10);

            ProphecyBackground = Color.Black;

            ProphecyChaosValue = Color.White;
            ProphecyProecyName = Color.White;
            ProphecyProecySealColor = Color.White;
        }

        public DateTime LastUpDateTime { get; set; } = DateTime.Now;
        public bool FirstTime { get; set; } = false;


        [Menu("Value Loop Timer")]
        public RangeNode<int> ValueLoopTimerMS { get; set; } = new RangeNode<int>(250, 1, 2000);

        [Menu("League", 1)]
        public ListNode LeagueList { get; set; }

        [Menu("Reload", 2)]
        public ButtonNode ReloadButton { get; set; }

        [Menu("Auto Reload Toggle", 3)]
        public ToggleNode AutoReload { get; set; }

        [Menu("Auto Reload (Minutes)", 31, 3)]
        public RangeNode<int> AutoReloadTimer { get; set; }

        [Menu("Plugin Wide Text Color", 355, 3)]
        public ColorNode UniTextColor { get; set; }


        [Menu("Debug", "Display debug strings", 6)]
        public ToggleNode Debug { get; set; }


        [Menu("Prophecy Prices", "This shows your proph prices whilst you have your stash tab open", 567765)]
        public ToggleNode ProphecyPrices { get; set; } = new ToggleNode(true);

        [Menu("Override Background Color from the default theme color", 123345, 567765)]
        public ToggleNode ProphecyOverrideColors { get; set; } = new ToggleNode(true);

        [Menu("Prophecy Background", 234, 567765)]
        public ColorNode ProphecyBackground { get; set; }

        [Menu("Prophecy Locked", "This will lock the proh box so you can click through it without accidentally moving it", 652, 567765)]
        public ToggleNode ProphecyLocked { get; set; } = new ToggleNode(true);

        [Menu("Chaos Value", 653, 567765)]
        public ColorNode ProphecyChaosValue { get; set; }

        [Menu("Prophecy Name", 654, 567765)]
        public ColorNode ProphecyProecyName { get; set; }

        [Menu("Prophecy Seal Cost", 655, 567765)]
        public ColorNode ProphecyProecySealColor { get; set; }

        #region Visible Stash Value

        [Menu("Visible Stash Value", "Calculate value (in chaos) for the current visible stash tab.", 4)]
        public ToggleNode VisibleStashValue { get; set; }

        [Menu("X", "Horizontal position of where the value in chaos should be drawn.", 41, 4)]
        public RangeNode<int> StashValueX { get; set; }

        [Menu("Y", "Horizontal position of where the value in chaos should be drawn.", 42, 4)]
        public RangeNode<int> StashValueY { get; set; }

        [Menu("Size", "Size of the font used to draw the chaos value of the visible stash tab.", 43, 4)]
        public RangeNode<int> StashValueFontSize { get; set; }

        [Menu("Color", 44, 4)]
        public ColorNode StashValueColorNode { get; set; }

        [Menu("Significant Digits", 45, 4)]
        public RangeNode<int> StashValueSignificantDigits { get; set; }

        [Menu("Currency Tab Specifc", 23452, 4)]
        public EmptyNode CurrencyTabSpecifc { get; set; }

        [Menu("Currency Tab Specifc", 75465, 23452)]
        public ToggleNode CurrencyTabSpecifcToggle { get; set; } = new ToggleNode(true);

        [Menu("Value Font Size", 57, 23452)]
        public RangeNode<int> CurrencyTabFontSize { get; set; } = new RangeNode<int>(14, 5, 50);

        [Menu("Significant Digits Per Currency", 58, 23452)]
        public RangeNode<int> CurrenctTabSigDigits { get; set; } = new RangeNode<int>(2, 0, 10);

        [Menu("Box Height", 59, 23452)]
        public RangeNode<int> CurrencyTabBoxHeight { get; set; } = new RangeNode<int>(15, 0, 100);

        [Menu("Font Color", 60, 23452)]
        public ColorNode CurrencyTabFontColor { get; set; } = new Color(216, 216, 216, 255);

        [Menu("Background Color", 61, 23452)]
        public ColorNode CurrencyTabBackgroundColor { get; set; } = new Color(0, 0, 0, 255);

        [Menu("Border Color", 62, 23452)]
        public ColorNode CurrencyTabBorderColor { get; set; } = new Color(146, 107, 43, 255);

        #endregion


        #region Visible Inventory Value

        [Menu("Highlight Unique Junk", "Highlight unique items under X value (useful for quick-selling to vendor).", 5)]
        public ToggleNode HighlightUniqueJunk { get; set; }

        [Menu("Size", "Size of the font used to draw the chaos value of the visible inventory.", 53, 5)]
        public RangeNode<int> HighlightFontSize { get; set; }

        [Menu("Color", 54, 5)]
        public ColorNode HighlightColor { get; set; }

        [Menu("Significant Digits", "The number of wanted decimals", 55, 5)]
        public RangeNode<int> HighlightSignificantDigits { get; set; }

        [Menu("Cut off Value", "Draws a border arround unique items if it's under X value in chaos", 56, 5)]
        public RangeNode<int> InventoryValueCutOff { get; set; }

        #endregion

        public ToggleNode Enable { get; set; } = new ToggleNode(true);
    }
}