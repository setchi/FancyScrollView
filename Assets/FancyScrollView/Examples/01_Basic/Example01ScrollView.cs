using UnityEngine;
using System.Collections.Generic;

namespace ArtisticScrollViewExamples
{
    public class Example01ScrollView : ArtisticScrollView<Example01CellDto>
    {

        [SerializeField]
        ScrollPositionController scrollPositionController;

        void Awake()
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
