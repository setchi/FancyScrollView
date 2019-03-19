using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollViewCell<TCellData, TContext> : MonoBehaviour where TContext : class, new()
    {
        /// <summary>
        /// Gets or sets the index of the data.
        /// </summary>
        /// <value>The index of the data.</value>
        public int DataIndex { get; set; } = -1;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:FancyScrollView.FancyScrollViewCell`2"/> is visible.
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        public virtual bool IsVisible => gameObject.activeSelf;

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        protected TContext Context { get; private set; }

        /// <summary>
        /// Sets the context.
        /// </summary>
        /// <param name="context">Context.</param>
        public virtual void SetContext(TContext context) => Context = context;

        /// <summary>
        /// Sets the visible.
        /// </summary>
        /// <param name="visible">If set to <c>true</c> visible.</param>
        public virtual void SetVisible(bool visible) => gameObject.SetActive(visible);

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="cellData">Cell data.</param>
        public abstract void UpdateContent(TCellData cellData);

        /// <summary>
        /// Updates the position.
        /// </summary>
        /// <param name="position">Position.</param>
        public abstract void UpdatePosition(float position);
    }

    public abstract class FancyScrollViewCell<TCellData> : FancyScrollViewCell<TCellData, FancyScrollViewNullContext>
    {
        public sealed override void SetContext(FancyScrollViewNullContext context) => base.SetContext(context);
    }
}
