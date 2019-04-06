using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example03
{
    public class Example03 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = Enumerable.Range(0, 20)
                .Select(i => new ItemData {Message = $"Cell {i}"})
                .ToArray();

            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }
    }
}
