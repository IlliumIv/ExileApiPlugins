namespace Stashie
{
    public class CorruptedItemFilter : IIFilter
    {
        public bool BCorrupted;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.isCorrupted == BCorrupted;
        }
    }
}
