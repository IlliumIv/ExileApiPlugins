namespace Stashie
{
    public class WarlordItemFilter : IIFilter
    {
        public bool isWarlord;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.isWarlord == isWarlord;
        }
    }
}