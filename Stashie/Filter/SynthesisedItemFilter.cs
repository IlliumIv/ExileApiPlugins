namespace Stashie
{
    public class SynthesisedItemFilter : IIFilter
    {
        public bool IsSynthesised;

        public bool CompareItem(ItemData itemData)
        {
            return itemData.Synthesised == IsSynthesised;
        }
    }
}
