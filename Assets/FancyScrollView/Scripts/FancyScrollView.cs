using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollView<TData, TContext> : MonoBehaviour where TContext : class
    {
        [SerializeField, Range(float.Epsilon, 1f)]
        float cellInterval;
        [SerializeField, Range(0f, 1f)]
        float cellOffset;
        [SerializeField]
        bool loop;
        [SerializeField]
        GameObject cellBase;
        [SerializeField]
        Transform cellContainer;

        readonly List<FancyScrollViewCell<TData, TContext>> cells = new List<FancyScrollViewCell<TData, TContext>>();
        float currentPosition;

        protected List<TData> cellData = new List<TData>();
        protected TContext Context { get; private set; }

        /// <summary>
        /// Sets the context.
        /// </summary>
        /// <param name="context">Context.</param>
        protected void SetContext(TContext context)
        {
            Context = context;

            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].SetContext(context);
            }
        }

        /// <summary>
        /// Updates the contents.
        /// </summary>
        protected void UpdateContents()
        {
            UpdatePosition(currentPosition, true);
        }

        /// <summary>
        /// Updates the scroll position.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="forceUpdateContents">If set to <c>true</c> force update contents.</param>
        protected void UpdatePosition(float position, bool forceUpdateContents = false)
        {
            currentPosition = position;

            var visibleMinPosition = position - (cellOffset / cellInterval);
            var firstCellPosition = (Mathf.Ceil(visibleMinPosition) - visibleMinPosition) * cellInterval;
            var dataStartIndex = Mathf.CeilToInt(visibleMinPosition);
            var count = 0;

            for (float p = firstCellPosition; p <= 1f; p += cellInterval, count++)
            {
                if (count >= cells.Count)
                {
                    cells.Add(CreateCell());
                }
            }

            count = 0;

            for (float p = firstCellPosition; p <= 1f; p += cellInterval, count++)
            {
                var dataIndex = dataStartIndex + count;
                var cell = cells[GetCircularIndex(dataIndex, cells.Count)];

                UpdateCell(cell, dataIndex, forceUpdateContents);

                if (cell.gameObject.activeSelf)
                {
                    cell.UpdatePosition(p);
                }
            }

            while (count < cells.Count)
            {
                cells[GetCircularIndex(dataStartIndex + count, cells.Count)].SetVisible(false);
                count++;
            }
        }

        /// <summary>
        /// Updates the cell.
        /// </summary>
        /// <param name="cell">Cell.</param>
        /// <param name="dataIndex">Data index.</param>
        /// <param name="forceUpdateContents">If set to <c>true</c> force update contents.</param>
        void UpdateCell(FancyScrollViewCell<TData, TContext> cell, int dataIndex, bool forceUpdateContents = false)
        {
            if (loop)
            {
                dataIndex = GetCircularIndex(dataIndex, cellData.Count);
            }
            else if (dataIndex < 0 || dataIndex > cellData.Count - 1)
            {
                // セルに対応するデータが存在しなければセルを表示しない
                cell.SetVisible(false);
                return;
            }

            if (forceUpdateContents || cell.DataIndex != dataIndex || !cell.IsVisible)
            {
                cell.DataIndex = dataIndex;
                cell.SetVisible(true);
                cell.UpdateContent(cellData[dataIndex]);
            }
        }

        /// <summary>
        /// Creates the cell.
        /// </summary>
        /// <returns>The cell.</returns>
        FancyScrollViewCell<TData, TContext> CreateCell()
        {
            var cellObject = Instantiate(cellBase, cellContainer);
            var cell = cellObject.GetComponent<FancyScrollViewCell<TData, TContext>>();

            cell.SetContext(Context);
            cell.SetVisible(false);
            cell.DataIndex = -1;

            return cell;
        }

        /// <summary>
        /// Gets the circular index.
        /// </summary>
        /// <returns>The circular index.</returns>
        /// <param name="index">Index.</param>
        /// <param name="maxSize">Max size.</param>
        int GetCircularIndex(int index, int maxSize)
        {
            return index < 0 ? maxSize - 1 + (index + 1) % maxSize : index % maxSize;
        }

#if UNITY_EDITOR
        bool cachedLoop;
        float cachedCellInterval, cachedCellOffset;

        void LateUpdate()
        {
            if (cachedLoop != loop || cachedCellOffset != cellOffset || cachedCellInterval != cellInterval)
            {
                cachedLoop = loop;
                cachedCellOffset = cellOffset;
                cachedCellInterval = cellInterval;

                UpdatePosition(currentPosition);
            }
        }
#endif
    }

    public sealed class FancyScrollViewNullContext
    {
    }

    public abstract class FancyScrollView<TData> : FancyScrollView<TData, FancyScrollViewNullContext>
    {
    }
}
