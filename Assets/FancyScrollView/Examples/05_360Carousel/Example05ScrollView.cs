using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example05ScrollView : FancyScrollView<Example05CellDto, Example05ScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;
        [SerializeField]
        float scrollToDuration = 0.4f;

        void Awake()
        {
            scrollPositionController.OnUpdatePosition(UpdatePosition);
            scrollPositionController.OnItemSelected(HandleItemSelected);
        }

        public void UpdateData(List<Example05CellDto> data, Example05ScrollViewContext context)
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

        void OnPressedCell(Example05ScrollViewCell cell)
        {
            UpdateSelection(cell.DataIndex);
        }
    }
}
