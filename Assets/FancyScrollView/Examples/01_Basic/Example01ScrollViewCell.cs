using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class Example01ScrollViewCell : FancyScrollViewCell<Example01CellDto>
    {
        [SerializeField]
        Animator animator;
        [SerializeField]
        Text message;

        static readonly int scrollTriggerHash = Animator.StringToHash("scroll");

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="itemData">Item data.</param>
        public override void UpdateContent(Example01CellDto itemData)
        {
            message.text = itemData.Message;
        }

        /// <summary>
        /// Updates the position.
        /// </summary>
        /// <param name="position">Position.</param>
        public override void UpdatePosition(float position)
        {
            currentPosition = position;
            animator.Play(scrollTriggerHash, -1, position);
            animator.speed = 0;
        }

        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        float currentPosition = 0;
        void OnEnable()
        {
            UpdatePosition(currentPosition);
        }
    }
}
