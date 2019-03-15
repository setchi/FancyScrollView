using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class Example04ScrollViewCell : FancyScrollViewCell<Example04CellData, Example04ScrollViewContext>
    {
        [SerializeField] Animator animator;
        [SerializeField] Text message;
        [SerializeField] Image image;
        [SerializeField] Button button;

        static readonly int ScrollTriggerHash = Animator.StringToHash("scroll");

        void Start() => button.onClick.AddListener(() => Context?.OnCellClicked?.Invoke(DataIndex));

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="cellData">Cell data.</param>
        public override void UpdateContent(Example04CellData cellData)
        {
            message.text = cellData.Message;

            var isSelected = (Context?.SelectedIndex ?? -1) == DataIndex;
            image.color = isSelected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
        }

        /// <summary>
        /// Updates the position.
        /// </summary>
        /// <param name="position">Position.</param>
        public override void UpdatePosition(float position)
        {
            currentPosition = position;
            animator.Play(ScrollTriggerHash, -1, position);
            animator.speed = 0;
        }

        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
