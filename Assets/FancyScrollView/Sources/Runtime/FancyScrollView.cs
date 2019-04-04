using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollView<TItemData, TContext> : MonoBehaviour where TContext : class, new()
    {
        [SerializeField, Range(float.Epsilon, 1f)] float cellSpacing = default;
        [SerializeField, Range(0f, 1f)] float scrollOffset = default;
        [SerializeField] bool loop = default;
        [SerializeField] Transform cellContainer = default;

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

            var p = position - scrollOffset / cellSpacing;
            var firstIndex = Mathf.CeilToInt(p);
            var firstPosition = (Mathf.Ceil(p) - p) * cellSpacing;

            if (firstPosition + pool.Count * cellSpacing <= 1f)
            {
                ResizePool(firstPosition);
            }

            UpdateCells(firstPosition, firstIndex, forceRefresh);
        }

        void ResizePool(float firstPosition)
        {
            if (CellPrefab == null)
            {
                throw new System.NullReferenceException(nameof(CellPrefab));
            }

            if (cellContainer == null)
            {
                throw new MissingComponentException(nameof(cellContainer));
            }

            var addCount = Mathf.CeilToInt((1f - firstPosition) / cellSpacing) - pool.Count;
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
                var position = firstPosition + i * cellSpacing;
                var cell = pool[CircularIndex(index, pool.Count)];

                index = loop ? CircularIndex(index, ItemsSource.Count) : index;
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

                if (cell.gameObject.activeSelf)
                {
                    cell.UpdatePosition(position);
                }
            }
        }

        int CircularIndex(int i, int size) => size < 1 ? 0 : i < 0 ? size - 1 + (i + 1) % size : i % size;

#if UNITY_EDITOR
        bool cachedLoop;
        float cachedCellSpacing, cachedScrollOffset;

        void LateUpdate()
        {
            if (cachedLoop != loop || cachedCellSpacing != cellSpacing || cachedScrollOffset != scrollOffset)
            {
                cachedLoop = loop;
                cachedCellSpacing = cellSpacing;
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
