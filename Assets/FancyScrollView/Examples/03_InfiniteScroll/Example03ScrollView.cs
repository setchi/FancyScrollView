using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example03ScrollView : FancyScrollView<Example03CellData, Example03ScrollViewContext>
    {
        [SerializeField] ScrollPositionController scrollPositionController;
        [SerializeField] GameObject cellPrefab;

        protected override GameObject CellPrefab => cellPrefab;

        void Awake() => SetContext(new Example03ScrollViewContext {OnCellClicked = OnCellClicked});

        void Start() => scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));

        public void UpdateData(IList<Example03CellData> cellData)
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
