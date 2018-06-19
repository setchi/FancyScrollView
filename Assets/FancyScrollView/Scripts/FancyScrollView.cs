using System.Collections.Generic;
using UnityEngine;

namespace FancyScrollView
{
    public enum FancyScrollViewResName
    {
        FullName,
        FilePath,
        FilePathWithoutExtension,
    }

    public class BaseFancyScrollView:MonoBehaviour
    {
        [SerializeField]
        protected FancyScrollViewResName ResNameMode;
        [SerializeField]
        protected float cellInterval;
        [SerializeField]
        protected float cellOffset;
        [SerializeField]
        protected bool loop;
        [SerializeField]
        protected string cellBase;
        [SerializeField]
        protected Transform cellContainer;
    }

    public class FancyScrollView<TData, TContext> : BaseFancyScrollView where TContext : class
    {
        float currentPosition;
        readonly List<FancyScrollViewCell<TData, TContext>> cells = ListPool<FancyScrollViewCell<TData, TContext>>.Get();

        protected TContext context;
        protected List<TData> cellData = ListPool<TData>.Get();
        private bool willquit;

        private static GameObjectPool<string> _pool;
        /// <summary>
        /// you can inherit this and set createfunc for your key
        /// </summary>
        protected static GameObjectPool<string> pool
        {
            get
            {
                if(_pool == null)
                {
                    _pool = new GameObjectPool<string>("FancyScrollViewPool", CreateInstance);
                }
                return _pool;
            }
        }

        static Transform CreateInstance(string key)
        {
            string fixname = System.IO.Path.GetFileNameWithoutExtension(key);
            Transform trans = Resources.Load<Transform>(fixname);
            if(trans != null)
            {
                return Instantiate<Transform>(trans);
            }
            return null;
        }

        protected void OnApplicationQuit()
        {
            willquit = true;
        }

        protected void OnDestroy()
        {
            ListPool<FancyScrollViewCell<TData, TContext>>.Release(cells);
 
            ListPool<TData>.Release(cellData);
            cellData = null;

            if(cellContainer != null && !willquit)
            {
                int childcnt = cellContainer.childCount;
                for(int i =0; i < childcnt;++i)
                {
                    pool.DeSpawn(cellBase, cellContainer.GetChild(i));
                }
            }
        }

        /// <summary>
        /// コンテキストを設定します
        /// </summary>
        /// <param name="context"></param>
        protected void SetContext(TContext context)
        {
            this.context = context;

            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].SetContext(context);
            }
        }

        /// <summary>
        /// セルを生成して返します
        /// </summary>
        /// <returns></returns>
        FancyScrollViewCell<TData, TContext> CreateCell()
        {
            Transform cellObject = pool.Spawn(cellBase);
            if (cellObject == null)
                throw new System.NullReferenceException(string.Format("create {0} failed", cellBase));

            cellObject.gameObject.SetActive(true);
            var cell = cellObject.GetComponent<FancyScrollViewCell<TData, TContext>>();

            var cellRectTransform = cellObject as RectTransform;

            // 親要素の付け替えをおこなうとスケールやサイズが失われるため、変数に保持しておく
            var scale = cellObject.localScale;
            var sizeDelta = Vector2.zero;
            var offsetMin = Vector2.zero;
            var offsetMax = Vector2.zero;

            if (cellRectTransform)
            {
                sizeDelta = cellRectTransform.sizeDelta;
                offsetMin = cellRectTransform.offsetMin;
                offsetMax = cellRectTransform.offsetMax;
            }

            cellObject.SetParent(cellContainer);

            cellObject.localScale = scale;
            if (cellRectTransform)
            {
                cellRectTransform.sizeDelta = sizeDelta;
                cellRectTransform.offsetMin = offsetMin;
                cellRectTransform.offsetMax = offsetMax;
            }

            cell.SetContext(context);
            cell.SetVisible(false);

            return cell;
        }

        float prevCellInterval, prevCellOffset;
        bool prevLoop;

        void LateUpdate()
        {
            if (prevLoop != loop ||
                prevCellOffset != cellOffset ||
                prevCellInterval != cellInterval)
            {
                UpdatePosition(currentPosition);

                prevLoop = loop;
                prevCellOffset = cellOffset;
                prevCellInterval = cellInterval;
            }
        }

        /// <summary>
        /// セルの内容を更新します
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="dataIndex"></param>
        void UpdateCellForIndex(FancyScrollViewCell<TData, TContext> cell, int dataIndex)
        {
            if (loop)
            {
                dataIndex = GetLoopIndex(dataIndex, cellData.Count);
            }
            else if (dataIndex < 0 || dataIndex > cellData.Count - 1)
            {
                // セルに対応するデータが存在しなければセルを表示しない
                cell.SetVisible(false);
                return;
            }

            cell.SetVisible(true);
            cell.DataIndex = dataIndex;
            cell.UpdateContent(cellData[dataIndex]);
        }

        /// <summary>
        /// 円環構造の index を取得します
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        int GetLoopIndex(int index, int length)
        {
            if (index < 0)
            {
                index = (length - 1) + (index + 1) % length;
            }
            else if (index > length - 1)
            {
                index = index % length;
            }
            return index;
        }

        /// <summary>
        /// 表示内容を更新します
        /// </summary>
        protected void UpdateContents()
        {
            UpdatePosition(currentPosition);
        }

        /// <summary>
        /// スクロール位置を更新します
        /// </summary>
        /// <param name="position"></param>
        protected void UpdatePosition(float position)
        {
            currentPosition = position;

            var visibleMinPosition = position - (cellOffset / cellInterval);
            var firstCellPosition = (Mathf.Ceil(visibleMinPosition) - visibleMinPosition) * cellInterval;
            var dataStartIndex = Mathf.CeilToInt(visibleMinPosition);
            var count = 0;
            var cellIndex = 0;

            for (float pos = firstCellPosition; pos <= 1f; pos += cellInterval, count++)
            {
                if (count >= cells.Count)
                {
                    cells.Add(CreateCell());
                }
            }

            count = 0;

            for (float pos = firstCellPosition; pos <= 1f; count++, pos += cellInterval)
            {
                var dataIndex = dataStartIndex + count;
                cellIndex = GetLoopIndex(dataIndex, cells.Count);
                if (cells[cellIndex].gameObject.activeSelf)
                {
                    cells[cellIndex].UpdatePosition(pos);
                }
                UpdateCellForIndex(cells[cellIndex], dataIndex);
            }

            cellIndex = GetLoopIndex(dataStartIndex + count, cells.Count);

            for (; count < cells.Count; count++, cellIndex = GetLoopIndex(dataStartIndex + count, cells.Count))
            {
                cells[cellIndex].SetVisible(false);
            }
        }
    }

    public sealed class FancyScrollViewNullContext
    {

    }

    public class FancyScrollView<TData> : FancyScrollView<TData, FancyScrollViewNullContext>
    {

    }
}
