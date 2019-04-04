using System;

namespace FancyScrollView.Example02
{
    public class Context
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}
