/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using EasingCore;

namespace FancyScrollView.Example08
{
    class Example08 : MonoBehaviour
    {
        [SerializeField] GridView gridView = default;
        [SerializeField] InputField paddingTopInputField = default;
        [SerializeField] InputField paddingBottomInputField = default;
        [SerializeField] InputField xSpacingInputField = default;
        [SerializeField] InputField ySpacingInputField = default;
        [SerializeField] InputField dataCountInputField = default;
        [SerializeField] InputField selectIndexInputField = default;
        [SerializeField] Dropdown alignmentDropdown = default;

        void Start()
        {
            gridView.OnCellClicked(index => selectIndexInputField.text = index.ToString());

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
            alignmentDropdown.value = (int)Alignment.Middle;

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
                gridView.ScrollTo(index, 0.4f, Ease.InOutQuint, (Alignment)alignmentDropdown.value);
            });
        }

        void GenerateCells(int dataCount)
        {
            var items = Enumerable.Range(0, dataCount)
                .Select(i => new ItemData(i))
                .ToArray();

            gridView.UpdateContents(items);
            SelectCell();
        }
    }
}
