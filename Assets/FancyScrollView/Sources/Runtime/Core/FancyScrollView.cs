using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// スクロールビューの抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext">コンテキストの型.</typeparam>
    public abstract class FancyScrollView<TItemData, TContext> : MonoBehaviour where TContext : class, new()
    {
        /// <summary>
        /// セル同士の間隔.
        /// </summary>
        [SerializeField, Range(1e-2f, 1f)] protected float cellInterval = 0.2f;

        /// <summary>
        /// スクロール位置の基準.
        /// </summary>
        /// <remarks>
        /// たとえば、 0.5 を指定してスクロール位置が 0 の場合, 中央に最初のセルが配置されます.
        /// </remarks>
        [SerializeField, Range(0f, 1f)] protected float scrollOffset = 0.5f;

        /// <summary>
        /// セルを循環して配置させるどうか.
        /// </summary>
        /// <remarks>
        /// true にすると最後のセルの後に最初のセル, 最初のセルの前に最後のセルが並ぶようになります.
        /// 無限スクロールを実装する場合は true を指定します.
        /// </remarks>
        [SerializeField] protected bool loop = false;

        /// <summary>
        /// セルの親要素となる Transform.
        /// </summary>
        [SerializeField] protected Transform cellContainer = default;

        readonly IList<FancyScrollViewCell<TItemData, TContext>> pool =
            new List<FancyScrollViewCell<TItemData, TContext>>();

        protected bool initialized;
        protected float currentPosition;

        /// <summary>
        /// セルの Prefab.
        /// </summary>
        protected abstract GameObject CellPrefab { get; }

        /// <summary>
        /// アイテム一覧のデータ.
        /// </summary>
        protected IList<TItemData> ItemsSource { get; set; } = new List<TItemData>();

        /// <summary>
        /// コンテキストのインスタンス.
        /// </summary>
        /// <remarks>
        /// セルとスクロールビュー間で同じインスタンスが共有されます. 情報の受け渡しや状態の保持に使用します.
        /// </remarks>
        protected TContext Context { get; } = new TContext();

        /// <summary>
        /// 初期化を行います.
        /// </summary>
        /// <remarks>
        /// 最初にセルが生成される直前に呼び出されます.
        /// </remarks>
        protected virtual void Initialize() { }

        /// <summary>
        /// 渡されたアイテム一覧に基づいて表示内容を更新します.
        /// </summary>
        /// <param name="itemsSource">アイテム一覧.</param>
        protected virtual void UpdateContents(IList<TItemData> itemsSource)
        {
            ItemsSource = itemsSource;
            Refresh();
        }

        /// <summary>
        /// セルの表示内容を更新します.
        /// </summary>
        protected virtual void Refresh() => UpdatePosition(currentPosition, true);

        /// <summary>
        /// スクロール位置を更新します.
        /// </summary>
        /// <param name="position">スクロール位置.</param>
        protected virtual void UpdatePosition(float position) => UpdatePosition(position, false);

        void UpdatePosition(float position, bool forceRefresh)
        {
            if (!initialized)
            {
                Initialize();
                initialized = true;
            }

            currentPosition = position;

            var p = position - scrollOffset / cellInterval;
            var firstIndex = Mathf.CeilToInt(p);
            var firstPosition = (Mathf.Ceil(p) - p) * cellInterval;

            if (firstPosition + pool.Count * cellInterval < 1f)
            {
                ResizePool(firstPosition);
            }

            UpdateCells(firstPosition, firstIndex, forceRefresh);
        }

        void ResizePool(float firstPosition)
        {
            Debug.Assert(CellPrefab != null);
            Debug.Assert(cellContainer != null);

            var addCount = Mathf.CeilToInt((1f - firstPosition) / cellInterval) - pool.Count;
            for (var i = 0; i < addCount; i++)
            {
                var cell = Instantiate(CellPrefab, cellContainer)
                    .GetComponent<FancyScrollViewCell<TItemData, TContext>>();
                if (cell == null)
                {
                    throw new MissingComponentException(
                        $"FancyScrollViewCell<{typeof(TItemData).FullName}, {typeof(TContext).FullName}> " +
                        $"component not found in {CellPrefab.name}.");
                }

                cell.SetupContext(Context);
                cell.SetVisible(false);
                pool.Add(cell);
            }
        }

        void UpdateCells(float firstPosition, int firstIndex, bool forceRefresh)
        {
            for (var i = 0; i < pool.Count; i++)
            {
                var index = firstIndex + i;
                var position = firstPosition + i * cellInterval;
                var cell = pool[CircularIndex(index, pool.Count)];

                if (loop)
                {
                    index = CircularIndex(index, ItemsSource.Count);
                }

                if (index < 0 || index >= ItemsSource.Count || position > 1f)
                {
                    cell.SetVisible(false);
                    continue;
                }

                if (forceRefresh || cell.Index != index || !cell.IsVisible)
                {
                    cell.Index = index;
                    cell.SetVisible(true);
                    cell.UpdateContent(ItemsSource[index]);
                }

                cell.UpdatePosition(position);
            }
        }

        int CircularIndex(int i, int size) => size < 1 ? 0 : i < 0 ? size - 1 + (i + 1) % size : i % size;

#if UNITY_EDITOR
        bool cachedLoop;
        float cachedCellInterval, cachedScrollOffset;

        void LateUpdate()
        {
            if (cachedLoop != loop ||
                cachedCellInterval != cellInterval ||
                cachedScrollOffset != scrollOffset)
            {
                cachedLoop = loop;
                cachedCellInterval = cellInterval;
                cachedScrollOffset = scrollOffset;

                UpdatePosition(currentPosition);
            }
        }
#endif
    }

    /// <summary>
    /// <see cref="FancyScrollView{TItemData}"/> を使用した際にシステムが使用する <see cref="FancyScrollView{TItemData, TContext}.Context"/>.
    /// </summary>
    public sealed class FancyScrollViewNullContext { }

    /// <summary>
    /// スクロールビューの抽象基底クラス.
    /// </summary>
    /// <remarks>
    /// <see cref="FancyScrollView{TItemData, TContext}.Context"/> が不要な場合はこちらを使用します.
    /// </remarks>
    /// <typeparam name="TItemData"></typeparam>
    public abstract class FancyScrollView<TItemData> : FancyScrollView<TItemData, FancyScrollViewNullContext> { }
}
