using UnityEngine;
using System.Collections.Generic;

namespace FancyScrollView
{
    public class Example01ScrollView : FancyScrollView<Example01CellData>
    {
        [SerializeField] ScrollPositionController scrollPositionController;
        [SerializeField] GameObject cellPrefab;

        protected override GameObject CellPrefab => cellPrefab;

        void Start() => scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));

        public void UpdateData(IList<Example01CellData> cellData)
        {
            UpdateContents(cellData);
            scrollPositionController.SetDataCount(cellData.Count);
        }
    }
}
