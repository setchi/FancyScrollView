using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView.Example07
{
    public class FancyScrollRect : FancyScrollRect<ItemData, Context>
    {
        [SerializeField] GameObject cellPrefab = default;

        protected override GameObject CellPrefab => cellPrefab;

        public int DataCount => ItemsSource.Count;

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

        public float Spacing
        {
            get => spacing;
            set
            {
                spacing = value;
                Refresh();
            }
        }

        public void UpdateData(IList<ItemData> items)
        {
            UpdateContents(items);
        }

        public void ScrollTo(int index, float duration, Ease easing, Alignment alignment = Alignment.Center)
        {
            UpdateSelection(index);
            base.ScrollTo(index, duration, easing, alignment);
        }

        public void JumpTo(int index, Alignment alignment = Alignment.Center)
        {
            UpdateSelection(index);
            UpdatePosition(index, alignment);
        }

        void UpdateSelection(int index)
        {
            if (Context.SelectedIndex == index)
            {
                return;
            }

            Context.SelectedIndex = index;
            Refresh();
        }
    }
}
