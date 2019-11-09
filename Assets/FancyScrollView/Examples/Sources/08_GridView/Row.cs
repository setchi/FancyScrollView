using UnityEngine;

namespace FancyScrollView.Example08
{
    public class Row : FancyGridViewRow<ItemData, Context>
    {
        [SerializeField] Cell[] cells = default;

        protected override FancyScrollViewCell<ItemData, Context>[] InstantiateCells() => cells;

        public override void UpdatePosition(float position)
        {
            base.UpdatePosition(position);

            var x = Mathf.Sin(position * Mathf.PI * 2) * 80;
            var y = CalculateViewportPosition(position);

            transform.localPosition = new Vector2(x, y);
        }
    }
}
