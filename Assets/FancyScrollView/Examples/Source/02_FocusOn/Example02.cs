using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example02
{
    public class Example02 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView;
        [SerializeField] Button prevCellButton;
        [SerializeField] Button nextCellButton;
        [SerializeField] Text selectedItemInfo;

        void Start()
        {
            prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
            nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
            scrollView.OnSelectionChanged(OnSelectionChanged);

            var items = Enumerable.Range(0, 20)
                .Select(i => new ItemData {Message = $"Cell {i}"})
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
