/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example06
{
    class Example06 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;
        [SerializeField] Text selectedItemInfo = default;
        [SerializeField] Window[] windows = default;

        Window currentWindow;

        void Start()
        {
            scrollView.OnSelectionChanged(OnSelectionChanged);

            var items = Enumerable.Range(0, windows.Length)
                .Select(i => new ItemData($"Tab {i}"))
                .ToList();

            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }

        void OnSelectionChanged(int index, MovementDirection direction)
        {
            selectedItemInfo.text = $"Selected tab info: index {index}";

            if (currentWindow != null)
            {
                currentWindow.Out(direction);
                currentWindow = null;
            }

            if (index >= 0 && index < windows.Length)
            {
                currentWindow = windows[index];
                currentWindow.In(direction);
            }
        }
    }
}
