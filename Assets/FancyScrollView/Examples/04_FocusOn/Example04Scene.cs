using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class Example04Scene : MonoBehaviour
    {
        [SerializeField] Example04ScrollView scrollView;
        [SerializeField] Button prevCellButton;
        [SerializeField] Button nextCellButton;
        [SerializeField] Text selectedItemInfo;

        void Start()
        {
            prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
            nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
            scrollView.OnSelectionChanged(OnSelectionChanged);

            var items = Enumerable.Range(0, 20)
                .Select(i => new Example04ItemData {Message = $"Cell {i}"})
                .ToArray();

            scrollView.UpdateData(items);
            scrollView.UpdateSelection(0);
        }

        void OnSelectionChanged(int index)
        {
            selectedItemInfo.text = $"Selected item info: index {index}";
        }
    }
}
