using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example01
{
    public class Cell : FancyScrollViewCell<ItemData>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        void Start()
        {
            animator.keepAnimatorControllerStateOnDisable = true;
        }

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Message;
        }

        public override void UpdatePosition(float position)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
            animator.speed = 0;
        }
    }
}
