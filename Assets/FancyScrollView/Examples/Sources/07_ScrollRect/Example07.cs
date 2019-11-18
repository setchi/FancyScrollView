using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using EasingCore;

namespace FancyScrollView.Example07
{
    public class Example07 : MonoBehaviour
    {
        [SerializeField] FancyScrollRect scrollView = default;
        [SerializeField] InputField paddingTopInputField = default;
        [SerializeField] InputField paddingBottomInputField = default;
        [SerializeField] InputField spacingInputField = default;
        [SerializeField] InputField dataCountInputField = default;
        [SerializeField] InputField selectIndexInputField = default;
        [SerializeField] Dropdown alignmentDropdown = default;
        [SerializeField] Dropdown easingDropdown = default;

        void Start()
        {
            paddingTopInputField.onValueChanged.AddListener(_ =>
                TryParseValue(paddingTopInputField, 0, 999, value => scrollView.PaddingTop = value));
            paddingTopInputField.text = scrollView.PaddingTop.ToString();

            paddingBottomInputField.onValueChanged.AddListener(_ =>
                TryParseValue(paddingBottomInputField, 0, 999, value => scrollView.PaddingBottom = value));
            paddingBottomInputField.text = scrollView.PaddingBottom.ToString();

            spacingInputField.onValueChanged.AddListener(_ =>
                TryParseValue(spacingInputField, 0, 100, value => scrollView.Spacing = value));
            spacingInputField.text = scrollView.Spacing.ToString();

            alignmentDropdown.AddOptions(Enum.GetNames(typeof(Alignment)).Select(x => new Dropdown.OptionData(x)).ToList());
            alignmentDropdown.onValueChanged.AddListener(_ => SelectCell());
            alignmentDropdown.value = (int)Alignment.Center;

            easingDropdown.AddOptions(Enum.GetNames(typeof(Ease)).Select(x => new Dropdown.OptionData(x)).ToList());
            easingDropdown.onValueChanged.AddListener(_ => SelectCell());
            easingDropdown.value = (int)Ease.InOutQuint;

            selectIndexInputField.onValueChanged.AddListener(_ => SelectCell());
            selectIndexInputField.text = "10";

            dataCountInputField.onValueChanged.AddListener(_ =>
                TryParseValue(dataCountInputField, 1, 99999, GenerateCells));
            dataCountInputField.text = "20";

            scrollView.JumpTo(10);
        }

        void TryParseValue(InputField inputField, int min, int max, Action<int> success)
        {
            if (!int.TryParse(inputField.text, out int value))
            {
                return;
            }

            if (value < min || value > max)
            {
                inputField.text = Mathf.Clamp(value, min, max).ToString();
                return;
            }

            success(value);
        }

        void SelectCell()
        {
            if (scrollView.DataCount == 0)
            {
                return;
            }

            TryParseValue(selectIndexInputField, 0, scrollView.DataCount - 1, index =>
                scrollView.ScrollTo(index, 0.4f, (Ease)easingDropdown.value, (Alignment)alignmentDropdown.value));
        }

        void GenerateCells(int dataCount)
        {
            var items = Enumerable.Range(0, dataCount)
                .Select(i => new ItemData($"Cell {i}"))
                .ToArray();

            scrollView.UpdateData(items);
            SelectCell();
        }
    }
}
