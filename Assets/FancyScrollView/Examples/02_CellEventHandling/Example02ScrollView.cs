using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example02ScrollView : FancyScrollView<Example02CellData, Example02ScrollViewContext>
    {
        [SerializeField] ScrollPositionController scrollPositionController;

        void Awake()
        {
            SetContext(new Example02ScrollViewContext {OnPressedCell = OnPressedCell});
        }

        void Start()
        {
            scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));
        }

        public void UpdateData(IList<Example02CellData> cellData)
        {
            UpdateContents(cellData);
            scrollPositionController.SetDataCount(cellData.Count);
        }

        void OnPressedCell(int index)
        {
            scrollPositionController.ScrollTo(index, 0.4f);
            Context.SelectedIndex = index;
            UpdateContents();
        }
    }
}
