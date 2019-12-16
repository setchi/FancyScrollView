/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example08
{
    public class Cell : FancyScrollViewCell<ItemData, Context>
    {
        [SerializeField] Text message = default;
        [SerializeField] Image image = default;

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Index.ToString();

            var selected = Context.SelectedItemIndex == Index;
            image.color = selected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
        }

        public override void UpdatePosition(float position)
        {
            var cellSize = Context.GetCellSize();
            var spacing = Context.GetColumnSpacing();
            var columnCount = Context.GetColumnCount();
            var count = Index % columnCount;

            var x = (cellSize + spacing) * (count - (columnCount - 1) * 0.5f);
            var y = transform.localPosition.y;
            var wave = Mathf.Sin(position * Mathf.PI * 2) * 65;

            transform.localPosition = new Vector2(x + wave, y);
        }
    }
}
