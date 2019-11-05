using System;

namespace FancyScrollView
{
    public interface IFancyScrollRectContext
    {
        Func<float> GetViewportSize { get; set; }
        Func<float> GetCellInterval { get; set; }
    }
}
