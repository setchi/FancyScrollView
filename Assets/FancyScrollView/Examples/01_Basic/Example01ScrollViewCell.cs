using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class Example01ScrollViewCell : FancyScrollViewCell<Example01ItemData>
    {
        [SerializeField] Animator animator;
        [SerializeField] Text message;

        static class AnimatorHash
        {
            internal static readonly int Scroll = Animator.StringToHash("scroll");
        }

        public override void UpdateContent(Example01ItemData itemData)
        {
            message.text = itemData.Message;
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;
            animator.Play(AnimatorHash.Scroll, -1, position);
            animator.speed = 0;
        }

        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
