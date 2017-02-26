using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollViewExamples
{
    public class Example03ScrollView : FancyScrollView<Example03CellDto, Example03ScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;

        void Awake()
        {
            scrollPositionController.OnUpdatePosition(UpdatePosition);
            SetContext(new Example03ScrollViewContext { OnPressedCell = OnPressedCell });
            base.Awake();
        }

        public void UpdateData(List<Example03CellDto> data)
        {
            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            UpdateContents();
        }

        void OnPressedCell(Example03ScrollViewCell cell)
        {
            scrollPositionController.SnapTo(cell.DataIndex);
            context.SelectedIndex = cell.DataIndex;
            UpdateContents();
        }
    }
}
