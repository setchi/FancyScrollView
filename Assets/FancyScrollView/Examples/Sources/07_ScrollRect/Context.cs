using System;

namespace FancyScrollView.Example07
{
    public class Context
    {
        public int SelectedIndex = -1;
        public Func<float> GetVisibleCellCount;
        public Func<float> GetViewportSize;
    }
}
