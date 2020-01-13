/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyGridView{TItemData, TContext}"/> のセルを実装するための抽象基底クラス.
    /// <see cref="FancyCell{TItemData, TContext}.Context"/> が不要な場合は
    /// 代わりに <see cref="FancyGridViewCell{TItemData}"/> を使用します.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext"><see cref="FancyCell{TItemData, TContext}.Context"/> の型.</typeparam>
    public abstract class FancyGridViewCell<TItemData, TContext> : FancyScrollRectCell<TItemData, TContext>
        where TContext : class, IFancyGridViewContext, new()
    {
        /// <inheritdoc/>
        protected override void UpdatePosition(float normalizedPosition, float localPosition)
        {
            var cellSize = Context.GetCellSize();
            var spacing = Context.GetStartAxisSpacing();
            var groupCount = Context.GetGroupCount();

            var indexInGroup = Index % groupCount;
            var positionInGroup = (cellSize + spacing) * (indexInGroup - (groupCount - 1) * 0.5f);

            transform.localPosition = Context.ScrollDirection == ScrollDirection.Horizontal
                ? new Vector2(-localPosition, -positionInGroup)
                : new Vector2(positionInGroup, localPosition);
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
