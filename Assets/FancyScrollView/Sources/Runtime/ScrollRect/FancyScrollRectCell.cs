using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollRectCell<TItemData, TContext> : FancyScrollViewCell<TItemData, TContext>
        where TContext : class, IFancyScrollRectContext, new()
    {
        public override void UpdatePosition(float position)
        {
            var (scrollSize, reuseMargin) = Context.CalculateScrollSize();

            var unclampedPosition = (Mathf.Lerp(0f, scrollSize, position) - reuseMargin) / (scrollSize - reuseMargin * 2f);

            var start = 0.5f * scrollSize;
            var end = -start;

            UpdatePosition(unclampedPosition, Mathf.Lerp(start, end, position));
        }

        protected virtual void UpdatePosition(float position, float viewportPosition) { }
    }

    public abstract class FancyScrollRectCell<TItemData> : FancyScrollRectCell<TItemData, FancyScrollRectContext>
    {
        public sealed override void SetupContext(FancyScrollRectContext context) => base.SetupContext(context);
    }
}
