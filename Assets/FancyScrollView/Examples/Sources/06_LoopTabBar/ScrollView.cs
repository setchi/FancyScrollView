using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView.Example06
{
    public class ScrollView : FancyScrollView<ItemData, Context>
    {
        [SerializeField] Scroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;

        Action<int, Scroller.MovementDirection> onSelectionChanged;

        protected override GameObject CellPrefab => cellPrefab;

        void Awake()
        {
            Context.OnCellClicked = SelectCell;
            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
        }

        void UpdateSelection(int index)
        {
            if (Context.SelectedIndex == index)
            {
                return;
            }

            var direction = scroller.GetMovementDirection(Context.SelectedIndex, index);

            Context.SelectedIndex = index;
            Refresh();

            onSelectionChanged?.Invoke(index, direction);
        }

        public void UpdateData(IList<ItemData> items)
        {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }

        public void OnSelectionChanged(Action<int, Scroller.MovementDirection> callback)
        {
            onSelectionChanged = callback;
        }

        public void SelectNextCell()
        {
            SelectCell(Context.SelectedIndex + 1);
        }

        public void SelectPrevCell()
        {
            SelectCell(Context.SelectedIndex - 1);
        }

        public void SelectCell(int index)
        {
            if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
            {
                return;
            }

            scroller.ScrollTo(index, 0.35f, Ease.OutCubic);
        }
    }
}
