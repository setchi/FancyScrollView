using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example06
{
    public class Example06 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;
        [SerializeField] Text selectedItemInfo = default;
        [SerializeField] private Window[] windows = default;

        private Window _currentWindow;
        
        void Start()
        {
            scrollView.OnSelectionChanged(OnSelectionChanged);

            var items = Enumerable.Range(0, windows.Length)
                .Select(i => new ItemData($"Tab {i}"))
                .ToList();

            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }

        void OnSelectionChanged(int index)
        {
            selectedItemInfo.text = $"Selected tab info: index {index}";
            var direction = scrollView.GetMovementDirection();
            if (_currentWindow != null)
            {
                _currentWindow.HideWindow(direction);
                _currentWindow = null;
            }

            if (index >= 0 && index < windows.Length)
            {
                _currentWindow = windows[index];
                _currentWindow.OpenWindow(direction);
            }
        }
    }
}