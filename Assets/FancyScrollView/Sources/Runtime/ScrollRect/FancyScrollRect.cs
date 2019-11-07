using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView
{
    [RequireComponent(typeof(Scroller))]
    public abstract class FancyScrollRect<TItemData, TContext>
        : FancyScrollView<TItemData, TContext> where TContext : class, IFancyScrollRectContext, new()
    {
        protected virtual float FancyScrollViewportSize => 1f / Mathf.Max(cellInterval, 1e-2f) - 1f;
        protected virtual float MaxScrollPosition => ItemsSource.Count - FancyScrollViewportSize;
        protected virtual bool ScrollEnabled => MaxScrollPosition > 0f;

        Scroller cachedScroller;
        protected Scroller Scroller => cachedScroller ?? (cachedScroller = GetComponent<Scroller>());

        protected virtual void Awake()
        {
            Context.GetCellInterval = () => cellInterval;
            Context.GetViewportSize = () => Scroller.ScrollDirection == ScrollDirection.Horizontal
                ? Scroller.Viewport.rect.size.x
                : Scroller.Viewport.rect.size.y;
        }

        protected virtual void Start()
        {
            scrollOffset = cellInterval;
            Scroller.OnValueChanged(OnScrollerValueChanged);
        }

        void OnScrollerValueChanged(float p)
        {
            base.UpdatePosition(ScrollEnabled ? ToFancyScrollViewPosition(p) : 0f);

            if (Scroller.Scrollbar)
            {
                if (p > ItemsSource.Count - 1)
                {
                    ShrinkScrollbar(p - (ItemsSource.Count - 1));
                }
                else if (p < 0f)
                {
                    ShrinkScrollbar(-p);
                }
            }
        }

        void ShrinkScrollbar(float offset)
        {
            var scale = 1f - ToFancyScrollViewPosition(offset) / FancyScrollViewportSize;
            UpdateScrollbarSize(FancyScrollViewportSize * scale / Mathf.Max(ItemsSource.Count, 1));
        }

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
                UpdateScrollbarSize(FancyScrollViewportSize / Mathf.Max(ItemsSource.Count, 1));
            }
        }

        protected new void UpdatePosition(float position)
        {
            UpdatePosition(position, Alignment.Center);
        }

        protected virtual void UpdatePosition(float position, Alignment alignment)
        {
            Scroller.Position = ToScrollerPosition(position, alignment);
        }

        public void ScrollTo(float position, float duration, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            Scroller.ScrollTo(ToScrollerPosition(position, alignment), duration, onComplete);
        }

        public void ScrollTo(float position, float duration, Ease easing, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            Scroller.ScrollTo(ToScrollerPosition(position, alignment), duration, easing, onComplete);
        }

        protected void UpdateScrollbarSize(float size)
        {
            Scroller.Scrollbar.size = ScrollEnabled ? Mathf.Clamp01(size) : 1f;
        }

        protected virtual float ToFancyScrollViewPosition(float scrollerPosition)
        {
            return scrollerPosition / Mathf.Max(ItemsSource.Count - 1, 1) * MaxScrollPosition;
        }

        protected virtual float ToScrollerPosition(float position)
        {
            return position / MaxScrollPosition * (ItemsSource.Count - 1);
        }

        protected virtual float ToScrollerPosition(float position, Alignment alignment = Alignment.Center)
        {
            var offset = (FancyScrollViewportSize - 1f) * GetAnchore(alignment);
            return ToScrollerPosition(Mathf.Clamp(position - offset, 0f, MaxScrollPosition));
        }

        protected virtual float GetAnchore(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Head: return 0.0f;
                case Alignment.Center: return 0.5f;
                case Alignment.Tail: return 1.0f;
                default: return GetAnchore(Alignment.Center);
            }
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            scrollOffset = cellInterval;

            Debug.Assert(!loop, "Loop is currently not supported in FancyScrollRect.");
            Debug.Assert(!Scroller.SnapEnabled, "Snap is currently not supported in FancyScrollRect.");
            Debug.Assert(Scroller.MovementType != MovementType.Unrestricted, "MovementType.Unrestricted is currently not supported in FancyScrollRect.");
#endif
        }
    }

    public abstract class FancyScrollRect<TItemData> : FancyScrollRect<TItemData, FancyScrollRectContext> { }
}
