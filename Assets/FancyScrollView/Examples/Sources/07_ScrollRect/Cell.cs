using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example07
{
    public class Cell : FancyScrollViewCell<ItemData, Context>
    {
        [SerializeField] Text message = default;
        [SerializeField] Image image = default;

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Message;

            var selected = Context.SelectedIndex == Index;
            image.color = selected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
        }

        public override void UpdatePosition(float position)
        {
            var viewportSize = Context.GetViewportSize();
            var offset = 0.5f * (viewportSize + viewportSize / (Context.GetVisibleCellCount() + 1f));
            var x = Mathf.Sin(position * Mathf.PI * 2) * 50;
            var y = Mathf.Lerp(offset, -offset, position);

            (transform as RectTransform).anchoredPosition = new Vector2(x, y);
        }
    }
}
