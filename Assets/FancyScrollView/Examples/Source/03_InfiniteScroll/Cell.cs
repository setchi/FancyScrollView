using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example03
{
    public class Cell : FancyScrollViewCell<ItemData, Context>
    {
        [SerializeField] Animator animator;
        [SerializeField] Text message;
        [SerializeField] Text messageLarge;
        [SerializeField] Image image;
        [SerializeField] Image imageLarge;
        [SerializeField] Button button;

        static class AnimatorHash
        {
            internal static readonly int Scroll = Animator.StringToHash("scroll");
        }

        void Start()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        }

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Message;
            messageLarge.text = Index.ToString();

            var selected = Context.SelectedIndex == Index;
            imageLarge.color = image.color = selected
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
