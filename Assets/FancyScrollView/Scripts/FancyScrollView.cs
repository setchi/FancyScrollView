using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollView<TItemData, TContext> : MonoBehaviour where TContext : class, new()
    {
        [SerializeField, Range(float.Epsilon, 1f)] float cellSpacing;
        [SerializeField, Range(0f, 1f)] float scrollOffset;
        [SerializeField] bool loop;
        [SerializeField] Transform cellContainer;

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
            var firstPosition = (Mathf.Ceil(p) - p) * cellSpacing;
            var firstIndex = Mathf.CeilToInt(p);

            if (firstPosition + pool.Count * cellSpacing <= 1f)
            {
                GrowPool(firstPosition);
            }

            UpdateCells(firstPosition, firstIndex, forceRefresh);
        }

        void GrowPool(float firstPosition)
        {
            if (CellPrefab == null)
            {
                throw new System.NullReferenceException(nameof(CellPrefab));
            }

            if (cellContainer == null)
            {
                throw new MissingComponentException(nameof(cellContainer));
            }

            for (var (count, p) = (0, firstPosition); p <= 1f; p += cellSpacing, count++)
            {
                if (count < pool.Count)
                {
                    continue;
                }

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
            var count = 0;

            for (var p = firstPosition; p <= 1f; p += cellSpacing, count++)
            {
                var index = firstIndex + count;
                var cell = pool[GetCircularIndex(index, pool.Count)];

                UpdateCell(cell, index, forceRefresh);

                if (cell.gameObject.activeSelf)
                {
                    cell.UpdatePosition(p);
                }
            }

            while (count < pool.Count)
            {
                pool[GetCircularIndex(firstIndex + count, pool.Count)].SetVisible(false);
                count++;
            }
        }

        void UpdateCell(FancyScrollViewCell<TItemData, TContext> cell, int newIndex, bool forceRefresh)
        {
            if (loop)
            {
                newIndex = GetCircularIndex(newIndex, ItemsSource.Count);
            }
            else if (newIndex < 0 || newIndex > ItemsSource.Count - 1)
            {
                cell.SetVisible(false);
                return;
            }

            if (!forceRefresh && cell.ItemIndex == newIndex && cell.IsVisible)
            {
                return;
            }

            cell.ItemIndex = newIndex;
            cell.SetVisible(true);
            cell.UpdateContent(ItemsSource[newIndex]);
        }

        int GetCircularIndex(int index, int maxSize) =>
            index < 0 ? maxSize - 1 + (index + 1) % maxSize : index % maxSize;

#if UNITY_EDITOR
        bool cachedLoop;
        float cachedCellSpacing, cachedScrollOffset;

        void LateUpdate()
        {
            if ((cachedLoop, cachedScrollOffset, cachedCellSpacing) != (loop, scrollOffset, cellSpacing))
            {
                (cachedLoop, cachedScrollOffset, cachedCellSpacing) = (loop, scrollOffset, cellSpacing);
                UpdatePosition(currentPosition);
            }
        }
#endif
    }

    public sealed class FancyScrollViewNullContext
    {
    }

    public abstract class FancyScrollView<TCellData> : FancyScrollView<TCellData, FancyScrollViewNullContext>
    {
    }
}
