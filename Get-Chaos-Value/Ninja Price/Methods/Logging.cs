using SharpDX;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Ninja_Price.Main
{
    public partial class Main
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return $"{GetType().Name}.{sf.GetMethod().Name}";
        }
    }
}