/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyScrollRect{TItemData, TContext}"/> のコンテキスト基底クラス.
    /// </summary>
    public class FancyScrollRectContext : IFancyScrollRectContext
    {
        ScrollDirection IFancyScrollRectContext.ScrollDirection { get; set; }
        Func<(float ScrollSize, float ReuseMargin)> IFancyScrollRectContext.CalculateScrollSize { get; set; }
    }
}
