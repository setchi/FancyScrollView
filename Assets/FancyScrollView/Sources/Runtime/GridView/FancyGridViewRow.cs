/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using System.Linq;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyGridView{TItemData, TContext}"/> の行を実装するための抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext"><see cref="FancyCell{TItemData, TContext}.Context"/> の型.</typeparam>
    public abstract class FancyGridViewRow<TItemData, TContext> : FancyCell<TItemData[], TContext>
        where TContext : class, IFancyGridViewContext, new()
    {
        /// <summary>
        /// この行で表示するセルの配列.
        /// </summary>
        protected virtual FancyCell<TItemData, TContext>[] Cells { get; private set; }

        /// <summary>
        /// この行で表示するセルの配列をインスタンス化します.
        /// </summary>
        /// <returns>この行で表示するセルの配列.</returns>
        protected virtual FancyCell<TItemData, TContext>[] InstantiateCells()
        {
            return Enumerable.Range(0, Context.GetColumnCount())
                .Select(_ => Instantiate(Context.CellTemplate, transform))
                .Select(x => x.GetComponent<FancyCell<TItemData, TContext>>())
                .ToArray();
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            Cells = InstantiateCells();
            Debug.Assert(Cells.Length == Context.GetColumnCount());

            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].SetContext(Context);
                Cells[i].Initialize();
            }
        }

        /// <inheritdoc/>
        public override void UpdateContent(TItemData[] rowContents)
        {
            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i].Index = i + Index * Context.GetColumnCount();
                Cells[i].SetVisible(i < rowContents.Length);

                if (Cells[i].IsVisible)
                {
                    Cells[i].UpdateContent(rowContents[i]);
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
