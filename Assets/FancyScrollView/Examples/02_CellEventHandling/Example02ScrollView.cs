using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example02ScrollView : FancyScrollView<Example02CellData, Example02ScrollViewContext>
    {
        [SerializeField] Scroller scroller;
        [SerializeField] GameObject cellPrefab;

        protected override GameObject CellPrefab => cellPrefab;

        void Awake()
        {
            Context.OnCellClicked = OnCellClicked;
        }

        void Start()
        {
            scroller.OnUpdatePosition(UpdatePosition);
        }

        public void UpdateData(IList<Example02CellData> cellData)
        {
            UpdateContents(cellData);
            scroller.SetDataCount(cellData.Count);
        }

        void OnCellClicked(int index)
        {
            scroller.ScrollTo(index, 0.4f);
            Context.SelectedIndex = index;
            Refresh();
        }
    }
}
