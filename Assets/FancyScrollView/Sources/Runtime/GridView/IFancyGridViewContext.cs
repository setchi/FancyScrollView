using System;
using UnityEngine;

namespace FancyScrollView
{
    public interface IFancyGridViewContext
    {
        Func<int> GetColumnCount { get; set; }
        Func<float> GetColumnSpacing { get; set; }
        GameObject CellTemplate { get; set; }
        ScrollDirection ScrollDirection { get; set; }
    }
}
