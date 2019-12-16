/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;

namespace FancyScrollView.Example08
{
    public class FancyGridView : FancyGridView<ItemData, Context>
    {
        [SerializeField] int columnCount = 3;
        [SerializeField] Cell cellPrefab = default;
        [SerializeField] Row rowPrefab = default;

        protected override int ColumnCount => columnCount;
        protected override FancyScrollViewCell<ItemData, Context> CellTemplate => cellPrefab;
        protected override FancyGridViewRow<ItemData, Context> RowTemplate => rowPrefab;

        public float PaddingTop
        {
            get => paddingHead;
            set
            {
                paddingHead = value;
                Refresh();
            }
        }

        public float PaddingBottom
        {
            get => paddingTail;
            set
            {
                paddingTail = value;
                Refresh();
            }
        }

        public float SpacingY
        {
            get => spacing;
            set
            {
                spacing = value;
                Refresh();
            }
        }

        public float SpacingX
        {
            get => columnSpacing;
            set
            {
                columnSpacing = value;
                Refresh();
            }
        }

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
