using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example02ScrollView : FancyScrollView<Example02CellData, Example02ScrollViewContext>
    {
        [SerializeField] ScrollPositionController scrollPositionController;
        [SerializeField] GameObject cellPrefab;

        protected override GameObject CellPrefab => cellPrefab;

        void Awake() => Context.OnCellClicked = OnCellClicked;

        void Start() => scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));

        public void UpdateData(IList<Example02CellData> cellData)
        {
            UpdateContents(cellData);
            scrollPositionController.SetDataCount(cellData.Count);
        }

        void OnCellClicked(int index)
        {
            scrollPositionController.ScrollTo(index, 0.4f);
            Context.SelectedIndex = index;
            UpdateContents();
        }
    }
}
