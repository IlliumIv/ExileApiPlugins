namespace Stashie
{
    public class AnyInfluenceItemFilter : IIFilter
    {
        public bool isInfluenced;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.isInfluenced == isInfluenced;
        }
    }
}
