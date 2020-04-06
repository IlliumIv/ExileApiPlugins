namespace Stashie
{
    public class HunterItemFilter : IIFilter
    {
        public bool isHunter;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.isHunter == isHunter;
        }
    }
}
