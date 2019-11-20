using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using EasingCore;

namespace FancyScrollView
{
    /// <summary>
    /// グリッドレイアウトのスクロールビューを実装するための抽象基底クラス.
    /// 無限スクロールおよびスナップには対応していません.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext"><see cref="FancyScrollView{TItemData, TContext}.Context"/> の型.</typeparam>
    public abstract class FancyGridView<TItemData, TContext> : FancyScrollRect<TItemData[], TContext>
        where TContext : class, IFancyScrollRectContext, IFancyGridViewContext, new()
    {
        /// <summary>
        /// 行同士の余白.
        /// </summary>
        [SerializeField] protected float columnSpacing = 0f;

        GameObject cachedRowPrefab;

        /// <summary>
        /// 行の Prefab.
        /// </summary>
        /// <remarks>
        /// <see cref="FancyGridView{TItemData, TContext}"/> では,
        /// <see cref="FancyScrollView{TItemData, TContext}.CellPrefab"/> を行オブジェクトとして使用します.
        /// </remarks>
        protected sealed override GameObject CellPrefab => cachedRowPrefab ?? (cachedRowPrefab = SetupRowTemplate());

        /// <summary>
        /// 一行あたりの要素数.
        /// </summary>
        protected abstract int ColumnCount { get; }

        /// <summary>
        /// セルのテンプレート.
        /// </summary>
        protected abstract FancyScrollViewCell<TItemData, TContext> CellTemplate { get; }

        /// <summary>
        /// 行オブジェクトのテンプレート.
        /// </summary>
        protected abstract FancyGridViewRow<TItemData, TContext> RowTemplate { get; }

        /// <summary>
        /// アイテムの総数.
        /// </summary>
        public int DataCount { get; private set; }

        /// <inheritdoc/>
        protected override void Initialize()
        {
            base.Initialize();

            Debug.Assert(RowTemplate != null);
            Debug.Assert(CellTemplate != null);
            Debug.Assert(ColumnCount > 0);

            Context.CellTemplate = CellTemplate.gameObject;
            Context.ScrollDirection = Scroller.ScrollDirection;
            Context.GetColumnCount = () => ColumnCount;
            Context.GetColumnSpacing = () => columnSpacing;
        }

        /// <summary>
        /// 行オブジェクトのセットアップを行います.
        /// </summary>
        /// <returns>行を構成する GameObject.</returns>
        protected virtual GameObject SetupRowTemplate()
        {
            var cell = CellTemplate.GetComponent<RectTransform>();
            var row = RowTemplate.GetComponent<RectTransform>();

            row.sizeDelta = Scroller.ScrollDirection == ScrollDirection.Horizontal
                ? new Vector2(cell.rect.width, row.sizeDelta.y)
                : new Vector2(row.sizeDelta.x, cell.rect.height);

            return row.gameObject;
        }

        /// <summary>
        /// 渡されたアイテム一覧に基づいて表示内容を更新します.
        /// </summary>
        /// <param name="items">アイテム一覧.</param>
        public virtual void UpdateContents(IList<TItemData> items)
        {
            DataCount = items.Count;

            var rows = items
                .Select((item, index) => (item, index))
                .GroupBy(
                    x => x.index / ColumnCount,
                    x => x.item)
                .Select(group => group.ToArray())
                .ToArray();

            UpdateContents(rows);
        }

        /// <summary>
        /// 指定したインデックスの位置まで移動します.
        /// </summary>
        /// <param name="itemIndex">アイテムのインデックス.</param>
        /// <param name="duration">移動にかける秒数.</param>
        /// <param name="alignment"><see cref="Alignment"/>.</param>
        /// <param name="onComplete">移動が完了した際に呼び出されるコールバック.</param>
        public override void ScrollTo(int itemIndex, float duration, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            var rowIndex = itemIndex / Context.GetColumnCount();
            base.ScrollTo(rowIndex, duration, alignment, onComplete);
        }

        /// <summary>
        /// 指定したインデックスの位置まで移動します.
        /// </summary>
        /// <param name="itemIndex">アイテムのインデックス.</param>
        /// <param name="duration">移動にかける秒数.</param>
        /// <param name="easing">移動に使用するイージング.</param>
        /// <param name="alignment"><see cref="Alignment"/>.</param>
        /// <param name="onComplete">移動が完了した際に呼び出されるコールバック.</param>
        public override void ScrollTo(int itemIndex, float duration, Ease easing, Alignment alignment = Alignment.Center, Action onComplete = null)
        {
            var rowIndex = itemIndex / Context.GetColumnCount();
            base.ScrollTo(rowIndex, duration, easing, alignment, onComplete);
        }

        /// <summary>
        /// 指定したインデックスの位置までジャンプします.
        /// </summary>
        /// <param name="itemIndex">アイテムのインデックス.</param>
        /// <param name="alignment"><see cref="Alignment"/>.</param>
        public virtual void JumpTo(int itemIndex, Alignment alignment = Alignment.Center)
        {
            var rowIndex = itemIndex / Context.GetColumnCount();
            UpdatePosition(rowIndex, alignment);
        }
    }
}
