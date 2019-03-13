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
            scrollView.OnSelectedIndexChanged(OnSelectedIndexChanged);

            var cellData = Enumerable.Range(0, 20)
                .Select(i => new Example04CellData {Message = "Cell " + i})
                .ToArray();

            scrollView.UpdateData(cellData);
            scrollView.UpdateSelection(0);
        }

        void OnSelectedIndexChanged(int index)
        {
            selectedItemInfo.text = string.Format("Selected item info: index {0}", index);
        }
    }
}
