namespace FancyScrollView.Example08
{
    public class Context : FancyScrollRectContext, IFancyGridViewContext
    {
        public int SelectedItemIndex = -1;

        public int ColumnCount { get; set; }
    }
}
