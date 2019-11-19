﻿using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// FancyScrollRect セルの抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext">コンテキストの型.</typeparam>
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
        ///  0.0 ~ 1.0 の範囲を超えた値が渡されることがあります.
        /// </param>
        /// <param name="viewportPosition">ローカル位置.</param>
        protected virtual void UpdatePosition(float position, float viewportPosition) { }
    }

    /// <summary>
    /// FancyScrollRect セルの抽象基底クラス.
    /// </summary>
    /// <remarks>
    /// <see cref="FancyScrollRectCell{TItemData, TContext}.Context"/> が不要な場合はこちらを使用します.
    /// </remarks>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    public abstract class FancyScrollRectCell<TItemData> : FancyScrollRectCell<TItemData, FancyScrollRectContext>
    {
        /// <inheritdoc/>
        public sealed override void SetupContext(FancyScrollRectContext context) => base.SetupContext(context);
    }
}
