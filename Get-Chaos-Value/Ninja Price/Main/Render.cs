using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.Shared.Enums;
using Ninja_Price.Enums;
using SharpDX;

namespace Ninja_Price.Main
{
    public partial class Main
    {
        // TODO: Clean this shit up later
        private static Vector2 lastProphWindowPos = new Vector2(0, 0);

        private static Vector2 lastProphWindowSize = new Vector2(0, 0);
        public Stopwatch ValueUpdateTimer = Stopwatch.StartNew();
        public double StashTabValue { get; set; }
        public List<NormalInventoryItem> ItemList { get; set; } = new List<NormalInventoryItem>();
        public List<CustomItem> FortmattedItemList { get; set; } = new List<CustomItem>();

        public List<NormalInventoryItem> InventoryItemList { get; set; } = new List<NormalInventoryItem>();
        public List<CustomItem> FortmattedInventoryItemList { get; set; } = new List<CustomItem>();

        public List<CustomItem> ItemsToDrawList { get; set; } = new List<CustomItem>();
        public List<CustomItem> InventoryItemsToDrawList { get; set; } = new List<CustomItem>();
        public StashElement StashPanel { get; set; }

        public CustomItem Hovereditem { get; set; }

        // TODO: Get hovered items && items from inventory - Getting hovered itemw ill become useful later on

        public override void Render()
        {
            #region Reset All Data

            CurrentLeague = Settings.LeagueList.Value; //  Update selected league every tick
            StashTabValue = 0;
            Hovereditem = null;

            StashPanel = GameController.Game.IngameState.IngameUi.StashElement;

            #endregion


            try // Im lazy and just want to surpress all errors produced
            {
                // only update if the time between last update is more than AutoReloadTimer interval
                if (Settings.AutoReload &&
                    Settings.LastUpDateTime.AddMinutes(Settings.AutoReloadTimer.Value) < DateTime.Now)
                {
                    LoadJsonData();
                    Settings.LastUpDateTime = DateTime.Now;
                }

                if (Settings.Debug) LogMessage($"{GetCurrentMethod()}.Loop() is Alive", 5, Color.LawnGreen);

                if (Settings.Debug)
                    LogMessage($"{GetCurrentMethod()}: Selected League: {Settings.LeagueList.Value}", 5, Color.White);


                // Everything is updated, lets check if we should draw
                if (StashPanel.IsVisible)
                {
                    if (ShouldUpdateValues())
                    {
                        // Format stash items
                        ItemList = new List<NormalInventoryItem>();
                        ItemList = StashPanel.VisibleStash.VisibleInventoryItems.ToList();
                        FortmattedItemList = new List<CustomItem>();
                        FortmattedItemList = FormatItems(ItemList);

                        // Format Inventory Items
                        InventoryItemList = new List<NormalInventoryItem>();
                        InventoryItemList = GetInventoryItems();
                        FortmattedInventoryItemList = new List<CustomItem>();
                        FortmattedInventoryItemList = FormatItems(InventoryItemList);

                        if (Settings.Debug)
                            LogMessage($"{GetCurrentMethod()}.Render() Looping if (ShouldUpdateValues())", 5,
                                Color.LawnGreen);

                        foreach (var item in FortmattedItemList)
                            GetValue(item);
                        foreach (var item in FortmattedInventoryItemList)
                            GetValue(item);
                    }

                    // Gather all information needed before rendering as we only want to itterate through the list once

                    ItemsToDrawList = new List<CustomItem>();
                    foreach (var item in FortmattedItemList)
                    {
                        if (item == null || item.Item.Address == 0) continue; // Item is fucked, skip
                        if (!item.Item.IsVisible && item.ItemType != ItemTypes.None)
                            continue; // Disregard non visable items as that usually means they arnt part of what we want to look at

                        StashTabValue += item.PriceData.ChaosValue;
                        ItemsToDrawList.Add(item);
                    }

                    InventoryItemsToDrawList = new List<CustomItem>();
                    foreach (var item in FortmattedInventoryItemList)
                    {
                        if (item == null || item.Item.Address == 0) continue; // Item is fucked, skip
                        if (!item.Item.IsVisible && item.ItemType != ItemTypes.None)
                            continue; // Disregard non visable items as that usually means they arnt part of what we want to look at

                        InventoryItemsToDrawList.Add(item);
                    }
                }

                // TODO: Graphical part from gathered data

                GetHoveredItem(); // Get information for the hovered item
                DrawGraphics();
            }
            catch (Exception e)
            {
                // ignored
                if (Settings.Debug)
                {
                    LogMessage("Error in: Main Render Loop, restart PoEHUD.", 5, Color.Red);
                    LogMessage(e.ToString(), 5, Color.Orange);
                }
            }

            try
            {
                //PropheccyDisplay();
            }
            catch
            {
                if (Settings.Debug) LogMessage("Error in: PropheccyDisplay(), restart PoEHUD.", 5, Color.Red);
            }
        }

        public void DrawGraphics()
        {
            // Hovered Item
            if (Hovereditem != null && Hovereditem.ItemType != ItemTypes.None)
            {
                var text = $"Change in last 7 Days: {Hovereditem.PriceData.ChangeInLast7Days}%";
                switch (Hovereditem.ItemType)
                {
                    case ItemTypes.Currency:
                    case ItemTypes.Essence:
                    case ItemTypes.Fragment:
                    case ItemTypes.Scarab:
                    case ItemTypes.Resonator:
                    case ItemTypes.Fossil:
                    case ItemTypes.Oil:
                        text += $"\n\rChaos: {Hovereditem.PriceData.ChaosValue / Hovereditem.CurrencyInfo.StackSize}" +
                                $"\n\rTotal: {Hovereditem.PriceData.ChaosValue}";
                        break;
                    case ItemTypes.Prophecy:
                    case ItemTypes.UniqueAccessory:
                    case ItemTypes.UniqueArmour:
                    case ItemTypes.UniqueFlask:
                    case ItemTypes.UniqueJewel:
                    case ItemTypes.UniqueMap:
                    case ItemTypes.UniqueWeapon:
                    case ItemTypes.NormalMap:
                    case ItemTypes.DivinationCard:
                    case ItemTypes.Incubator:
                        text += $"\n\rChaos: {Hovereditem.PriceData.ChaosValue}";
                        break;
                }

                if (Settings.Debug) text += $"\n\rItemType: {Hovereditem.ItemType}";
                var textMeasure = Graphics.MeasureText(text, 13);
                Graphics.DrawBox(new RectangleF(10, 10, textMeasure.X, textMeasure.Y), Color.Black);
                Graphics.DrawText(text, new Vector2(10, 10), Color.White);
            }

            if (!StashPanel.IsVisible)
                return;

            // Stash Tab Value
            VisibleStashValue();

            var tabType = StashPanel.VisibleStash.InvType;
            foreach (var customItem in ItemsToDrawList)
            {
                if (customItem.ItemType == ItemTypes.None) continue;

                if (Settings.CurrencyTabSpecifcToggle)
                    switch (tabType)
                    {
                        case InventoryType.CurrencyStash:
                        case InventoryType.FragmentStash:
                        case InventoryType.DelveStash:
                            PriceBoxOverItem(customItem);
                            break;
                    }

                if (Settings.HighlightUniqueJunk)
                {
                    if (customItem.ItemType == ItemTypes.None || customItem.ItemType == ItemTypes.Currency ||
                        customItem.ItemType == ItemTypes.Oil || customItem.ItemType == ItemTypes.Scarab) continue;
                    HighlightJunkUniques(customItem);
                }
            }

            if (Settings.HighlightUniqueJunk)
                foreach (var customItem in InventoryItemsToDrawList)
                {
                    if (customItem.ItemType == ItemTypes.None || customItem.ItemType == ItemTypes.Currency ||
                        customItem.ItemType == ItemTypes.Oil || customItem.ItemType == ItemTypes.Scarab) continue;

                    HighlightJunkUniques(customItem);
                }
        }

        private void VisibleStashValue()
        {
            try
            {
                var StashType = GameController.Game.IngameState.IngameUi.StashElement.VisibleStash.InvType;
                if (!Settings.VisibleStashValue.Value ||
                    !StashPanel.IsVisible && StashType != InventoryType.MapStash) return;
                {
                    var pos = new Vector2(Settings.StashValueX.Value, Settings.StashValueY.Value);
                    var significantDigits =
                        Math.Round((decimal) StashTabValue, Settings.StashValueSignificantDigits.Value);
                    //Graphics.DrawText(
                    //    DrawImage($"{DirectoryFullName}//images//Chaos_Orb_inventory_icon.png",
                    //        new RectangleF(Settings.StashValueX.Value - Settings.StashValueFontSize.Value,
                    //            Settings.StashValueY.Value,
                    //            Settings.StashValueFontSize.Value, Settings.StashValueFontSize.Value))
                    //        ? $"{significantDigits}"
                    //        : $"{significantDigits} Chaos", Settings.StashValueFontSize.Value, pos,
                    //    Settings.UniTextColor);
                    Graphics.DrawText($"{significantDigits} Chaos", pos, Settings.UniTextColor, FontAlign.Center);
                }
            }
            catch (Exception e)
            {
                // ignored
                if (Settings.Debug)
                {
                    LogMessage("Error in: VisibleStashValue, restart PoEHUD.", 5, Color.Red);
                    LogMessage(e.ToString(), 5, Color.Orange);
                }
            }
        }

        private void PriceBoxOverItem(CustomItem item)
        {
            var box = item.Item.GetClientRect();
            var drawBox = new RectangleF(box.X, box.Y - 2, box.Width, -Settings.CurrencyTabBoxHeight);
            var position = new Vector2(drawBox.Center.X, drawBox.Center.Y - Settings.CurrencyTabFontSize.Value / 2);

            Graphics.DrawText(
                Math.Round((decimal) item.PriceData.ChaosValue, Settings.CurrenctTabSigDigits.Value).ToString(),
                position, Settings.CurrencyTabFontColor, FontAlign.Center);
            Graphics.DrawBox(drawBox, Settings.CurrencyTabBackgroundColor);
            Graphics.DrawFrame(drawBox, Settings.CurrencyTabBorderColor, 1);
        }

        /// <summary>
        ///     Displays price for unique items, and highlights the uniques under X value by drawing a border arround them.
        /// </summary>
        /// <param name="item"></param>
        private void HighlightJunkUniques(CustomItem item)
        {
            var hoverUi = GameController.Game.IngameState.UIHoverTooltip.Tooltip;
            if (hoverUi != null && (item.Rarity != ItemRarity.Unique ||
                                    hoverUi.GetClientRect().Intersects(item.Item.GetClientRect()) &&
                                    hoverUi.IsVisibleLocal)) return;

            var chaosValueSignificanDigits = Math.Round((decimal) item.PriceData.ChaosValue,
                Settings.HighlightSignificantDigits.Value);
            if (chaosValueSignificanDigits >= Settings.InventoryValueCutOff.Value) return;
            var rec = item.Item.GetClientRect();
            var fontSize = Settings.HighlightFontSize.Value;
            var backgroundBox = Graphics.MeasureText($"{chaosValueSignificanDigits}", fontSize);
            var position = new Vector2(rec.TopRight.X - fontSize, rec.TopRight.Y);

            //Graphics.DrawBox(new RectangleF(position.X - backgroundBox.Width, position.Y, backgroundBox.Width, backgroundBox.Height), Color.Black);
            Graphics.DrawText($"{chaosValueSignificanDigits}", position, Settings.UniTextColor, FontAlign.Center);

            DrawImage($"{DirectoryFullName}//images//Chaos_Orb_inventory_icon.png",
                new RectangleF(rec.TopRight.X - fontSize, rec.TopRight.Y, Settings.HighlightFontSize.Value,
                    Settings.HighlightFontSize.Value));
            //Graphics.DrawFrame(item.Item.GetClientRect(), 2, Settings.HighlightColor.Value);
        }

        //private void PropheccyDisplay()
        //{
        //    if (!Settings.ProphecyPrices)
        //        return;

        //    try
        //    {
        //        var UIHover = GameController.Game.IngameState.UIHover;
        //        var newBox = new RectangleF(lastProphWindowPos.X, lastProphWindowPos.Y, lastProphWindowSize.X, lastProphWindowSize.Y);

        //        if (!StashPanel.IsVisible) return;
        //        var refBool = true;

        //        if (!UIHover.Tooltip.GetClientRect().Intersects(newBox))
        //        {
        //            var menuOpacity = ImGui.GetStyle().GetColor(ColorTarget.WindowBg).W;
        //            if (Settings.ProphecyOverrideColors)
        //            {
        //                var tempColor = new SharpDX.Vector4(Settings.ProphecyBackground.Value.R / 255.0f, Settings.ProphecyBackground.Value.G / 255.0f,
        //                    Settings.ProphecyBackground.Value.B / 255.0f, Settings.ProphecyBackground.Value.A / 255.0f);
        //                ImGui.PushStyleColor(ColorTarget.WindowBg, ToImVector4(tempColor));
        //                menuOpacity = ImGui.GetStyle().GetColor(ColorTarget.WindowBg).W;
        //            }

        //            ImGui.BeginWindow("Poe.NinjaProphs", ref refBool, new System.Numerics.Vector2(200, 150), menuOpacity, Settings.ProphecyLocked ? WindowFlags.NoCollapse | WindowFlags.NoScrollbar | WindowFlags.NoMove | WindowFlags.NoResize | WindowFlags.NoInputs | WindowFlags.NoBringToFrontOnFocus | WindowFlags.NoTitleBar | WindowFlags.NoFocusOnAppearing : WindowFlags.Default | WindowFlags.NoTitleBar | WindowFlags.ResizeFromAnySide);

        //            if (Settings.ProphecyOverrideColors)
        //                ImGui.PopStyleColor();


        //            var prophystringlist = new List<string>();
        //            var propicies = GameController.Player.GetComponent<Player>().Prophecies;
        //            foreach (var prophecyDat in propicies)
        //            {
        //                //var text = $"{GetProphecyValues(prophecyDat.Name)}c - {prophecyDat.Name}({prophecyDat.SealCost})";
        //                var text = $"{{{HexConverter(Settings.ProphecyChaosValue)}}}{GetProphecyValues(prophecyDat.Name)}c {{}}- {{{HexConverter(Settings.ProphecyProecyName)}}}{prophecyDat.Name} {{{HexConverter(Settings.ProphecyProecySealColor)}}}({prophecyDat.SealCost}){{}}";
        //                if (prophystringlist.Any(x => Equals(x, text))) continue;
        //                prophystringlist.Add(text);
        //            }

        //            foreach (var proph in prophystringlist)
        //                //ImGui.Text(VARIABLE);
        //                Coloredtext(proph);

        //            lastProphWindowSize = new Vector2(ImGui.GetWindowSize().X, ImGui.GetWindowSize().Y);
        //            lastProphWindowPos = new Vector2(ImGui.GetWindowPosition().X, ImGui.GetWindowPosition().Y);
        //            ImGui.EndWindow();
        //        }
        //    }
        //    catch
        //    {
        //        ImGui.EndWindow();
        //    }
        //}
    }
}