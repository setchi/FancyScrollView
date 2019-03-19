using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollView<TCellData, TContext> : MonoBehaviour where TContext : class, new()
    {
        [SerializeField, Range(float.Epsilon, 1f)] float cellSpacing;
        [SerializeField, Range(0f, 1f)] float scrollOffset;
        [SerializeField] bool loop;
        [SerializeField] Transform cellContainer;

        readonly IList<FancyScrollViewCell<TCellData, TContext>> cells = new List<FancyScrollViewCell<TCellData, TContext>>();
        float currentPosition;

        protected abstract GameObject CellPrefab { get; }
        protected IList<TCellData> CellData { get; set; } = new List<TCellData>();
        protected TContext Context { get; } = new TContext();

        /// <summary>
        /// Updates the contents.
        /// </summary>
        /// <param name="cellData">Cell data.</param>
        protected void UpdateContents(IList<TCellData> cellData) 
        {
            CellData = cellData;
            UpdateContents();
        }

        /// <summary>
        /// Updates the contents.
        /// </summary>
        protected void UpdateContents() => UpdatePosition(currentPosition, true);

        /// <summary>
        /// Updates the scroll position.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="forceUpdateContents">If set to <c>true</c> force update contents.</param>
        protected void UpdatePosition(float position, bool forceUpdateContents = false)
        {
            currentPosition = position;

            var p = position - scrollOffset / cellSpacing;
            var startPosition = (Mathf.Ceil(p) - p) * cellSpacing;
            CreateCellsIfNeeded(startPosition);

            var startIndex = Mathf.CeilToInt(p);
            UpdateCells(startPosition, startIndex, forceUpdateContents);
        }

        /// <summary>
        /// Creates the cells if needed.
        /// </summary>
        /// <param name="startPosition">Start position.</param>
        void CreateCellsIfNeeded(float startPosition)
        {
            int count = 0;

            for (var p = startPosition; p <= 1f; p += cellSpacing, count++)
            {
                if (count >= cells.Count)
                {
                    cells.Add(CreateCell());
                }
            }
        }

        /// <summary>
        /// Creates the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        FancyScrollViewCell<TCellData, TContext> CreateCell()
        {
            var cell = Instantiate(CellPrefab, cellContainer)
                .GetComponent<FancyScrollViewCell<TCellData, TContext>>();

            cell.SetContext(Context);
            cell.SetVisible(false);
            return cell;
        }

        /// <summary>
        /// Updates the cells.
        /// </summary>
        /// <param name="startPosition">Start position.</param>
        /// <param name="startIndex">Start index.</param>
        /// <param name="forceUpdateContents">If set to <c>true</c> force update contents.</param>
        void UpdateCells(float startPosition, int startIndex, bool forceUpdateContents)
        {
            int count = 0;

            for (var p = startPosition; p <= 1f; p += cellSpacing, count++)
            {
                var dataIndex = startIndex + count;
                var cell = cells[GetCircularIndex(dataIndex, cells.Count)];

                UpdateCell(cell, dataIndex, forceUpdateContents);

                if (cell.gameObject.activeSelf)
                {
                    cell.UpdatePosition(p);
                }
            }

            while (count < cells.Count)
            {
                cells[GetCircularIndex(startIndex + count, cells.Count)].SetVisible(false);
                count++;
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="cell">Cell.</param>
        /// <param name="dataIndex">Data index.</param>
        /// <param name="forceUpdateContents">If set to <c>true</c> force update contents.</param>
        void UpdateCell(FancyScrollViewCell<TCellData, TContext> cell, int dataIndex, bool forceUpdateContents = false)
        {
            if (loop)
            {
                dataIndex = GetCircularIndex(dataIndex, CellData.Count);
            }
            else if (dataIndex < 0 || dataIndex > CellData.Count - 1)
            {
                cell.SetVisible(false);
                return;
            }

            if (!forceUpdateContents && cell.DataIndex == dataIndex && cell.IsVisible)
            {
                return;
            }

            cell.DataIndex = dataIndex;
            cell.SetVisible(true);
            cell.UpdateContent(CellData[dataIndex]);
        }

        /// <summary>
        /// Gets the circular index.
        /// </summary>
        /// <returns>The circular index.</returns>
        /// <param name="index">Index.</param>
        /// <param name="maxSize">Max size.</param>
        int GetCircularIndex(int index, int maxSize) =>
            index < 0 ? maxSize - 1 + (index + 1) % maxSize : index % maxSize;

#if UNITY_EDITOR
        bool cachedLoop;
        float cachedCellSpacing, cachedScrollOffset;

        void LateUpdate()
        {
            if (cachedLoop != loop || cachedScrollOffset != scrollOffset || cachedCellSpacing != cellSpacing)
            {
                cachedLoop = loop;
                cachedScrollOffset = scrollOffset;
                cachedCellSpacing = cellSpacing;

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
