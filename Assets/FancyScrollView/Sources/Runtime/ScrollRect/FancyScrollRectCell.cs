using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollRectCell<TItemData, TContext>
        : FancyScrollViewCell<TItemData, TContext> where TContext : class, IFancyScrollRectContext, new()
    {
        protected float CalculateViewportPosition(float position)
        {
            var cellInterval = Context.GetCellInterval();
            var viewportSize = Context.GetViewportSize();
            var fancyScrollViewportSize = 1f / Mathf.Max(cellInterval, 1e-2f);

            var scale = fancyScrollViewportSize / Mathf.Max(fancyScrollViewportSize - 1f, 1e-2f);
            var margin = viewportSize * scale * cellInterval;
            var scrollSize = viewportSize + margin;

            var start = 0.5f * scrollSize;
            var end = -start;

            return Mathf.Lerp(start, end, position);
        }
    }

    public abstract class FancyScrollRectCell<TItemData> : FancyScrollRectCell<TItemData, FancyScrollRectContext>
    {
        public sealed override void SetupContext(FancyScrollRectContext context) => base.SetupContext(context);
    }
}
