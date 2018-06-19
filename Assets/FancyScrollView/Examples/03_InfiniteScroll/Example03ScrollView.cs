﻿using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example03ScrollView : FancyScrollView<Example03CellDto, Example03ScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;

        void Awake()
        {
            scrollPositionController.OnUpdatePosition(UpdatePosition);
            SetContext(new Example03ScrollViewContext { OnPressedCell = OnPressedCell });
        }

        public void UpdateData(List<Example03CellDto> data)
        {
            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            RefreshCells();
            UpdateContents();
        }

        void OnPressedCell(Example03ScrollViewCell cell)
        {
            scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
            context.SelectedIndex = cell.DataIndex;
            UpdateContents();
        }
    }
}
