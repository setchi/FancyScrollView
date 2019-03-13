using System;
using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public class Example04ScrollView : FancyScrollView<Example04CellData, Example04ScrollViewContext>
    {
        [SerializeField] ScrollPositionController scrollPositionController;

        Action<int> onSelectedIndexChanged;

        void Awake()
        {
            SetContext(new Example04ScrollViewContext
            {
                OnPressedCell = OnPressedCell,
                OnSelectedIndexChanged = index =>
                {
                    if (onSelectedIndexChanged != null)
                    {
                        onSelectedIndexChanged(index);
                    }
                }
            });
        }

        void Start()
        {
            scrollPositionController.OnUpdatePosition(p => UpdatePosition(p));
            scrollPositionController.OnItemSelected(OnItemSelected);
        }

        public void UpdateData(IList<Example04CellData> cellData)
        {
            UpdateContents(cellData);
            scrollPositionController.SetDataCount(cellData.Count);
        }

        public void UpdateSelection(int index)
        {
            if (index < 0 || index >= CellData.Count)
            {
                return;
            }

            scrollPositionController.ScrollTo(index, 0.4f);
            Context.SelectedIndex = index;
            UpdateContents();
        }

        public void OnSelectedIndexChanged(Action<int> onSelectedIndexChanged)
        {
            this.onSelectedIndexChanged = onSelectedIndexChanged;
        }

        public void SelectNextCell()
        {
            UpdateSelection(Context.SelectedIndex + 1);
        }

        public void SelectPrevCell()
        {
            UpdateSelection(Context.SelectedIndex - 1);
        }

        void OnItemSelected(int index)
        {
            Context.SelectedIndex = index;
            UpdateContents();
        }

        void OnPressedCell(int index)
        {
            UpdateSelection(index);
        }
    }
}
