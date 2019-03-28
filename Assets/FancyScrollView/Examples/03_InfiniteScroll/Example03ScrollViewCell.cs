using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class Example03ScrollViewCell : FancyScrollViewCell<Example03ItemData, Example03ScrollViewContext>
    {
        [SerializeField] Animator animator;
        [SerializeField] Text message;
        [SerializeField] Image image;
        [SerializeField] Button button;

        static class AnimatorHash
        {
            internal static readonly int Scroll = Animator.StringToHash("scroll");
        }

        void Start()
        {
            var rectTransform = transform as RectTransform;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchoredPosition3D = Vector3.zero;

            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(ItemIndex));
        }

        public override void UpdateContent(Example03ItemData itemData)
        {
            message.text = itemData.Message;

            var selected = Context.SelectedIndex == ItemIndex;
            image.color = selected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
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
