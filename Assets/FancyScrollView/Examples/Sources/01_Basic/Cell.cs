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

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Message;
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }

            animator.speed = 0;
        }

        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
