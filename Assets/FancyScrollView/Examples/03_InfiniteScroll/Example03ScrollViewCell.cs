using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class Example03ScrollViewCell : FancyScrollViewCell<Example03CellData, Example03ScrollViewContext>
    {
        [SerializeField] Animator animator;
        [SerializeField] Text message;
        [SerializeField] Image image;
        [SerializeField] Button button;

        static readonly int ScrollTriggerHash = Animator.StringToHash("scroll");

        void Start()
        {
            var rectTransform = transform as RectTransform;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchoredPosition3D = Vector3.zero;

            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(DataIndex));
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="cellData">Cell data.</param>
        public override void UpdateContent(Example03CellData cellData)
        {
            message.text = cellData.Message;

            var isSelected = Context.SelectedIndex == DataIndex;
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
