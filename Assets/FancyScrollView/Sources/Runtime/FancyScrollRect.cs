using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    [RequireComponent(typeof(ScrollRect))]
    public abstract class FancyScrollRect<TItemData, TContext>
        : FancyScrollView<TItemData, TContext> where TContext : class, IFancyScrollRectContext, new()
    {
        [SerializeField] protected ScrollDirection scrollDirection = default;

        protected virtual float MaxScrollPosition => ItemsSource.Count - FancyScrollViewportSize;
        protected virtual float FancyScrollViewportSize => 1f / Mathf.Max(cellInterval, 1e-2f) - 1f;
        protected virtual float ViewportSize => scrollDirection == ScrollDirection.Horizontal
            ? (transform as RectTransform).rect.size.x
            : (transform as RectTransform).rect.size.y;
        protected virtual bool ScrollEnabled => MaxScrollPosition > 0f;

        ScrollRect scroller;
        protected ScrollRect Scroller => scroller ?? (scroller = GetComponent<ScrollRect>());

        protected virtual void Awake()
        {
            Context.GetViewportSize = () => ViewportSize;
            Context.GetFancyScrollViewportSize = () => FancyScrollViewportSize;
        }

        protected virtual void Start()
        {
            Scroller.onValueChanged.AddListener(p =>
                base.UpdatePosition(ScrollEnabled ? ToFancyScrollViewPosition(p) : 0f));
        }

        protected new void UpdatePosition(float position) => UpdatePosition(position, Alignment.Center);

        protected virtual void UpdatePosition(float position, Alignment alignment)
        {
            var normalizedPosition = ToScrollRectNormalizedPosition(position, alignment);
            switch(scrollDirection)
            {
                case ScrollDirection.Horizontal:
                    Scroller.horizontalNormalizedPosition = normalizedPosition;
                    break;

                case ScrollDirection.Vertical:
                    Scroller.verticalNormalizedPosition = normalizedPosition;
                    break;
            }
        }

        protected override void UpdateContents(IList<TItemData> items)
        {
            base.UpdateContents(items);
            AdjustContentSize();
        }

        protected virtual void AdjustContentSize()
        {
            var cellSize = ViewportSize / FancyScrollViewportSize;
            var contentSize = cellSize * ItemsSource.Count;

            switch (scrollDirection)
            {
                case ScrollDirection.Horizontal:
                    Scroller.content.sizeDelta = new Vector2(contentSize, Scroller.content.sizeDelta.y);
                    Scroller.horizontal = ScrollEnabled;
                    break;

                case ScrollDirection.Vertical:
                    Scroller.content.sizeDelta = new Vector2(Scroller.content.sizeDelta.x, contentSize);
                    Scroller.vertical = ScrollEnabled;
                    break;
            }
        }

        protected virtual float GetAlignment(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Head: return 0.0f;
                case Alignment.Center: return 0.5f;
                case Alignment.Tail: return 1.0f;
                default:
                    return GetAlignment(Alignment.Center);
            }
        }

        protected virtual float ToFancyScrollViewPosition(Vector2 scrollRectPosition)
        {
            return ToFancyScrollViewPosition(scrollDirection == ScrollDirection.Horizontal
                ? scrollRectPosition.x
                : scrollRectPosition.y);
        }

        protected virtual float ToFancyScrollViewPosition(float scrollRectNormalizedPosition)
        {
            return (1f - scrollRectNormalizedPosition) * MaxScrollPosition;
        }

        protected virtual float ToScrollRectNormalizedPosition(float position, Alignment alignment = Alignment.Center)
        {
            var offset = (ItemsSource.Count - 1 - MaxScrollPosition) * GetAlignment(alignment);
            return Mathf.Clamp01(1f - (position - offset) / MaxScrollPosition);
        }

        protected virtual void Update() => scrollOffset = cellInterval;
    }
}
