/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using EasingCore;

namespace FancyScrollView.Example08
{
    public class GridView : FancyGridView<ItemData, Context>
    {
        class CellGroup : DefaultCellGroup { }

        [SerializeField] Cell cellPrefab = default;

        protected override void SetupCellTemplate() => Setup<CellGroup>(cellPrefab);

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
            get => startAxisSpacing;
            set
            {
                startAxisSpacing = value;
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

        public void ScrollTo(int index, float duration, Ease easing, Alignment alignment = Alignment.Middle)
        {
            UpdateSelection(index);
            ScrollTo(index, duration, easing, GetAlignment(alignment));
        }

        public void JumpTo(int index, Alignment alignment = Alignment.Middle)
        {
            UpdateSelection(index);
            JumpTo(index, GetAlignment(alignment));
        }

        float GetAlignment(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Upper: return 0.0f;
                case Alignment.Middle: return 0.5f;
                case Alignment.Lower: return 1.0f;
                default: return GetAlignment(Alignment.Middle);
            }
        }
    }
}
