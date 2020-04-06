namespace Stashie
{
    public class RedeemerItemFilter : IIFilter
    {
        public bool isRedeemer;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.isRedeemer == isRedeemer;
        }
    }
}
