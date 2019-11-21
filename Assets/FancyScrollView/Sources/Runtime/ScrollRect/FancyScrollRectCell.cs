/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyScrollRect{TItemData, TContext}"/> のセルを実装するための抽象基底クラス.
    /// <see cref="FancyScrollViewCell{TItemData, TContext}.Context"/> が不要な場合は
    /// 代わりに <see cref="FancyScrollRectCell{TItemData}"/> を使用します.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext"><see cref="FancyScrollViewCell{TItemData, TContext}.Context"/> の型.</typeparam>
    public abstract class FancyScrollRectCell<TItemData, TContext> : FancyScrollViewCell<TItemData, TContext>
        where TContext : class, IFancyScrollRectContext, new()
    {
        /// <inheritdoc/>
        public override void UpdatePosition(float position)
        {
            var (scrollSize, reuseMargin) = Context.CalculateScrollSize();

            var unclampedPosition = (Mathf.Lerp(0f, scrollSize, position) - reuseMargin) / (scrollSize - reuseMargin * 2f);

            var start = 0.5f * scrollSize;
            var end = -start;

            UpdatePosition(unclampedPosition, Mathf.Lerp(start, end, position));
        }

        /// <summary>
        /// このセルの位置を更新します.
        /// </summary>
        /// <param name="position">
        /// ビューポートの範囲で正規化されたスクロール位置.
        /// <see cref="FancyScrollRect{TItemData, TContext}.reuseCellMarginCount"/> の値に基づいて
        ///  <c>0.0</c> ~ <c>1.0</c> の範囲を超えた値が渡されることがあります.
        /// </param>
        /// <param name="viewportPosition">ローカル位置.</param>
        protected virtual void UpdatePosition(float position, float viewportPosition) { }
    }

    /// <summary>
    /// <see cref="FancyScrollRect{TItemData}"/> のセルを実装するための抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <seealso cref="FancyScrollRectCell{TItemData, TContext}"/>
    public abstract class FancyScrollRectCell<TItemData> : FancyScrollRectCell<TItemData, FancyScrollRectContext>
    {
        /// <inheritdoc/>
        public sealed override void SetupContext(FancyScrollRectContext context) => base.SetupContext(context);
    }
}
