using System;
using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView.Example02
{
    public class ScrollView : FancyScrollView<ItemData, Context>
    {
        [SerializeField] Scroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;

        Action<int> onSelectionChanged;

        protected override GameObject CellPrefab => cellPrefab;

        void Awake()
        {
            Context.OnCellClicked = UpdateSelection;
        }

        void Start()
        {
            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
        }

        public void UpdateData(IList<ItemData> items)
        {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }

        public void OnSelectionChanged(Action<int> callback)
        {
            onSelectionChanged = callback;
        }

        public void SelectNextCell()
        {
            UpdateSelection(Context.SelectedIndex + 1);
        }

        public void SelectPrevCell()
        {
            UpdateSelection(Context.SelectedIndex - 1);
        }

        public void UpdateSelection(int index)
        {
            if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
            {
                return;
            }

            Context.SelectedIndex = index;
            Refresh();

            scroller.ScrollTo(index, 0.35f);
            onSelectionChanged?.Invoke(index);
        }
    }
}
