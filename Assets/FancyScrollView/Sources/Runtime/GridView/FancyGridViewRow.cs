using UnityEngine;
using System.Linq;

namespace FancyScrollView
{
    public abstract class FancyGridViewRow<TItemData, TContext> : FancyScrollRectCell<TItemData[], TContext>
        where TContext : class, IFancyScrollRectContext, IFancyGridViewContext, new()
    {
        protected virtual FancyScrollViewCell<TItemData, TContext>[] Cells { get; private set; }

        protected virtual FancyScrollViewCell<TItemData, TContext>[] InstantiateCells()
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

            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].SetupContext(context);
            }
        }

        public override void UpdateContent(TItemData[] rowContents)
        {
            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].Index = i + Index * Context.GetColumnCount();
                Cells[i].SetVisible(i < rowContents.Length);

                if (Cells[i].IsVisible)
                {
                    Cells[i].UpdateContent(rowContents[i]);
                }
            }
        }

        public override void UpdatePosition(float position)
        {
            base.UpdatePosition(position);

            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].UpdatePosition(position);
            }
        }

        protected override void UpdatePosition(float position, float viewportPosition)
        {
            transform.localPosition = Context.ScrollDirection == ScrollDirection.Horizontal
                ? new Vector2(viewportPosition, transform.localPosition.y)
                : new Vector2(transform.localPosition.x, viewportPosition);
        }
    }
}
