using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView
{
    public abstract class FancyGridView<TItemData, TContext> : FancyScrollRect<FancyGridRowData<TItemData>, TContext>
        where TContext : class, IFancyScrollRectContext, IFancyGridViewContext, new()
    {
        [SerializeField] protected float columnSpacing = 0f;

        protected GameObject cachedRowPrefab;
        protected sealed override GameObject CellPrefab => cachedRowPrefab ?? (cachedRowPrefab = SetupRowTemplate());

        protected abstract FancyScrollViewCell<TItemData, TContext> CellTemplate { get; }

        protected abstract FancyGridViewRow<TItemData, TContext> RowTemplate { get; }

        protected abstract int ColumnCount { get; }

        public int DataCount { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Context.GetColumnCount = () => ColumnCount;
            Context.GetColumnSpacing = () => columnSpacing;
            Context.CellTemplate = CellTemplate.gameObject;
            Context.ScrollDirection = Scroller.ScrollDirection;
        }

        protected virtual GameObject SetupRowTemplate()
        {
            Debug.Assert(CellTemplate != null);
            Debug.Assert(RowTemplate != null);

            var cell = CellTemplate.GetComponent<RectTransform>();
            var row = RowTemplate.GetComponent<RectTransform>();

            row.sizeDelta = Scroller.ScrollDirection == ScrollDirection.Horizontal
                ? new Vector2(cell.rect.width, row.sizeDelta.y)
                : new Vector2(row.sizeDelta.x, cell.rect.height);

            return row.gameObject;
        }

        public virtual void UpdateData(IList<TItemData> items)
        {
            Debug.Assert(Context.GetColumnCount != null);
            Debug.Assert(Context.GetColumnCount() > 0);

            DataCount = items.Count;

            var rows = items
                .Select((item, index) => (item, index))
                .GroupBy(
                    x => x.index / ColumnCount,
                    x => x.item)
                .Select(group => new FancyGridRowData<TItemData>(group.ToArray()))
                .ToArray();

            UpdateContents(rows);
        }

        protected override void UpdateContents(IList<FancyGridRowData<TItemData>> items)
        {
            Debug.Assert(Context.GetColumnSpacing != null);
            base.UpdateContents(items);
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
