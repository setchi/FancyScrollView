using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyGridViewRow<TItemData, TContext> : FancyScrollRectCell<FancyGridRowData<TItemData>, TContext>
        where TContext : class, IFancyScrollRectContext, IFancyGridViewContext, new()
    {
        protected FancyScrollViewCell<TItemData, TContext>[] Cells { get; private set; }

        protected abstract FancyScrollViewCell<TItemData, TContext>[] InstantiateCells();

        public override void SetupContext(TContext context)
        {
            base.SetupContext(context);

            Cells = InstantiateCells();
            Debug.Assert(Cells.Length == Context.ColumnCount);

            foreach (var cell in Cells)
            {
                cell.SetupContext(context);
            }
        }

        public override void UpdateContent(FancyGridRowData<TItemData> row)
        {
            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].Index = i + Index * Context.ColumnCount;
                Cells[i].SetVisible(i < row.Entities.Length);

                if (Cells[i].IsVisible)
                {
                    Cells[i].UpdateContent(row.Entities[i]);
                }
            }
        }

        public override void UpdatePosition(float position)
        {
            base.UpdatePosition(position);

            foreach (var cell in Cells)
            {
                cell.UpdatePosition(position);
            }
        }
    }
}
