/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyGridView{TItemData, TContext}"/> のコンテキストインターフェース.
    /// </summary>
    public interface IFancyGridViewContext : IFancyScrollRectContext, IFancyCellGroupContext
    {
        Func<float> GetStartAxisSpacing { get; set; }
        Func<float> GetCellSize { get; set ; }
    }
}
