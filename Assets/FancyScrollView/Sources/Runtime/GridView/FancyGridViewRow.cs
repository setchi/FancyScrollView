using UnityEngine;
using System.Linq;

namespace FancyScrollView
{
    public class FancyGridViewRow<TItemData, TContext> : FancyScrollRectCell<FancyGridRowData<TItemData>, TContext>
        where TContext : class, IFancyScrollRectContext, IFancyGridViewContext, new()
    {
        protected FancyScrollViewCell<TItemData, TContext>[] Cells { get; private set; }

        protected FancyScrollViewCell<TItemData, TContext>[] InstantiateCells()
        {
            return Enumerable.Range(0, Context.GetColumnCount())
                .Select(_ => Instantiate(Context.CellTemplate, transform))
                .Select(x => x.GetComponent<FancyScrollViewCell<TItemData, TContext>>())
                .ToArray();
        }

        public override void SetupContext(TContext context)
        {
            base.SetupContext(context);

            Cells = InstantiateCells();
            Debug.Assert(Cells.Length == Context.GetColumnCount());

            foreach (var cell in Cells)
            {
                cell.SetupContext(context);
            }
        }

        public override void UpdateContent(FancyGridRowData<TItemData> row)
        {
            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].Index = i + Index * Context.GetColumnCount();
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

        protected override void UpdatePosition(float position, float viewportPosition)
        {
            var x = transform.localPosition.x;
            var y = viewportPosition;

            transform.localPosition = new Vector2(x, y);
        }
    }
}
