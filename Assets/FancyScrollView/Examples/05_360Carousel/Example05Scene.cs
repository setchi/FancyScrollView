using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class Example05Scene : MonoBehaviour
    {
        [SerializeField]
        Example05ScrollView scrollView;
        [SerializeField]
        Button prevCellButton;
        [SerializeField]
        Button nextCellButton;
        [SerializeField]
        Text selectedItemInfo;

        List<Example05CellDto> cellData;
        Example05ScrollViewContext context;

        void HandlePrevButton()
        {
            SelectCell(context.SelectedIndex - 1);
        }

        void HandleNextButton()
        {
            SelectCell(context.SelectedIndex + 1);
        }

        void SelectCell(int index)
        {
            if (index >= 0 && index < cellData.Count)
            {
                scrollView.UpdateSelection(index);
            }
        }

        void HandleSelectedIndexChanged(int index)
        {
            selectedItemInfo.text = String.Format("Selected item info: index {0}", index);
        }

        void Awake()
        {
            prevCellButton.onClick.AddListener(HandlePrevButton);
            nextCellButton.onClick.AddListener(HandleNextButton);
        }

        void Start()
        {
            cellData = Enumerable.Range(0, 20)
                .Select(i => new Example05CellDto { Message = "Cell " + i })
                .ToList();
            context = new Example05ScrollViewContext();
            context.OnSelectedIndexChanged = HandleSelectedIndexChanged;
            context.SelectedIndex = 0;

            scrollView.UpdateData(cellData, context);
            scrollView.UpdateSelection(context.SelectedIndex);
        }
    }
}
