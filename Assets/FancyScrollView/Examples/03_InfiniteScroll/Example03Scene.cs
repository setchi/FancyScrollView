using System.Linq;
using UnityEngine;

namespace FancyScrollView
{
    public class Example03Scene : MonoBehaviour
    {
        [SerializeField] Example03ScrollView scrollView;

        void Start()
        {
            var items = Enumerable.Range(0, 20)
                .Select(i => new Example03ItemData {Message = $"Cell {i}"})
                .ToArray();

            scrollView.UpdateData(items);
        }
    }
}
