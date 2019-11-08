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

        public void UpdateData(IList<ItemData> items)
        {
            UpdateContents(items);
        }

        public void ScrollTo(int index, float duration, Alignment alignment = Alignment.Center)
        {
            UpdateSelection(index);
            ScrollTo(index, duration, Ease.InOutQuint, alignment);
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
