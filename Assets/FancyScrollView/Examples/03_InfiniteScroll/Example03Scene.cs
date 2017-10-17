using System.Linq;
using UnityEngine;

namespace FancyScrollView
{
    public class Example03Scene : MonoBehaviour
    {
        [SerializeField]
        Example03ScrollView scrollView;

        void Start()
        {
            var cellData = Enumerable.Range(0, 20)
                .Select(i => new Example03CellDto { Message = "Cell " + i })
                .ToList();

            scrollView.UpdateData(cellData);
        }
    }
}
