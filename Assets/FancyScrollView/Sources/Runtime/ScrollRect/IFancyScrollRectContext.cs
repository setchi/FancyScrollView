using System;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyScrollRect{TItemData, TContext}"/> のコンテキストインターフェース.
    /// </summary>
    public interface IFancyScrollRectContext
    {
        Func<(float ScrollSize, float ReuseMargin)> CalculateScrollSize { get; set; }
    }
}
