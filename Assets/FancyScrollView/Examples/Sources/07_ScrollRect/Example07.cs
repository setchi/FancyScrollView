using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example07
{
    public class Example07 : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;
        [SerializeField] Dropdown alignmentDropdown = default;
        [SerializeField] InputField dataCountInputField = default;
        [SerializeField] InputField selectIndexInputField = default;

        void Start()
        {
            alignmentDropdown.AddOptions(Enum.GetNames(typeof(Alignment)).Select(x => new Dropdown.OptionData(x)).ToList());
            alignmentDropdown.onValueChanged.AddListener(_ => SelectCell());
            alignmentDropdown.value = (int)Alignment.Center;

            dataCountInputField.onValueChanged.AddListener(_ => GenerateItems());
            dataCountInputField.text = "20";

            selectIndexInputField.onValueChanged.AddListener(_ => SelectCell());
            selectIndexInputField.text = "10";
        }

        void SelectCell()
        {
            if (!int.TryParse(selectIndexInputField.text, out int index))
            {
                return;
            }

            if (index < 0 || index > scrollView.DataCount - 1)
            {
                selectIndexInputField.text = Mathf.Clamp(index, 0, scrollView.DataCount - 1).ToString();
                return;
            }

            scrollView.JumpTo(index, (Alignment)alignmentDropdown.value);
        }

        void GenerateItems()
        {
            if (!int.TryParse(dataCountInputField.text, out int dataCount))
            {
                return;
            }

            var items = Enumerable.Range(0, dataCount)
                .Select(i => new ItemData($"Cell {i}"))
                .ToArray();

            scrollView.UpdateData(items);
        }
    }
}
