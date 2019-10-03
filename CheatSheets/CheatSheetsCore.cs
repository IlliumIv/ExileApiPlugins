using ExileCore;
using ExileCore.PoEMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using CheatSheets.Leagues;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using System.Collections;
using System.Windows.Forms;

namespace CheatSheets
{
    public partial class CheatSheetsCore : BaseSettingsPlugin<Settings>
    {
        private bool isLoading;
        private bool working;
        private bool canRender;
        private DebugInformation debugInformation;
        private readonly List<long> filesPtr = new List<long>();
        public static List<string> CurrentAreaPreloads = new List<string>();
        public static List<Sheet> SheetsList { get; private set; } = new List<Sheet>();

        public override void OnLoad()
        {
            ConfigPreload();
        }

        private void ConfigPreload()
        {
            SheetsList.Add(new Incursion());
            SheetsList.Add(new Betrayal());
        }

        public override bool Initialise()
        {
            Name = "CheatSheets";

            InitialiseTexture();

            debugInformation = new DebugInformation("Preload parsing", false);

            AreaChange(GameController.Area.CurrentArea);

            return base.Initialise();
        }

        public override Job Tick()
        {
            canRender = true;

            if (!Settings.Enable
                || GameController.Area.CurrentArea != null
                || GameController.IsLoading
                || !GameController.InGame
                || GameController.Game.IngameState.IngameUi.StashElement.IsVisibleLocal)
            {
                canRender = false;
            }

            if (Input.GetKeyState(Keys.F5)) AreaChange(GameController.Area.CurrentArea);

            /*
            if (Input.GetKeyState(Keys.F5))
            {
                LogMessage("Drawing CurrentAreaPreloads");

                foreach (string text in CurrentAreaPreloads)
                    LogMessage(text);
            }
            */

            return null;
        }

        public override void Render()
        {
            if (!canRender) return;

            if (isLoading)
            {
                // Draw "Loading..."
            }
            else
            {
               // Draw Sheets
            }
        }

        public override void AreaChange(AreaInstance area)
        {
            isLoading = true;

            foreach (var sheet in SheetsList)
                sheet.AllowIconDrawing = false;

            if (GameController.Area.CurrentArea.IsHideout && !Settings.ShowInHideout)
            {
                isLoading = false;
                return;
            }

            LogMessage("Current Area: " + area.DisplayName);

            // var text = "Metadata";
            // var sheets_preload = Preloads.Values.Where(list => text.StartsWith(list.Any<string>, StringComparison.OrdinalIgnoreCase))
            //     .Select(kv => kv.Value).FirstOrDefault();

            Core.ParallelRunner.Run(new Coroutine(Parse(), this, "Preload parse"));
            isLoading = false;
        }

        private IEnumerator Parse()
        {
            if (!working)
            {
                working = true;

                Task.Run(() =>
                {
                    debugInformation.TickAction(() =>
                    {
                        var memory = GameController.Memory;
                        var pFileRoot = memory.AddressOfProcess + memory.BaseOffsets[OffsetsName.FileRoot];
                        var count = memory.Read<int>(pFileRoot + 0x10); // check how many files are loaded
                        var areaChangeCount = GameController.Game.AreaChangeCount;
                        var listIterator = memory.Read<long>(pFileRoot + 0x8, 0x0);
                        filesPtr.Clear();

                        for (var i = 0; i < count; i++)
                        {
                            listIterator = memory.Read<long>(listIterator);

                            if (listIterator == 0)
                            {
                                //MessageBox.Show("address is null, something has gone wrong, start over");
                                // address is null, something has gone wrong, start over
                                break;
                            }

                            filesPtr.Add(listIterator);
                        }

                        if (Settings.ParallelParsing)
                        {
                            Parallel.ForEach(filesPtr, (iter, state) =>
                            {
                                try
                                {
                                    var fileAddr = memory.Read<long>(iter + 0x18);

                                    //some magic number

                                    if (memory.Read<long>(iter + 0x10) != 0 && memory.Read<int>(fileAddr + 0x48) == areaChangeCount)
                                    {
                                        var size = memory.Read<int>(fileAddr + 0x30);
                                        if (size < 7) return;

                                        var fileNamePointer = memory.Read<long>(iter + 0x10);

                                        var text = RemoteMemoryObject.Cache.StringCache.Read($"{nameof(CheatSheetsCore)}{fileNamePointer}",
                                            () => memory.ReadStringU(
                                                fileNamePointer, size * 2));

                                        if (text.Contains('@')) text = text.Split('@')[0];

                                        CurrentAreaPreloads.Add(text);
                                        CheckForSheets(text);
                                    }
                                }
                                catch (Exception e)
                                {
                                    DebugWindow.LogError($"{nameof(CheatSheetsCore)} -> {e}");
                                }
                            });
                        }
                        else
                        {
                            string text;

                            foreach (var iter in filesPtr)
                            {
                                try
                                {
                                    var fileAddr = memory.Read<long>(iter + 0x18);

                                    if (memory.Read<long>(iter + 0x10) != 0 && memory.Read<int>(fileAddr + 0x48) == areaChangeCount)
                                    {
                                        var size = memory.Read<int>(fileAddr + 0x30);
                                        if (size < 7) continue;

                                        var fileNamePointer = memory.Read<long>(iter + 0x10);

                                        text = RemoteMemoryObject.Cache.StringCache.Read($"{nameof(CheatSheetsCore)}{fileNamePointer}",
                                            () => memory.ReadStringU(
                                                fileNamePointer, size * 2));

                                        if (text.Contains('@')) text = text.Split('@')[0];

                                        CurrentAreaPreloads.Add(text);
                                        CheckForSheets(text);
                                    }
                                }
                                catch (Exception e)
                                {
                                    DebugWindow.LogError($"{nameof(CheatSheetsCore)} -> {e}");
                                }
                            }
                        }
                    });

                    working = false;
                });
            }

            yield return null;
        }

        private void CheckForSheets(string text)
        {
            foreach (var sheet in SheetsList)
                if (sheet.Preloads.Contains("", StringComparer.OrdinalIgnoreCase))
                {
                    sheet.AllowIconDrawing = true;
                    LogMessage(sheet.Name + " contains preload " + text);
                    break;
                }
            // var sheets_preload = Preloads.Where(kv => text.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
            //     .Select(kv => kv.Value).FirstOrDefault();

            // foreach (string preload in sheets_preload)
            //    LogMessage(preload);

            /*
            if (alertStrings.ContainsKey(text))
            {
                lock (_locker)
                {
                    alerts[alertStrings[text].Text] = alertStrings[text];
                }

                return;
            }

            if (text.Contains("Metadata/Terrain/Doodads/vaal_sidearea_effects/soulcoaster.ao"))
            {
                if (Settings.CorruptedTitle)
                {
                    // using corrupted titles so set the color here, XpRatePlugin will grab the color to use when drawing the title.
                    AreaNameColor = Settings.CorruptedAreaColor;
                    GameController.Area.CurrentArea.AreaColorName = AreaNameColor;
                }
                else
                {
                    // not using corrupted titles, so throw it in a preload alert
                    lock (_locker)
                    {
                        alerts[text] = new PreloadConfigLine { Text = "Corrupted Area", FastColor = () => Settings.CorruptedAreaColor };
                    }
                }

                return;
            }

            if (Settings.Essence)
            {
                var essence_alert = Essences.Where(kv => text.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase)).Select(kv => kv.Value)
                    .FirstOrDefault();

                if (essence_alert != null)
                {
                    essencefound = true;

                    if (alerts.ContainsKey("Remnant of Corruption"))

                    //TODO: TEST ESSENCE
                    {
                        lock (_locker)
                        {
                            alerts.Remove("Remnant of Corruption");
                        }
                    }

                    lock (_locker)
                    {
                        alerts[essence_alert.Text] = essence_alert;
                    }

                    return;
                }

                if (!essencefound && text.Contains("MiniMonolith"))
                {
                    lock (_locker)
                    {
                        alerts["Remnant of Corruption"] = new PreloadConfigLine
                        {
                            Text = "Remnant of Corruption",
                            FastColor = () => Settings.RemnantOfCorruption
                        };
                    }
                }
            }

            var perandus_alert = PerandusLeague.Where(kv => text.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
                .Select(kv => kv.Value).FirstOrDefault();

            if (perandus_alert != null && Settings.PerandusBoxes)
            {
                foundSpecificPerandusChest = true;

                if (alerts.ContainsKey("Unknown Perandus Chest"))
                {
                    lock (_locker)
                    {
                        alerts.Remove("Unknown Perandus Chest");
                    }
                }

                lock (_locker)
                {
                    alerts.Add(perandus_alert.Text, perandus_alert);
                }

                return;
            }

            if (Settings.PerandusBoxes && !foundSpecificPerandusChest && text.StartsWith("Metadata/Chests/PerandusChests"))
            {
                lock (_locker)
                {
                    alerts["Unknown Perandus Chest"] = new PreloadConfigLine
                    {
                        Text = "Unknown Perandus Chest",
                        FastColor = () => Settings.PerandusChestStandard
                    };
                }
            }

            var _alert = Strongboxes.Where(kv => text.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase)).Select(kv => kv.Value)
                .FirstOrDefault();

            if (_alert != null && Settings.Strongboxes)
            {
                lock (_locker)
                {
                    alerts[_alert.Text] = _alert;
                }

                return;
            }

            // var alert = SheetsPreload.Where(kv => text.EndsWith(kv.Key, StringComparison.OrdinalIgnoreCase)).Select(kv => kv.Value)
                .FirstOrDefault();

            if (alert != null && Settings.Exiles)
            {
                lock (_locker)
                {
                    alerts[alert.Text] = alert;
                }
            }
            */
        }

        // private void CheckForPreload(string text)
        // {
        //     var alert = Preload.Where(kv => text.EndsWith(kv.Key, StringComparison.OrdinalIgnoreCase)).Select(kv => kv.Value)
        //     .FirstOrDefault();
        // }
    }
}
