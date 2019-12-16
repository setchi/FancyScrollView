/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyGridView{TItemData, TContext}"/> のコンテキスト基底クラス.
    /// </summary>
    public class FancyGridViewContext : IFancyGridViewContext, IFancyScrollRectContext
    {
        Func<(float ScrollSize, float ReuseMargin)> IFancyScrollRectContext.CalculateScrollSize { get; set; }
        GameObject IFancyGridViewContext.CellTemplate { get; set; }
        ScrollDirection IFancyGridViewContext.ScrollDirection { get; set; }
        public Func<int> GetColumnCount { get; set; }
        public Func<float> GetColumnSpacing { get; set; }
        public Func<float> GetCellSize { get; set; }
    }
}
