using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollRectCell<TItemData, TContext>
        : FancyScrollViewCell<TItemData, TContext> where TContext : class, IFancyScrollRectContext, new()
    {
        public override void UpdatePosition(float position)
        {
            var viewportSize = Context.GetViewportSize();
            var scrollSize = viewportSize + viewportSize / (Context.GetFancyScrollViewportSize() + 1f);

            var start = 0.5f * scrollSize;
            var end = -start;

            UpdatePosition(position, Mathf.Lerp(start, end, position));
        }

        protected virtual void UpdatePosition(float position, float viewportPosition) { }
    }
}
