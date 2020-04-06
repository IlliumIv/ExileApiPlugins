namespace Stashie
{
    public class CrusaderItemFilter : IIFilter
    {
        public bool isCrusader;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.isCrusader == isCrusader;
        }
    }
}
