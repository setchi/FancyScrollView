using System.Collections.Generic;
using UnityEngine;

namespace ArtisticScrollViewExamples
{
    public class Example02ScrollView : FancyScrollView<Example02CellDto, Example02ScrollViewContext>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;

        void Awake()
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
            scrollPositionController.SnapTo(cell.DataIndex);
            context.SelectedIndex = cell.DataIndex;
            UpdateContents();
        }
    }
}
