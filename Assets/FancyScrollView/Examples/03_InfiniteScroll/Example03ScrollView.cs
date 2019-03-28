using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example03ScrollView : FancyScrollView<Example03ItemData, Example03ScrollViewContext>
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
            scroller.OnValueChanged(UpdatePosition);
        }

        public void UpdateData(IList<Example03ItemData> items)
        {
            UpdateContents(items);
            scroller.SetTotalCount(items.Count);
        }

        void OnCellClicked(int index)
        {
            scroller.ScrollTo(index, 0.4f);
            Context.SelectedIndex = index;
            Refresh();
        }
    }
}
