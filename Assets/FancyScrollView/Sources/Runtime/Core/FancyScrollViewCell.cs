using UnityEngine;

namespace FancyScrollView
{
    /// <summary>
    /// <see cref="FancyScrollView{TItemData, TContext}"/> のセルを実装するための抽象基底クラス.
    /// <see cref="FancyScrollViewCell{TItemData, TContext}.Context"/> が不要な場合は
    /// 代わりに <see cref="FancyScrollViewCell{TItemData}"/> を使用します.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    /// <typeparam name="TContext">コンテキストの型.</typeparam>
    public abstract class FancyScrollViewCell<TItemData, TContext> : MonoBehaviour where TContext : class, new()
    {
        /// <summary>
        /// このセルで表示しているデータのインデックス.
        /// </summary>
        public int Index { get; set; } = -1;

        /// <summary>
        /// このセルの可視状態.
        /// </summary>
        public virtual bool IsVisible => gameObject.activeSelf;

        /// <summary>
        /// <see cref="FancyScrollView{TItemData, TContext}.Context"/> の参照.
        /// セルとスクロールビュー間で同じインスタンスが共有されます. 情報の受け渡しや状態の保持に使用します.
        /// </summary>
        protected TContext Context { get; private set; }

        /// <summary>
        /// <see cref="Context"/> のセットアップを行います.
        /// </summary>
        /// <param name="context">コンテキスト.</param>
        public virtual void SetupContext(TContext context) => Context = context;

        /// <summary>
        /// このセルの可視状態を設定します.
        /// </summary>
        /// <param name="visible">可視状態なら true, 非可視状態なら false.</param>
        public virtual void SetVisible(bool visible) => gameObject.SetActive(visible);

        /// <summary>
        /// アイテムデータに基づいてこのセルの表示内容を更新します.
        /// </summary>
        /// <param name="itemData">アイテムデータ.</param>
        public abstract void UpdateContent(TItemData itemData);

        /// <summary>
        /// 0.0 ~ 1.0 の値に基づいてこのセルのスクロール位置を更新します.
        /// </summary>
        /// <param name="position">正規化されたスクロール位置.</param>
        public abstract void UpdatePosition(float position);
    }

    /// <summary>
    /// <see cref="FancyScrollView{TItemData}"/> のセルを実装するための抽象基底クラス.
    /// </summary>
    /// <typeparam name="TItemData">アイテムのデータ型.</typeparam>
    public abstract class FancyScrollViewCell<TItemData> : FancyScrollViewCell<TItemData, FancyScrollViewNullContext>
    {
        /// <inheritdoc/>
        public sealed override void SetupContext(FancyScrollViewNullContext context) => base.SetupContext(context);
    }
}
