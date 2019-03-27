using UnityEngine;
using System.Collections.Generic;

namespace FancyScrollView
{
    public class Example01ScrollView : FancyScrollView<Example01CellData>
    {
        [SerializeField] Scroller scroller;
        [SerializeField] GameObject cellPrefab;

        protected override GameObject CellPrefab => cellPrefab;

        void Start()
        {
            scroller.OnUpdatePosition(UpdatePosition);
        }

        public void UpdateData(IList<Example01CellData> cellData)
        {
            UpdateContents(cellData);
            scroller.SetDataCount(cellData.Count);
        }
    }
}
