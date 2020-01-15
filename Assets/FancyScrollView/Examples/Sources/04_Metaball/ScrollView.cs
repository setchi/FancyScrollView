/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView.Example04
{
    class ScrollView : FancyScrollView<ItemData, Context>
    {
        [SerializeField] Scroller scroller = default;
        [SerializeField] GameObject cellPrefab = default;

        Action<int> onSelectionChanged;

        protected override GameObject CellPrefab => cellPrefab;

        public int CellInstanceCount => Mathf.CeilToInt(1f / Mathf.Max(cellInterval, 1e-3f));

        protected override void Initialize()
        {
            base.Initialize();

            Context.OnCellClicked = SelectCell;

            scroller.OnValueChanged(UpdatePosition);
            scroller.OnSelectionChanged(UpdateSelection);
        }

        public void UpdateSelection(int index)
        {
            if (Context.SelectedIndex == index)
            {
                return;
            }

            Context.SelectedIndex = index;
            Refresh();

            onSelectionChanged?.Invoke(index);
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

            UpdateSelection(index);
            ScrollTo(index, 0.35f, Ease.OutCubic);
        }

        public void ScrollTo(float position, float duration, Ease easing, Action onComplete = null)
        {
            scroller.ScrollTo(position, duration, easing, onComplete);
        }

        public void JumpTo(int index)
        {
            scroller.JumpTo(index);
        }

        public Vector4[] GetCellState()
        {
            Context.UpdateCellState?.Invoke();
            return Context.CellState;
        }

        public void SetCellState(int cellIndex, int dataIndex, float x, float y, float scale)
        {
            Context.SetCellState(cellIndex, dataIndex, x, y, scale);
        }
    }
}
