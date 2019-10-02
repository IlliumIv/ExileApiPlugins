using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ninja_Price.API.PoeNinja;
using Ninja_Price.API.PoeNinja.Classes;
using SharpDX;

namespace Ninja_Price.Main
{
    public partial class Main
    {
        private const string CurrencyURL = "https://poe.ninja/api/data/currencyoverview?type=Currency&league=";
        private const string DivinationCards_URL = "https://poe.ninja/api/data/itemoverview?type=DivinationCard&league=";
        private const string Essences_URL = "https://poe.ninja/api/data/itemoverview?type=Essence&league=";
        private const string Fragments_URL = "https://poe.ninja/api/data/currencyoverview?type=Fragment&league=";
        private const string Prophecies_URL = "https://poe.ninja/api/data/itemoverview?type=Prophecy&league=";
        private const string UniqueAccessories_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueAccessory&league=";
        private const string UniqueArmours_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueArmour&league=";
        private const string UniqueFlasks_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueFlask&league=";
        private const string UniqueJewels_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueJewel&league=";
        private const string UniqueMaps_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueMap&league=";
        private const string UniqueWeapons_URL = "https://poe.ninja/api/data/itemoverview?type=UniqueWeapon&league=";
        private const string WhiteMaps_URL = "https://poe.ninja/api/data/itemoverview?type=Map&league=";
        private const string Resonators_URL = "https://poe.ninja/api/data/itemoverview?type=Resonator&league=";
        private const string Fossils_URL = "https://poe.ninja/api/data/itemoverview?type=Fossil&league=";
        private const string Scarabs_URL = "https://poe.ninja/api/data/itemoverview?type=Scarab&league=";
        private const string Incubators_URL = "https://poe.ninja/api/data/itemoverview?type=Incubator&league=";
        private const string Oil_URL = "https://poe.ninja/api/data/itemoverview?type=Oil&league=";

        private void GetJsonData(string league)
        {
            Task.Run(() =>
            {
                while (UpdatingFromAPI || UpdatingFromJson)
                {
                    if (Settings.Debug)
                        LogMessage($"{GetCurrentMethod()}: Waiting on UpdatePoeNinjaData() to finish", 5, Color.Orange);
                    Thread.Sleep(250);
                }

                LogMessage("Gathering Data from Poe.Ninja.", 5);
                UpdatingFromAPI = true;
                Api.Json.SaveSettingFile(NinjaDirectory + "Currency.json", JsonConvert.DeserializeObject<Currency.RootObject>(Api.DownloadFromUrl(CurrencyURL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "DivinationCards.json", JsonConvert.DeserializeObject<DivinationCards.RootObject>(Api.DownloadFromUrl(DivinationCards_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "Essences.json", JsonConvert.DeserializeObject<Essences.RootObject>(Api.DownloadFromUrl(Essences_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "Fragments.json", JsonConvert.DeserializeObject<Fragments.RootObject>(Api.DownloadFromUrl(Fragments_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "Prophecies.json", JsonConvert.DeserializeObject<Prophecies.RootObject>(Api.DownloadFromUrl(Prophecies_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "UniqueAccessories.json", JsonConvert.DeserializeObject<UniqueAccessories.RootObject>(Api.DownloadFromUrl(UniqueAccessories_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "UniqueArmours.json", JsonConvert.DeserializeObject<UniqueArmours.RootObject>(Api.DownloadFromUrl(UniqueArmours_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "UniqueFlasks.json", JsonConvert.DeserializeObject<UniqueFlasks.RootObject>(Api.DownloadFromUrl(UniqueFlasks_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "UniqueJewels.json", JsonConvert.DeserializeObject<UniqueJewels.RootObject>(Api.DownloadFromUrl(UniqueJewels_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "UniqueMaps.json", JsonConvert.DeserializeObject<UniqueMaps.RootObject>(Api.DownloadFromUrl(UniqueMaps_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "UniqueWeapons.json", JsonConvert.DeserializeObject<UniqueWeapons.RootObject>(Api.DownloadFromUrl(UniqueWeapons_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "WhiteMaps.json", JsonConvert.DeserializeObject<WhiteMaps.RootObject>(Api.DownloadFromUrl(WhiteMaps_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "Resonators.json", JsonConvert.DeserializeObject<Resonators.RootObject>(Api.DownloadFromUrl(Resonators_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "Fossils.json", JsonConvert.DeserializeObject<Fossils.RootObject>(Api.DownloadFromUrl(Fossils_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "Incubators.json", JsonConvert.DeserializeObject<Incubators.RootObject>(Api.DownloadFromUrl(Incubators_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "Scarabs.json", JsonConvert.DeserializeObject<Scarab.RootObject>(Api.DownloadFromUrl(Scarabs_URL + league)));
                Api.Json.SaveSettingFile(NinjaDirectory + "Oils.json", JsonConvert.DeserializeObject<Oils.RootObject>(Api.DownloadFromUrl(Oil_URL + league)));
                LogMessage("Finished Gathering Data from Poe.Ninja.", 5);
                UpdatingFromAPI = false;
                UpdatePoeNinjaData();
            });
        }

        public bool JsonExists(string fileName)
        {
            return File.Exists(NinjaDirectory + fileName);
        }

        private void UpdatePoeNinjaData()
        {
            Task.Run(() =>
            {
                while (UpdatingFromAPI || UpdatingFromJson)
                {
                    if (Settings.Debug)
                        LogMessage($"{GetCurrentMethod()}: Waiting on GetJsonData() to finish", 5, Color.Orange);
                    Thread.Sleep(250);
                }

                var newData = new CollectiveApiData();

                UpdatingFromJson = true;
                if (JsonExists("Currency.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Currency.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Currency = JsonConvert.DeserializeObject<Currency.RootObject>(json);
                    }

                if (JsonExists("DivinationCards.json"))
                    using (var r = new StreamReader(NinjaDirectory + "DivinationCards.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.DivinationCards = JsonConvert.DeserializeObject<DivinationCards.RootObject>(json);
                    }

                if (JsonExists("Essences.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Essences.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Essences = JsonConvert.DeserializeObject<Essences.RootObject>(json);
                    }

                if (JsonExists("Fragments.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Fragments.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Fragments = JsonConvert.DeserializeObject<Fragments.RootObject>(json);
                    }

                if (JsonExists("Prophecies.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Prophecies.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Prophecies = JsonConvert.DeserializeObject<Prophecies.RootObject>(json);
                    }

                if (JsonExists("UniqueAccessories.json"))
                    using (var r = new StreamReader(NinjaDirectory + "UniqueAccessories.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.UniqueAccessories = JsonConvert.DeserializeObject<UniqueAccessories.RootObject>(json);
                    }

                if (JsonExists("UniqueArmours.json"))
                    using (var r = new StreamReader(NinjaDirectory + "UniqueArmours.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.UniqueArmours = JsonConvert.DeserializeObject<UniqueArmours.RootObject>(json);
                    }

                if (JsonExists("UniqueFlasks.json"))
                    using (var r = new StreamReader(NinjaDirectory + "UniqueFlasks.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.UniqueFlasks = JsonConvert.DeserializeObject<UniqueFlasks.RootObject>(json);
                    }

                if (JsonExists("UniqueJewels.json"))
                    using (var r = new StreamReader(NinjaDirectory + "UniqueJewels.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.UniqueJewels = JsonConvert.DeserializeObject<UniqueJewels.RootObject>(json);
                    }

                if (JsonExists("UniqueMaps.json"))
                    using (var r = new StreamReader(NinjaDirectory + "UniqueMaps.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.UniqueMaps = JsonConvert.DeserializeObject<UniqueMaps.RootObject>(json);
                    }

                if (JsonExists("UniqueWeapons.json"))
                    using (var r = new StreamReader(NinjaDirectory + "UniqueWeapons.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.UniqueWeapons = JsonConvert.DeserializeObject<UniqueWeapons.RootObject>(json);
                    }

                if (JsonExists("WhiteMaps.json"))
                    using (var r = new StreamReader(NinjaDirectory + "WhiteMaps.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.WhiteMaps = JsonConvert.DeserializeObject<WhiteMaps.RootObject>(json);
                    }

                if (JsonExists("Resonators.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Resonators.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Resonators = JsonConvert.DeserializeObject<Resonators.RootObject>(json);
                    }

                if (JsonExists("Fossils.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Fossils.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Fossils = JsonConvert.DeserializeObject<Fossils.RootObject>(json);
                    }

                if (JsonExists("Incubators.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Incubators.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Incubators = JsonConvert.DeserializeObject<Incubators.RootObject>(json);
                    }

                if (JsonExists("Scarabs.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Scarabs.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Scarabs = JsonConvert.DeserializeObject<Scarab.RootObject>(json);
                    }

                if (JsonExists("Oils.json"))
                    using (var r = new StreamReader(NinjaDirectory + "Oils.json"))
                    {
                        var json = r.ReadToEnd();
                        newData.Oils = JsonConvert.DeserializeObject<Oils.RootObject>(json);
                    }

                CollectedData = newData;
                LogMessage("Updated CollectedData.", 5);
                UpdatingFromJson = false;
            });
        }
    }
}