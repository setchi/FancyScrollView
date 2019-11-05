using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollRectCell<TItemData, TContext>
        : FancyScrollViewCell<TItemData, TContext> where TContext : class, IFancyScrollRectContext, new()
    {
        public override void UpdatePosition(float position)
        {
            var viewportSize = Context.GetViewportSize();
            var scrollSize = viewportSize + viewportSize * Context.GetCellInterval();

            var start = 0.5f * scrollSize;
            var end = -start;

            UpdatePosition(position, Mathf.Lerp(start, end, position));
        }

        protected virtual void UpdatePosition(float position, float viewportPosition) { }
    }

    public abstract class FancyScrollRectCell<TItemData> : FancyScrollRectCell<TItemData, FancyScrollRectContext>
    {
        public sealed override void SetupContext(FancyScrollRectContext context) => base.SetupContext(context);
    }
}
