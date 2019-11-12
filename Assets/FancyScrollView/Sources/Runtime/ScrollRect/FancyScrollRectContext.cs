using System;

namespace FancyScrollView
{
    public class FancyScrollRectContext : IFancyScrollRectContext
    {
        Func<(float ScrollSize, float ReuseMargin)> IFancyScrollRectContext.CalculateScrollSize { get; set; }
    }
}
