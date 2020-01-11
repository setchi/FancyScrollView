/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyGridView{TItemData, TContext}"/> のセルを実装するための抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext"><see cref="FancyCell{TItemData, TContext}.Context"/> の型.</typeparam>
    public abstract class FancyGridViewCell<TItemData, TContext> : FancyCell<TItemData, TContext>
        where TContext : class, IFancyGridViewContext, new()
    {
        /// <inheritdoc/>
        public override void UpdatePosition(float position)
        {
            var cellSize = Context.GetCellSize();
            var spacing = Context.GetColumnSpacing();
            var columnCount = Context.GetColumnCount();

            var count = Index % columnCount;
            var p = (cellSize + spacing) * (count - (columnCount - 1) * 0.5f);

            transform.localPosition = Context.ScrollDirection == ScrollDirection.Horizontal
                ? new Vector2(transform.localPosition.x, p)
                : new Vector2(p, transform.localPosition.y);
        }
    }

    /// <summary>
    /// <see cref="FancyGridView{TItemData}"/> のセルを実装するための抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <seealso cref="FancyGridViewCell{TItemData, TContext}"/>
    public abstract class FancyGridViewCell<TItemData> : FancyGridViewCell<TItemData, FancyGridViewContext>
    {
        /// <inheritdoc/>
        public sealed override void SetContext(FancyGridViewContext context) => base.SetContext(context);
    }
}
