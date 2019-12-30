/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using EasingCore;

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

        public void ScrollTo(int index, float duration, Ease easing, Alignment alignment = Alignment.Center)
        {
            UpdateSelection(index);
            ScrollTo(index, duration, easing, GetAnchor(alignment));
        }

        public void JumpTo(int index, Alignment alignment = Alignment.Center)
        {
            UpdateSelection(index);
            JumpTo(index, GetAnchor(alignment));
        }

        float GetAnchor(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Head: return 0.0f;
                case Alignment.Center: return 0.5f;
                case Alignment.Tail: return 1.0f;
                default: return GetAnchor(Alignment.Center);
            }
        }
    }
}
