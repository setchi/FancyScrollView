using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollViewExamples
{
    public class Example04ScrollView : FancyScrollView<Example04CellDto, Example04ScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;
        [SerializeField]
        float scrollToDuration = 0.4f;

        protected override void Awake()
        {
            scrollPositionController.OnUpdatePosition(UpdatePosition);
            scrollPositionController.OnItemSelected(HandleItemSelected);
            base.Awake();
        }

        public void UpdateData(List<Example04CellDto> data, Example04ScrollViewContext context)
        {
            context.OnPressedCell = OnPressedCell;
            SetContext(context);

            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            UpdateContents();
        }

        public void UpdateSelection(int selectedCellIndex)
        {
            scrollPositionController.ScrollTo(selectedCellIndex, scrollToDuration);
            context.SelectedIndex = selectedCellIndex;
            UpdateContents();
        }

        void HandleItemSelected(int selectedItemIndex)
        {
            context.SelectedIndex = selectedItemIndex;
            UpdateContents();
        }

        void OnPressedCell(Example04ScrollViewCell cell)
        {
            UpdateSelection(cell.DataIndex);
        }
    }
}
