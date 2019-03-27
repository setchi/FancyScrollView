using System;
using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example04ScrollView : FancyScrollView<Example04CellData, Example04ScrollViewContext>
    {
        [SerializeField] Scroller scroller;
        [SerializeField] GameObject cellPrefab;

        Action<int> onSelectedIndexChanged;

        protected override GameObject CellPrefab => cellPrefab;

        void Awake()
        {
            Context.OnCellClicked = UpdateSelection;
        }

        void Start()
        {
            scroller.OnUpdatePosition(UpdatePosition);
            scroller.OnSelectedIndexChanged(UpdateSelection);
        }

        public void UpdateData(IList<Example04CellData> cellData)
        {
            UpdateContents(cellData);
            scroller.SetDataCount(cellData.Count);
        }

        public void OnSelectedIndexChanged(Action<int> callback)
        {
            onSelectedIndexChanged = callback;
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
            if (index < 0 || index >= CellData.Count || index == Context.SelectedIndex)
            {
                return;
            }

            Context.SelectedIndex = index;
            Refresh();

            scroller.ScrollTo(index, 0.4f);
            onSelectedIndexChanged?.Invoke(index);
        }
    }
}
