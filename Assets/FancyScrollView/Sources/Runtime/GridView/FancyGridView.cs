using System;
using System.Linq;
using System.Collections.Generic;
using EasingCore;

namespace FancyScrollView
{
    public abstract class FancyGridView<TItemData, TContext> : FancyScrollRect<FancyGridRowData<TItemData>, TContext>
        where TContext : class, IFancyScrollRectContext, IFancyGridViewContext, new()
    {
        protected abstract int ColumnCount { get; }

        public int DataCount { get; private set; }

        public virtual void UpdateData(IList<TItemData> items)
        {
            Context.ColumnCount = ColumnCount;
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

        public override void ScrollTo(int itemIndex, float duration, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            var rowIndex = itemIndex / Context.ColumnCount;
            base.ScrollTo(rowIndex, duration, alignment, onComplete);
        }

        public override void ScrollTo(int itemIndex, float duration, Ease easing, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            var rowIndex = itemIndex / Context.ColumnCount;
            base.ScrollTo(rowIndex, duration, easing, alignment, onComplete);
        }

        public virtual void JumpTo(int itemIndex, Alignment alignment = Alignment.Center)
        {
            var rowIndex = itemIndex / Context.ColumnCount;
            UpdatePosition(rowIndex, alignment);
        }
    }
}
