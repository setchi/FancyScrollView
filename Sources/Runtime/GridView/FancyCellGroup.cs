/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using System.Linq;

namespace FancyScrollView
{
    /// <summary>
    /// 複数の <see cref="FancyCell{TItemData, TContext}"/> を持つセルグループ実装するための抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext"><see cref="FancyCell{TItemData, TContext}.Context"/> の型.</typeparam>
    public abstract class FancyCellGroup<TItemData, TContext> : FancyCell<TItemData[], TContext>
        where TContext : class, IFancyCellGroupContext, new()
    {
        /// <summary>
        /// このグループで表示するセルの配列.
        /// </summary>
        protected virtual FancyCell<TItemData, TContext>[] Cells { get; private set; }

        /// <summary>
        /// このグループで表示するセルの配列をインスタンス化します.
        /// </summary>
        /// <returns>このグループで表示するセルの配列.</returns>
        protected virtual FancyCell<TItemData, TContext>[] InstantiateCells()
        {
            return Enumerable.Range(0, Context.GetGroupCount())
                .Select(_ => Instantiate(Context.CellTemplate, transform))
                .Select(x => x.GetComponent<FancyCell<TItemData, TContext>>())
                .ToArray();
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            Cells = InstantiateCells();
            Debug.Assert(Cells.Length == Context.GetGroupCount());

            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].SetContext(Context);
                Cells[i].Initialize();
            }
        }

        /// <inheritdoc/>
        public override void UpdateContent(TItemData[] contents)
        {
            var firstCellIndex = Index * Context.GetGroupCount();

            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].Index = i + firstCellIndex;
                Cells[i].SetVisible(i < contents.Length);

                if (Cells[i].IsVisible)
                {
                    Cells[i].UpdateContent(contents[i]);
                }
            }
        }

        /// <inheritdoc/>
        public override void UpdatePosition(float position)
        {
            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].UpdatePosition(position);
            }
        }
    }
}
