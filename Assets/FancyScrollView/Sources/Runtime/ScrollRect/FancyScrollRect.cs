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
            Context.GetCellInterval = () => cellInterval;
            Context.GetViewportSize = () => Scroller.ScrollDirection == ScrollDirection.Horizontal
                ? Scroller.Viewport.rect.size.x
                : Scroller.Viewport.rect.size.y;
        }

        protected virtual void Start()
        {
            Scroller.SnapEnabled = false;
            scrollOffset = cellInterval;

            Scroller.OnValueChanged(p =>
                base.UpdatePosition(ScrollEnabled ? ToFancyScrollViewPosition(p) : 0f));
        }

        protected new void UpdatePosition(float position) => UpdatePosition(position, Alignment.Center);

        protected virtual void UpdatePosition(float position, Alignment alignment) =>
            Scroller.Position = ToScrollerPosition(position, alignment);

        protected override void UpdateContents(IList<TItemData> items)
        {
            base.UpdateContents(items);

            Scroller.SetTotalCount(items.Count);
            Scroller.Draggable = ScrollEnabled;
            Scroller.ScrollSensitivity = ToScrollerPosition(FancyScrollViewportSize);
            Scroller.Position = ToScrollerPosition(currentPosition);

            if (Scroller.Scrollbar)
            {
                Scroller.Scrollbar.gameObject.SetActive(ScrollEnabled);
                Scroller.Scrollbar.size = ScrollEnabled
                    ? Mathf.Clamp01(FancyScrollViewportSize / Mathf.Max(ItemsSource.Count, 1-2f))
                    : 1f;
            }
        }

        protected virtual float ToFancyScrollViewPosition(float scrollerPosition) =>
            scrollerPosition / Mathf.Max(ItemsSource.Count - 1, 1) * MaxScrollPosition;

        protected virtual float ToScrollerPosition(float position, Alignment alignment = Alignment.Center)
        {
            var offset = (FancyScrollViewportSize - 1f) * GetAnchore(alignment);
            return ToScrollerPosition(Mathf.Clamp(position - offset, 0f, MaxScrollPosition));
        }

        protected virtual float ToScrollerPosition(float position) =>
            position / MaxScrollPosition * (ItemsSource.Count - 1f);

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
            Scroller.SnapEnabled = false;
            scrollOffset = cellInterval;
#endif
        }
    }

    public abstract class FancyScrollRect<TItemData> : FancyScrollRect<TItemData, FancyScrollRectContext> { }
}
