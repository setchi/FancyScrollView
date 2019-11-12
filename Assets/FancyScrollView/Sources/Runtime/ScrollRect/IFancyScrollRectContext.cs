using System;

namespace FancyScrollView
{
    public interface IFancyScrollRectContext
    {
        Func<(float CellInterval, float ReuseMarginCount)> CalculateScrollSize { get; set; }
    }
}
