using UnityEngine;

namespace FancyScrollView.Example08
{
    public class Row : FancyGridViewRow<ItemData, Context>
    {
        [SerializeField] Cell[] cells = default;

        protected override FancyScrollViewCell<ItemData, Context>[] InstantiateCells() => cells;

        protected override void UpdatePosition(float position, float viewportPosition)
        {
            var x = Mathf.Sin(position * Mathf.PI * 2) * 80;
            var y = viewportPosition;

            transform.localPosition = new Vector2(x, y);
        }
    }
}
