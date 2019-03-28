using System.Linq;
using UnityEngine;

namespace FancyScrollView
{
    public class Example02Scene : MonoBehaviour
    {
        [SerializeField] Example02ScrollView scrollView;

        void Start()
        {
            var items = Enumerable.Range(0, 20)
                .Select(i => new Example02ItemData {Message = $"Cell {i}"})
                .ToArray();

            scrollView.UpdateData(items);
        }
    }
}
