using System;

namespace FancyScrollView
{
    public interface IFancyScrollRectContext
    {
        Func<(float ScrollSize, float ReuseMargin)> CalculateScrollSize { get; set; }
    }
}
