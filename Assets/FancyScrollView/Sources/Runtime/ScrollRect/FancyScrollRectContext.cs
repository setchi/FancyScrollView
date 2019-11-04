using System;

namespace FancyScrollView
{
    public class FancyScrollRectContext : IFancyScrollRectContext
    {
        Func<float> IFancyScrollRectContext.GetViewportSize { get; set; }
        Func<float> IFancyScrollRectContext.GetFancyScrollViewportSize { get; set; }
    }
}
