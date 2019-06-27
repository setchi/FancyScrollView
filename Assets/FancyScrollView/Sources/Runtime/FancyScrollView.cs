using System;
using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollView<TItemData, TContext> : MonoBehaviour where TContext : class, new()
    {
        [SerializeField, Range(float.Epsilon, 1f)] protected float cellInterval = 0.2f;
        [SerializeField, Range(0f, 1f)] protected float scrollOffset = 0.5f;
        [SerializeField] protected bool loop = false;
        [SerializeField] protected Transform cellContainer = default;

        readonly IList<FancyScrollViewCell<TItemData, TContext>> pool =
            new List<FancyScrollViewCell<TItemData, TContext>>();

        float currentPosition;

        protected abstract GameObject CellPrefab { get; }
        protected IList<TItemData> ItemsSource { get; set; } = new List<TItemData>();
        protected TContext Context { get; } = new TContext();

        /// <summary>
        /// Updates the contents.
        /// </summary>
        /// <param name="itemsSource">Items source.</param>
        protected void UpdateContents(IList<TItemData> itemsSource)
        {
            ItemsSource = itemsSource;
            Refresh();
        }

        /// <summary>
        /// Refreshes the cells.
        /// </summary>
        protected void Refresh() => UpdatePosition(currentPosition, true);

        /// <summary>
        /// Updates the scroll position.
        /// </summary>
        /// <param name="position">Position.</param>
        protected void UpdatePosition(float position) => UpdatePosition(position, false);

        void UpdatePosition(float position, bool forceRefresh)
        {
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
            if (CellPrefab == null)
            {
                throw new NullReferenceException(nameof(CellPrefab));
            }

            if (cellContainer == null)
            {
                throw new MissingComponentException(nameof(cellContainer));
            }

            var addCount = Mathf.CeilToInt((1f - firstPosition) / cellInterval) - pool.Count;
            for (var i = 0; i < addCount; i++)
            {
                var cell = Instantiate(CellPrefab, cellContainer).GetComponent<FancyScrollViewCell<TItemData, TContext>>();
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
            if (cachedLoop != loop || cachedCellInterval != cellInterval || cachedScrollOffset != scrollOffset)
            {
                cachedLoop = loop;
                cachedCellInterval = cellInterval;
                cachedScrollOffset = scrollOffset;

                UpdatePosition(currentPosition);
            }
        }
#endif
    }

    public sealed class FancyScrollViewNullContext
    {
    }

    public abstract class FancyScrollView<TItemData> : FancyScrollView<TItemData, FancyScrollViewNullContext>
    {
    }
}
