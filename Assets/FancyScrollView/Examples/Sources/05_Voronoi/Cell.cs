using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example05
{
    [ExecuteInEditMode]
    public class Cell : FancyScrollViewCell<ItemData, Context>
    {
        [SerializeField] Animator scrollAnimator = default;
        [SerializeField] Animator selectAnimator = default;
        [SerializeField] Text message = default;
        [SerializeField] Image image = default;
        [SerializeField] Button button = default;
        [SerializeField] RectTransform rectTransform = default;
        [SerializeField, HideInInspector] Vector3 position = default;

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
            public static readonly int In = Animator.StringToHash("in");
            public static readonly int Out = Animator.StringToHash("out");
        }

        float hash;
        bool currentSelection;
        float currentPosition;
        float updateSelectionTime;

        void Start()
        {
            hash = Random.value * 100f;
            scrollAnimator.keepAnimatorControllerStateOnDisable = true;
            selectAnimator.keepAnimatorControllerStateOnDisable = true;

            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        }

        void LateUpdate()
        {
            image.rectTransform.localPosition = position + GetFluctuation();
        }

        Vector3 GetFluctuation()
        {
            var fluctX = Mathf.Sin(Time.time + hash * 40) * 10;
            var fluctY = Mathf.Sin(Time.time + hash) * 10;
            return new Vector3(fluctX, fluctY, 0f);
        }

        public override void SetupContext(Context context)
        {
            base.SetupContext(context);

            Context.UpdateCellState += () =>
            {
                var siblingIndex = rectTransform.GetSiblingIndex();
                var t = Mathf.Clamp01((Time.time - updateSelectionTime) * (1f / 0.3f));
                var selectAnimation = currentSelection ? t : 1f - t;
                var position = IsVisible
                    ? this.position + GetFluctuation()
                    : Vector3.left * rectTransform.rect.size.x * 10f;

                Context.SetCellState(siblingIndex, Index, position.x, position.y, selectAnimation);
            };
        }

        public override void UpdateContent(ItemData cellData)
        {
            message.text = cellData.Message;
            SetSelection(Context.SelectedIndex == Index);
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;
            scrollAnimator.Play(AnimatorHash.Scroll, -1, position);
            scrollAnimator.speed = 0;
        }

        void SetSelection(bool selected)
        {
            if (currentSelection == selected)
            {
                return;
            }

            currentSelection = selected;
            selectAnimator.SetTrigger(selected ? AnimatorHash.In : AnimatorHash.Out);
            updateSelectionTime = Time.time;
        }
    }
}
