using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    [RequireComponent(typeof(Scroller))]
    public abstract class FancyScrollRect<TItemData, TContext>
        : FancyScrollView<TItemData, TContext> where TContext : class, IFancyScrollRectContext, new()
    {
        protected virtual float FancyScrollViewportSize => 1f / Mathf.Max(cellInterval, 1e-2f) - 1f;
        protected virtual float MaxScrollPosition => ItemsSource.Count - FancyScrollViewportSize;
        protected virtual bool ScrollEnabled => MaxScrollPosition > 0f;

        Scroller scroller;
        protected Scroller Scroller => scroller ?? (scroller = GetComponent<Scroller>());

        protected virtual void Awake()
        {
            Context.GetFancyScrollViewportSize = () => FancyScrollViewportSize;
            Context.GetViewportSize = () => Scroller.ScrollDirection == ScrollDirection.Horizontal
                ? Scroller.Viewport.rect.size.x
                : Scroller.Viewport.rect.size.y;
        }

        protected virtual void Start()
        {
            Scroller.Snap.Enable = false;
            scrollOffset = cellInterval;

            Scroller.OnValueChanged(p =>
                base.UpdatePosition(ScrollEnabled ? ToFancyScrollViewPosition(p) : 0f));
        }

        protected new void UpdatePosition(float position) => UpdatePosition(position, Alignment.Center);

        protected virtual void UpdatePosition(float position, Alignment alignment) =>
            scroller.UpdatePosition(ToScrollerPosition(position, alignment));

        protected override void UpdateContents(IList<TItemData> items)
        {
            base.UpdateContents(items);

            scroller.SetTotalCount(items.Count);
            scroller.Draggable = ScrollEnabled;
            Scroller.ScrollSensitivity = FancyScrollViewportSize * ((ItemsSource.Count - 1f) / MaxScrollPosition);
            scroller.UpdatePosition(currentPosition / MaxScrollPosition * (ItemsSource.Count - 1f));

            if (scroller.Scrollbar)
            {
                scroller.Scrollbar.gameObject.SetActive(ScrollEnabled);
                scroller.Scrollbar.size = ScrollEnabled
                    ? Mathf.Clamp01(FancyScrollViewportSize / Mathf.Max(ItemsSource.Count, 1-2f))
                    : 1f;
            }
        }

        protected virtual float ToFancyScrollViewPosition(float scrollerPosition)
        {
            scrollerPosition /= Mathf.Max(ItemsSource.Count - 1f, 1e-3f);
            return scrollerPosition * MaxScrollPosition;
        }

        protected virtual float ToScrollerPosition(float position, Alignment alignment = Alignment.Center)
        {
            var offset = (ItemsSource.Count - 1 - MaxScrollPosition) * GetAnchore(alignment);
            return Mathf.Clamp01((position - offset) / MaxScrollPosition) * (ItemsSource.Count - 1f);
        }

        protected virtual float GetAnchore(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Head: return 0.0f;
                case Alignment.Center: return 0.5f;
                case Alignment.Tail: return 1.0f;
                default:
                    return GetAnchore(Alignment.Center);
            }
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            Scroller.Snap.Enable = false;
            scrollOffset = cellInterval;
#endif
        }
    }

    public abstract class FancyScrollRect<TItemData> : FancyScrollRect<TItemData, FancyScrollRectContext> { }
}
