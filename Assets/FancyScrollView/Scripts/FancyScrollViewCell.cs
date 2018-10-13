using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollViewCell<TData, TContext> : MonoBehaviour where TContext : class
    {
        /// <summary>
        /// Gets or sets the index of the data.
        /// </summary>
        /// <value>The index of the data.</value>
        public int DataIndex { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:FancyScrollView.FancyScrollViewCell`2"/> is visible.
        /// </summary>
        /// <value><c>true</c> if is visible; otherwise, <c>false</c>.</value>
        public virtual bool IsVisible { get { return gameObject.activeSelf; } }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        protected TContext Context { get; private set; }

        /// <summary>
        /// Sets the context.
        /// </summary>
        /// <param name="context">Context.</param>
        public virtual void SetContext(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Sets the visible.
        /// </summary>
        /// <param name="visible">If set to <c>true</c> visible.</param>
        public virtual void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="itemData">Item data.</param>
        public virtual void UpdateContent(TData itemData)
        {
        }

        /// <summary>
        /// Updates the position.
        /// </summary>
        /// <param name="position">Position.</param>
        public virtual void UpdatePosition(float position)
        {
        }
    }

    public abstract class FancyScrollViewCell<TData> : FancyScrollViewCell<TData, FancyScrollViewNullContext>
    {
    }
}
