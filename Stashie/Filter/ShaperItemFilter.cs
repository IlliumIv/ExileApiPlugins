namespace Stashie
{
    public class ShaperItemFilter : IIFilter
    {
        public bool isShaper;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.isShaper == isShaper;
        }
    }
}
