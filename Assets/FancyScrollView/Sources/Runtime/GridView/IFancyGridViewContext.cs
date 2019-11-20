using System;
using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyGridView{TItemData, TContext}"/> のコンテキストインターフェース.
    /// </summary>
    public interface IFancyGridViewContext
    {
        GameObject CellTemplate { get; set; }
        ScrollDirection ScrollDirection { get; set; }
        Func<int> GetColumnCount { get; set; }
        Func<float> GetColumnSpacing { get; set; }
    }
}
