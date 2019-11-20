﻿using System;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView
{
    /// <summary>
    /// ScrollRect 風スクロールビューを実装するための抽象基底クラス.
    /// 無限スクロールおよびスナップには対応していません.
    /// <see cref="FancyScrollView{TItemData, TContext}.Context"/> が不要な場合は
    /// 代わりに <see cref="FancyScrollRect{TItemData}"/> を使用します.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext"><see cref="FancyScrollView{TItemData, TContext}.Context"/> の型.</typeparam>
    [RequireComponent(typeof(Scroller))]
    public abstract class FancyScrollRect<TItemData, TContext> : FancyScrollView<TItemData, TContext>
        where TContext : class, IFancyScrollRectContext, new()
    {
        /// <summary>
        /// スクロール中にセルが再利用されるまでの余白のセル数.
        /// </summary>
        /// <remarks>
        /// 0 を指定するとセルが完全に隠れた直後に再利用されます.
        /// 1 以上を指定すると, そのセル数だけ余分にスクロールしてから再利用されます.
        /// </remarks>
        [SerializeField] protected float reuseCellMarginCount = 0f;

        /// <summary>
        /// コンテンツ先頭の余白.
        /// </summary>
        [SerializeField] protected float paddingHead = 0f;

        /// <summary>
        /// コンテンツ末尾の余白.
        /// </summary>
        [SerializeField] protected float paddingTail = 0f;

        /// <summary>
        /// セル同士の余白.
        /// </summary>
        [SerializeField] protected float spacing = 0f;

        protected virtual float ScrollLength => 1f / Mathf.Max(cellInterval, 1e-2f) - 1f;

        protected virtual float ViewportLength => ScrollLength - reuseCellMarginCount * 2f;

        protected virtual float PaddingHeadLength => (paddingHead - spacing * 0.5f) / (CellSize + spacing);

        protected virtual float MaxScrollPosition => ItemsSource.Count
            - ScrollLength
            + reuseCellMarginCount * 2f
            + (paddingHead + paddingTail - spacing) / (CellSize + spacing);

        /// <summary>
        /// スクロール可能かどうか.
        /// </summary>
        /// <remarks>
        /// アイテム数が十分少なくビューポート内に全てのセルが収まっている場合は false, それ以外は true になります.
        /// </remarks>
        protected virtual bool Scrollable => MaxScrollPosition > 0f;

        /// <summary>
        /// セルのサイズ.
        /// </summary>
        protected virtual float CellSize => Scroller.ScrollDirection == ScrollDirection.Horizontal
            ? CellRectTransform.rect.width
            : CellRectTransform.rect.height;

        /// <summary>
        /// スクロール位置を制御する <see cref="Scroller"/> のインスタンス.
        /// </summary>
        /// <remarks>
        /// <see cref="Scroller"/> のスクロール位置を変更する際は必ず <see cref="ToScrollerPosition(float)"/> を使用して変換した位置を渡してください.
        /// </remarks>
        Scroller cachedScroller;
        protected Scroller Scroller => cachedScroller ?? (cachedScroller = GetComponent<Scroller>());

        RectTransform cachedCellRect;
        RectTransform CellRectTransform => cachedCellRect ?? (cachedCellRect = CellPrefab.transform as RectTransform);

        /// <inheritdoc/>
        protected override void Initialize()
        {
            base.Initialize();

            Context.CalculateScrollSize = () =>
            {
                var interval = CellSize + spacing;
                var reuseMargin = interval * reuseCellMarginCount;
                var scrollSize = Scroller.ViewportSize + interval + reuseMargin * 2f;
                return (scrollSize, reuseMargin);
            };

            AdjustCellIntervalAndScrollOffset();
            Scroller.OnValueChanged(OnScrollerValueChanged);
        }

        /// <summary>
        /// <see cref="Scroller"/> のスクロール位置が変更された際の処理.
        /// </summary>
        /// <param name="p"><see cref="Scroller"/> のスクロール位置.</param>
        void OnScrollerValueChanged(float p)
        {
            base.UpdatePosition(Scrollable ? ToFancyScrollViewPosition(p) : 0f);

            if (Scroller.Scrollbar)
            {
                if (p > ItemsSource.Count - 1)
                {
                    ShrinkScrollbar(p - (ItemsSource.Count - 1));
                }
                else if (p < 0f)
                {
                    ShrinkScrollbar(-p);
                }
            }
        }

        /// <summary>
        /// スクロール範囲を超えてスクロールされた量に基づいて, スクロールバーのサイズを縮小します.
        /// </summary>
        /// <param name="offset">スクロール範囲を超えてスクロールされた量.</param>
        void ShrinkScrollbar(float offset)
        {
            var scale = 1f - ToFancyScrollViewPosition(offset) / (ViewportLength - PaddingHeadLength);
            UpdateScrollbarSize((ViewportLength - PaddingHeadLength) * scale);
        }

        /// <inheritdoc/>
        protected override void Refresh()
        {
            AdjustCellIntervalAndScrollOffset();
            RefreshScroller();
            base.Refresh();
        }

        /// <summary>
        /// <see cref="Scroller"/> の各種状態を更新します.
        /// </summary>
        protected void RefreshScroller()
        {
            Scroller.Draggable = Scrollable;
            Scroller.ScrollSensitivity = ToScrollerPosition(ViewportLength - PaddingHeadLength);
            Scroller.Position = ToScrollerPosition(currentPosition);

            if (Scroller.Scrollbar)
            {
                Scroller.Scrollbar.gameObject.SetActive(Scrollable);
                UpdateScrollbarSize(ViewportLength);
            }
        }

        /// <inheritdoc/>
        protected override void UpdateContents(IList<TItemData> items)
        {
            Debug.Assert(Context.CalculateScrollSize != null);

            AdjustCellIntervalAndScrollOffset();
            base.UpdateContents(items);

            Scroller.SetTotalCount(items.Count);
            RefreshScroller();
        }

        /// <summary>
        /// スクロール位置を更新します.
        /// </summary>
        /// <param name="position">スクロール位置.</param>
        protected new void UpdatePosition(float position)
        {
            UpdatePosition(position, Alignment.Center);
        }

        /// <summary>
        /// スクロール位置を更新します.
        /// </summary>
        /// <param name="position">スクロール位置.</param>
        /// <param name="alignment"><see cref="Alignment"/>.</param>
        protected virtual void UpdatePosition(float position, Alignment alignment)
        {
            Scroller.Position = ToScrollerPosition(position, alignment);
        }

        /// <summary>
        /// 指定したインデックスの位置まで移動します.
        /// </summary>
        /// <param name="index">アイテムのインデックス.</param>
        /// <param name="duration">移動にかける秒数.</param>
        /// <param name="alignment"><see cref="Alignment"/>.</param>
        /// <param name="onComplete">移動が完了した際に呼び出されるコールバック.</param>
        public virtual void ScrollTo(int index, float duration, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            Scroller.ScrollTo(ToScrollerPosition(index, alignment), duration, onComplete);
        }

        /// <summary>
        /// 指定したインデックスの位置まで移動します.
        /// </summary>
        /// <param name="index">アイテムのインデックス.</param>
        /// <param name="duration">移動にかける秒数.</param>
        /// <param name="easing">移動に使用するイージング.</param>
        /// <param name="alignment"><see cref="Alignment"/>.</param>
        /// <param name="onComplete">移動が完了した際に呼び出されるコールバック.</param>
        public virtual void ScrollTo(int index, float duration, Ease easing, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            Scroller.ScrollTo(ToScrollerPosition(index, alignment), duration, easing, onComplete);
        }

        /// <summary>
        /// ビューポートとコンテンツの長さに基づいてスクロールバーのサイズを更新します.
        /// </summary>
        /// <param name="viewportLength">ビューポートのサイズ.</param>
        protected void UpdateScrollbarSize(float viewportLength)
        {
            var contentLength = Mathf.Max(ItemsSource.Count + (paddingHead + paddingTail - spacing) / (CellSize + spacing), 1);
            Scroller.Scrollbar.size = Scrollable ? Mathf.Clamp01(viewportLength / contentLength) : 1f;
        }

        /// <summary>
        /// <see cref="Scroller"/> が扱うスクロール位置を <see cref="FancyScrollRect{TItemData, TContext}"/> が扱うスクロール位置に変換します.
        /// </summary>
        /// <param name="position"><see cref="Scroller"/> が扱うスクロール位置.</param>
        /// <returns><see cref="FancyScrollRect{TItemData, TContext}"/> が扱うスクロール位置.</returns>
        protected virtual float ToFancyScrollViewPosition(float position)
        {
            return position / Mathf.Max(ItemsSource.Count - 1, 1) * MaxScrollPosition - PaddingHeadLength;
        }

        /// <summary>
        /// <see cref="FancyScrollRect{TItemData, TContext}"/> が扱うスクロール位置を <see cref="Scroller"/> が扱うスクロール位置に変換します.
        /// </summary>
        /// <param name="position"><see cref="FancyScrollRect{TItemData, TContext}"/> が扱うスクロール位置.</param>
        /// <returns><see cref="Scroller"/> が扱うスクロール位置.</returns>
        protected virtual float ToScrollerPosition(float position)
        {
            return (position + PaddingHeadLength) / MaxScrollPosition * Mathf.Max(ItemsSource.Count - 1, 1);
        }

        /// <summary>
        /// <see cref="FancyScrollRect{TItemData, TContext}"/> が扱うスクロール位置を <see cref="Scroller"/> が扱うスクロール位置に変換します.
        /// </summary>
        /// <param name="position"><see cref="FancyScrollRect{TItemData, TContext}"/> が扱うスクロール位置.</param>
        /// <param name="alignment"><see cref="Alignment"/>.</param>
        /// <returns><see cref="Scroller"/> が扱うスクロール位置.</returns>
        protected virtual float ToScrollerPosition(float position, Alignment alignment = Alignment.Center)
        {
            var offset = (ScrollLength - (1f + reuseCellMarginCount * 2f)) * GetAnchore(alignment);
            return ToScrollerPosition(Mathf.Clamp(position - offset, 0f, MaxScrollPosition));
        }

        protected virtual float GetAnchore(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Head: return 0.0f;
                case Alignment.Center: return 0.5f;
                case Alignment.Tail: return 1.0f;
                default: return GetAnchore(Alignment.Center);
            }
        }

        /// <summary>
        /// 指定された設定を実現するための
        /// <see cref="FancyScrollView{TItemData,TContext}.cellInterval"/> と
        /// <see cref="FancyScrollView{TItemData,TContext}.scrollOffset"/> を計算して適用します.
        /// </summary>
        protected virtual void AdjustCellIntervalAndScrollOffset()
        {
            var totalSize = Scroller.ViewportSize + (CellSize + spacing) * (1f + reuseCellMarginCount * 2f);
            cellInterval = (CellSize + spacing) / totalSize;
            scrollOffset = cellInterval * (1f + reuseCellMarginCount);
        }

        protected virtual void OnValidate()
        {
            if (CellPrefab)
            {
                AdjustCellIntervalAndScrollOffset();
            }

            if (loop)
            {
                loop = false;
                Debug.LogError("Loop is currently not supported in FancyScrollRect.");
            }

            if (Scroller.SnapEnabled)
            {
                Scroller.SnapEnabled = false;
                Debug.LogError("Snap is currently not supported in FancyScrollRect.");
            }

            if (Scroller.MovementType == MovementType.Unrestricted)
            {
                Scroller.MovementType = MovementType.Elastic;
                Debug.LogError("MovementType.Unrestricted is currently not supported in FancyScrollRect.");
            }
        }
    }

    /// <summary>
    /// ScrollRect 風スクロールビューを実装するための抽象基底クラス.
    /// 無限スクロールおよびスナップには対応していません.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    public abstract class FancyScrollRect<TItemData> : FancyScrollRect<TItemData, FancyScrollRectContext> { }
}
