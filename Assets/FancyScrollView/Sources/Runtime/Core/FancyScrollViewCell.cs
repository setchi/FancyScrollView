using UnityEngine;

namespace FancyScrollView
{
    public abstract class FancyScrollViewCell<TItemData, TContext> : MonoBehaviour where TContext : class, new()
    {
        public int Index { get; set; } = -1;

        public virtual bool IsVisible => gameObject.activeSelf;

        protected TContext Context { get; private set; }

        public virtual void SetupContext(TContext context) => Context = context;

        public virtual void SetVisible(bool visible) => gameObject.SetActive(visible);

        public abstract void UpdateContent(TItemData itemData);

        public abstract void UpdatePosition(float position);
    }

    public abstract class FancyScrollViewCell<TItemData> : FancyScrollViewCell<TItemData, FancyScrollViewNullContext>
    {
        public sealed override void SetupContext(FancyScrollViewNullContext context) => base.SetupContext(context);
    }
}
