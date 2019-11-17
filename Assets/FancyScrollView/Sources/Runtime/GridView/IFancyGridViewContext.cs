using System;
using UnityEngine;

namespace FancyScrollView
{
    public interface IFancyGridViewContext
    {
        GameObject CellTemplate { get; set; }
        ScrollDirection ScrollDirection { get; set; }
        Func<int> GetColumnCount { get; set; }
        Func<float> GetColumnSpacing { get; set; }
    }
}
