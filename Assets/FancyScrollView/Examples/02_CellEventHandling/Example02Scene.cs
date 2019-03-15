using System.Linq;
using UnityEngine;

namespace FancyScrollView
{
    public class Example02Scene : MonoBehaviour
    {
        [SerializeField] Example02ScrollView scrollView;

        void Start()
        {
            var cellData = Enumerable.Range(0, 20)
                .Select(i => new Example02CellData {Message = $"Cell {i}"})
                .ToArray();

            scrollView.UpdateData(cellData);
        }
    }
}
