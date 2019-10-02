using Ninja_Price.API.PoeNinja.Classes;

namespace Ninja_Price.Main
{
    public class CollectiveApiData
    {
        public Currency.RootObject Currency { get; set; } = new Currency.RootObject();
        public DivinationCards.RootObject DivinationCards { get; set; } = new DivinationCards.RootObject();
        public Essences.RootObject Essences { get; set; } = new Essences.RootObject();
        public Fragments.RootObject Fragments { get; set; } = new Fragments.RootObject();
        public HelmetEnchants.RootObject HelmetEnchants { get; set; } = new HelmetEnchants.RootObject();
        public Prophecies.RootObject Prophecies { get; set; } = new Prophecies.RootObject();
        public UniqueAccessories.RootObject UniqueAccessories { get; set; } = new UniqueAccessories.RootObject();
        public UniqueArmours.RootObject UniqueArmours { get; set; } = new UniqueArmours.RootObject();
        public UniqueFlasks.RootObject UniqueFlasks { get; set; } = new UniqueFlasks.RootObject();
        public UniqueJewels.RootObject UniqueJewels { get; set; } = new UniqueJewels.RootObject();
        public UniqueMaps.RootObject UniqueMaps { get; set; } = new UniqueMaps.RootObject();
        public UniqueWeapons.RootObject UniqueWeapons { get; set; } = new UniqueWeapons.RootObject();
        public WhiteMaps.RootObject WhiteMaps { get; set; } = new WhiteMaps.RootObject();
        public Resonators.RootObject Resonators { get; set; } = new Resonators.RootObject();
        public Fossils.RootObject Fossils { get; set; } = new Fossils.RootObject();
        public Scarab.RootObject Scarabs { get; set; } = new Scarab.RootObject();
        public Oils.RootObject Oils { get; set; } = new Oils.RootObject();
        public Incubators.RootObject Incubators { get; set; } = new Incubators.RootObject();
    }
}