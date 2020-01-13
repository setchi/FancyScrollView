/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyCellGroup{TItemData, TContext}"/> のコンテキストインターフェース.
    /// </summary>
    public interface IFancyCellGroupContext
    {
        GameObject CellTemplate { get; set; }
        Func<int> GetGroupCount { get; set; }
    }
}
