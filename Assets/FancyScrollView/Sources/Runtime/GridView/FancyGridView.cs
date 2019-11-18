using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView
{
    public abstract class FancyGridView<TItemData, TContext> : FancyScrollRect<TItemData[], TContext>
        where TContext : class, IFancyScrollRectContext, IFancyGridViewContext, new()
    {
        [SerializeField] protected float columnSpacing = 0f;

        protected GameObject cachedRowPrefab;
        protected sealed override GameObject CellPrefab => cachedRowPrefab ?? (cachedRowPrefab = SetupRowTemplate());

        protected abstract int ColumnCount { get; }

        protected abstract FancyScrollViewCell<TItemData, TContext> CellTemplate { get; }

        protected abstract FancyGridViewRow<TItemData, TContext> RowTemplate { get; }

        public int DataCount { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            Debug.Assert(RowTemplate != null);
            Debug.Assert(CellTemplate != null);
            Debug.Assert(ColumnCount > 0);

            Context.CellTemplate = CellTemplate.gameObject;
            Context.ScrollDirection = Scroller.ScrollDirection;
            Context.GetColumnCount = () => ColumnCount;
            Context.GetColumnSpacing = () => columnSpacing;
        }

        protected virtual GameObject SetupRowTemplate()
        {
            var cell = CellTemplate.GetComponent<RectTransform>();
            var row = RowTemplate.GetComponent<RectTransform>();

            row.sizeDelta = Scroller.ScrollDirection == ScrollDirection.Horizontal
                ? new Vector2(cell.rect.width, row.sizeDelta.y)
                : new Vector2(row.sizeDelta.x, cell.rect.height);

            return row.gameObject;
        }

        public virtual void UpdateContents(IList<TItemData> items)
        {
            DataCount = items.Count;

            var rows = items
                .Select((item, index) => (item, index))
                .GroupBy(
                    x => x.index / ColumnCount,
                    x => x.item)
                .Select(group => group.ToArray())
                .ToArray();

            UpdateContents(rows);
        }

        public override void ScrollTo(int itemIndex, float duration, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            var rowIndex = itemIndex / Context.GetColumnCount();
            base.ScrollTo(rowIndex, duration, alignment, onComplete);
        }

        public override void ScrollTo(int itemIndex, float duration, Ease easing, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            var rowIndex = itemIndex / Context.GetColumnCount();
            base.ScrollTo(rowIndex, duration, easing, alignment, onComplete);
        }

        public virtual void JumpTo(int itemIndex, Alignment alignment = Alignment.Center)
        {
            var rowIndex = itemIndex / Context.GetColumnCount();
            UpdatePosition(rowIndex, alignment);
        }
    }
}
