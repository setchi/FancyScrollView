using System;

namespace FancyScrollView
{
    public class FancyScrollRectContext : IFancyScrollRectContext
    {
        Func<(float CellInterval, float ReuseMarginCount)> IFancyScrollRectContext.CalculateScrollSize { get; set; }
    }
}
