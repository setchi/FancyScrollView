using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollViewExamples
{
    public class Example04Scene : MonoBehaviour
    {
        [SerializeField]
        Example04ScrollView scrollView;
        [SerializeField]
        Button prevCellButton;
        [SerializeField]
        Button nextCellButton;

        List<Example04CellDto> cellData;
        Example04ScrollViewContext context;

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

        void Awake()
        {
            prevCellButton.onClick.AddListener(HandlePrevButton);
            nextCellButton.onClick.AddListener(HandleNextButton);
        }

        void Start()
        {
            cellData = Enumerable.Range(0, 20)
                .Select(i => new Example04CellDto { Message = "Cell " + i })
                .ToList();
            context = new Example04ScrollViewContext(){ SelectedIndex = 0 };

            scrollView.UpdateData(cellData, context);
            scrollView.UpdateSelection(context.SelectedIndex);
        }
    }
}
