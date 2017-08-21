using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollViewExamples
{
    public class Example02ScrollView : FancyScrollView<Example02CellDto, Example02ScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;

        protected override void Awake()
        {
            scrollPositionController.OnUpdatePosition(UpdatePosition);
            SetContext(new Example02ScrollViewContext { OnPressedCell = OnPressedCell });
            base.Awake();
        }

        public void UpdateData(List<Example02CellDto> data)
        {
            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            UpdateContents();
        }

        void OnPressedCell(Example02ScrollViewCell cell)
        {
            scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
            context.SelectedIndex = cell.DataIndex;
            UpdateContents();
        }
    }
}
