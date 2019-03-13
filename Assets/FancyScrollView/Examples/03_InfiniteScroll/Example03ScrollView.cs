using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example03ScrollView : FancyScrollView<Example03CellDto, Example03ScrollViewContext>
    {
        [SerializeField] ScrollPositionController scrollPositionController;

        void Awake()
        {
            scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));
            SetContext(new Example03ScrollViewContext {OnPressedCell = OnPressedCell});
        }

        public void UpdateData(IList<Example03CellDto> data)
        {
            UpdateContents(data);
            scrollPositionController.SetDataCount(CellData.Count);
        }

        void OnPressedCell(Example03ScrollViewCell cell)
        {
            scrollPositionController.ScrollTo(cell.DataIndex, 0.4f);
            Context.SelectedIndex = cell.DataIndex;
            UpdateContents();
        }
    }
}
