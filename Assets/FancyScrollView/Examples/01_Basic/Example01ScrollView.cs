using UnityEngine;
using System.Collections.Generic;

namespace FancyScrollViewExamples
{
    public class Example01ScrollView : FancyScrollView<Example01CellDto>
    {
        [SerializeField]
        ScrollPositionController scrollPositionController;

        protected override void Awake()
        {
            base.Awake();
            scrollPositionController.OnUpdatePosition(UpdatePosition);
        }

        public void UpdateData(List<Example01CellDto> data)
        {
            cellData = data;
            scrollPositionController.SetDataCount(cellData.Count);
            UpdateContents();
        }
    }
}
