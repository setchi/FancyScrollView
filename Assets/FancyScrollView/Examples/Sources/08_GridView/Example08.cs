using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example08
{
    public class Example08 : MonoBehaviour
    {
        [SerializeField] FancyGridView gridView = default;
        [SerializeField] Dropdown alignmentDropdown = default;
        [SerializeField] InputField dataCountInputField = default;
        [SerializeField] InputField selectIndexInputField = default;

        void Start()
        {
            alignmentDropdown.AddOptions(Enum.GetNames(typeof(Alignment)).Select(x => new Dropdown.OptionData(x)).ToList());
            alignmentDropdown.onValueChanged.AddListener(_ => SelectCell());
            alignmentDropdown.value = (int)Alignment.Center;

            selectIndexInputField.onValueChanged.AddListener(_ => SelectCell());
            selectIndexInputField.text = "50";

            dataCountInputField.onValueChanged.AddListener(_ => GenerateItems());
            dataCountInputField.text = "100";

            gridView.UpdateSelection(50);
            gridView.JumpTo(50);
        }

        void SelectCell()
        {
            if (gridView.DataCount == 0 ||
                !int.TryParse(selectIndexInputField.text, out int index))
            {
                return;
            }

            if (index < 0 || index > gridView.DataCount - 1)
            {
                selectIndexInputField.text = Mathf.Clamp(index, 0, gridView.DataCount - 1).ToString();
                return;
            }

            gridView.UpdateSelection(index);
            gridView.ScrollTo(index, 0.6f, (Alignment)alignmentDropdown.value);
        }

        void GenerateItems()
        {
            if (!int.TryParse(dataCountInputField.text, out int dataCount))
            {
                return;
            }

            if (dataCount < 0)
            {
                dataCountInputField.text = "1";
                return;
            }

            var items = Enumerable.Range(0, dataCount)
                .Select(i => new ItemData(i))
                .ToArray();

            gridView.UpdateData(items);
            SelectCell();
        }
    }
}
