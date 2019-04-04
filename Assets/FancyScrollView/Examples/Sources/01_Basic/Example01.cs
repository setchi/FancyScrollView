using System.Linq;
using UnityEngine;

namespace FancyScrollView.Example01
{
    public class Example01 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            var items = Enumerable.Range(0, 20)
                .Select(i => new ItemData {Message = $"Cell {i}"})
                .ToArray();

            scrollView.UpdateData(items);
        }
    }
}
