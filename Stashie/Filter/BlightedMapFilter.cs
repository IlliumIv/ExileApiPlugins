namespace Stashie
{
    public class BlightedMapFilter : IIFilter
    {
        public bool isBlightMap;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.isBlightMap == isBlightMap;
        }
    }
}
