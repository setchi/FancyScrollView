using UnityEngine;

namespace FancyScrollView.Example08
{
    public class FancyGridView : FancyGridView<ItemData, Context>
    {
        [SerializeField] GameObject cellPrefab = default;

        protected override GameObject CellPrefab => cellPrefab;

        protected override int ColumnCount => 3;

        public void UpdateSelection(int index)
        {
            if (Context.SelectedItemIndex == index)
            {
                return;
            }

            Context.SelectedItemIndex = index;
            Refresh();
        }
    }
}
