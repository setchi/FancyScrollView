/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using System.Linq;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyGridView{TItemData, TContext}"/> の行を実装するための抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext"><see cref="FancyScrollViewCell{TItemData, TContext}.Context"/> の型.</typeparam>
    public abstract class FancyGridViewRow<TItemData, TContext> : FancyScrollRectCell<TItemData[], TContext>
        where TContext : class, IFancyScrollRectContext, IFancyGridViewContext, new()
    {
        /// <summary>
        /// この行で表示するセルの配列.
        /// </summary>
        protected virtual FancyScrollViewCell<TItemData, TContext>[] Cells { get; private set; }

        /// <summary>
        /// この行で表示するセルの配列をインスタンス化します.
        /// </summary>
        /// <returns>この行で表示するセルの配列.</returns>
        protected virtual FancyScrollViewCell<TItemData, TContext>[] InstantiateCells()
        {
            return Enumerable.Range(0, Context.GetColumnCount())
                .Select(_ => Instantiate(Context.CellTemplate, transform))
                .Select(x => x.GetComponent<FancyScrollViewCell<TItemData, TContext>>())
                .ToArray();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override void UpdatePosition(float position)
        {
            base.UpdatePosition(position);

            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].UpdatePosition(position);
            }
        }

        /// <inheritdoc/>
        protected override void UpdatePosition(float position, float viewportPosition)
        {
            transform.localPosition = Context.ScrollDirection == ScrollDirection.Horizontal
                ? new Vector2(viewportPosition, transform.localPosition.y)
                : new Vector2(transform.localPosition.x, viewportPosition);
        }
    }
}
