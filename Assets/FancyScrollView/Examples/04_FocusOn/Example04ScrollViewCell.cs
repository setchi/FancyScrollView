using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class Example04ScrollViewCell
        : FancyScrollViewCell<Example04CellDto, Example04ScrollViewContext>
    {
        [SerializeField]
        Animator animator;
        [SerializeField]
        Text message;
        [SerializeField]
        Image image;
        [SerializeField]
        Button button;

        static readonly int scrollTriggerHash = Animator.StringToHash("scroll");

        void Start()
        {
            UpdatePosition(0);
            button.onClick.AddListener(OnPressedCell);
        }

        /// <summary>
        /// セルの内容を更新します
        /// </summary>
        /// <param name="itemData"></param>
        public override void UpdateContent(Example04CellDto itemData)
        {
            message.text = itemData.Message;

            if (Context != null)
            {
                var isSelected = Context.SelectedIndex == DataIndex;
                image.color = isSelected
                    ? new Color32(0, 255, 255, 100)
                    : new Color32(255, 255, 255, 77);
            }
        }

        /// <summary>
        /// セルの位置を更新します
        /// </summary>
        /// <param name="position"></param>
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

        void OnPressedCell()
        {
            if (Context != null)
            {
                Context.OnPressedCell(this);
            }
        }
    }
}
