using System.Linq;
using UnityEngine;

namespace FancyScrollView
{
    public class Example01Scene : MonoBehaviour
    {
        [SerializeField]
        Example01ScrollView scrollView;

        void Start()
        {
            var cellData = Enumerable.Range(0, 20)
                .Select(i => new Example01CellDto { Message = "Cell " + i })
                .ToList();

            scrollView.UpdateData(cellData);
        }
    }
}
