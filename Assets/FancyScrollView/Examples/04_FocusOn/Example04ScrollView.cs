using System;
using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example04ScrollView : FancyScrollView<Example04CellDto, Example04ScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;

        Action<int> onSelectedIndexChanged;

        void Awake()
        {
            scrollPositionController.OnUpdatePosition(UpdatePosition);
            scrollPositionController.OnItemSelected(HandleItemSelected);

            var context = new Example04ScrollViewContext();
            context.SelectedIndex = 0;
            context.OnPressedCell = OnPressedCell;
            context.OnSelectedIndexChanged = index =>
            {
                if (onSelectedIndexChanged != null)
                {
                    onSelectedIndexChanged(index);
                }
            };
            SetContext(context);
        }

        public void UpdateData(List<Example04CellDto> data)
        {
            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            UpdateContents();
        }

        public void UpdateSelection(int index)
        {
            if (index < 0 || index >= cellData.Count)
            {
                return;
            }

            scrollPositionController.ScrollTo(index, 0.4f);
            Context.SelectedIndex = index;
            UpdateContents();
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

        void HandleItemSelected(int selectedItemIndex)
        {
            Context.SelectedIndex = selectedItemIndex;
            UpdateContents();
        }

        void OnPressedCell(Example04ScrollViewCell cell)
        {
            UpdateSelection(cell.DataIndex);
        }
    }
}
