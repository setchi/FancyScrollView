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
            var baseScale = 0.6f;
            position += Index % Context.ColumnCount * 0.3f;

            var xx = Mathf.Sin(position * Mathf.PI * 2) * 20;

            var x = baseScale + (Mathf.Sin(position * Mathf.PI * 3.5f) * 0.5f + 0.5f) * (1f - baseScale) * 1.9f;
            var y = baseScale + (Mathf.Sin(position * Mathf.PI * 3.5f) * 0.5f + 0.5f) * (1f - baseScale) * 1.9f;

            transform.localScale = new Vector2(x, y);
            message.transform.localScale = Vector2.one / new Vector2(x, y);

            var slide = position * 100;
            transform.localPosition = new Vector2(-130f + xx + slide + 100f * (Index % Context.ColumnCount - 1f), transform.localPosition.y);
        }
    }
}
