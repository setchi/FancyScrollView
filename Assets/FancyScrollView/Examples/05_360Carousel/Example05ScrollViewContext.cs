using System;

namespace FancyScrollView
{
    public class Example05ScrollViewContext
    {
        int selectedIndex = -1;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                var prevSelectedIndex = selectedIndex;
                selectedIndex = value;
                if (prevSelectedIndex != selectedIndex)
                {
                    if (OnSelectedIndexChanged != null)
                    {
                        OnSelectedIndexChanged(selectedIndex);
                    }
                }
            }
        }

        public Action<Example05ScrollViewCell> OnPressedCell;
        public Action<int> OnSelectedIndexChanged;
    }
}
