﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example08
{
    public class Example08 : MonoBehaviour
    {
        [SerializeField] FancyGridView gridView = default;
        [SerializeField] InputField paddingTopInputField = default;
        [SerializeField] InputField paddingBottomInputField = default;
        [SerializeField] InputField xSpacingInputField = default;
        [SerializeField] InputField ySpacingInputField = default;
        [SerializeField] Dropdown alignmentDropdown = default;
        [SerializeField] InputField dataCountInputField = default;
        [SerializeField] InputField selectIndexInputField = default;

        void Start()
        {
            paddingTopInputField.onValueChanged.AddListener(_ =>
                TryParseValue(paddingTopInputField, 0, 999, value => gridView.PaddingTop = value));
            paddingTopInputField.text = gridView.PaddingTop.ToString();

            paddingBottomInputField.onValueChanged.AddListener(_ =>
                TryParseValue(paddingBottomInputField, 0, 999, value => gridView.PaddingBottom = value));
            paddingBottomInputField.text = gridView.PaddingBottom.ToString();

            xSpacingInputField.onValueChanged.AddListener(_ =>
                TryParseValue(xSpacingInputField, 0, 99, value => gridView.SpacingX = value));
            xSpacingInputField.text = gridView.SpacingX.ToString();

            ySpacingInputField.onValueChanged.AddListener(_ =>
                TryParseValue(ySpacingInputField, 0, 99, value => gridView.SpacingY = value));
            ySpacingInputField.text = gridView.SpacingY.ToString();

            alignmentDropdown.AddOptions(Enum.GetNames(typeof(Alignment)).Select(x => new Dropdown.OptionData(x)).ToList());
            alignmentDropdown.onValueChanged.AddListener(_ => SelectCell());
            alignmentDropdown.value = (int)Alignment.Center;

            selectIndexInputField.onValueChanged.AddListener(_ => SelectCell());
            selectIndexInputField.text = "50";

            dataCountInputField.onValueChanged.AddListener(_ =>
                TryParseValue(dataCountInputField, 1, 99999, GenerateCells));
            dataCountInputField.text = "100";

            gridView.JumpTo(50);
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
            if (gridView.DataCount == 0)
            {
                return;
            }

            TryParseValue(selectIndexInputField, 0, gridView.DataCount - 1, index =>
            {
                gridView.UpdateSelection(index);
                gridView.ScrollTo(index, 0.5f, (Alignment)alignmentDropdown.value);
            });
        }

        void GenerateCells(int dataCount)
        {
            var items = Enumerable.Range(0, dataCount)
                .Select(i => new ItemData(i))
                .ToArray();

            gridView.UpdateData(items);
            SelectCell();
        }
    }
}
