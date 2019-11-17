using System;
using UnityEngine;

namespace FancyScrollView
{
    public class FancyGridViewContext : IFancyGridViewContext, IFancyScrollRectContext
    {
        Func<(float ScrollSize, float ReuseMargin)> IFancyScrollRectContext.CalculateScrollSize { get; set; }

        public Func<int> GetColumnCount { get; set; }

        public Func<float> GetColumnSpacing { get; set; }

        public GameObject CellTemplate { get; set; }

        public ScrollDirection ScrollDirection { get; set; }
    }
}
