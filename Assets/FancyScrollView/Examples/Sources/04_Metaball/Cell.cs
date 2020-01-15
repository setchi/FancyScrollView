/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example04
{
    [ExecuteInEditMode]
    class Cell : FancyCell<ItemData, Context>
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

        public override void Initialize()
        {
            hash = Random.value * 100f;
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));

            Context.UpdateCellState += () =>
            {
                var siblingIndex = rectTransform.GetSiblingIndex();
                var scale = Mathf.Min(1f, 10 * (0.5f - Mathf.Abs(currentPosition - 0.5f)));
                var position = IsVisible
                    ? this.position + GetFluctuation()
                    : rectTransform.rect.size.x * 10f * Vector3.left;

                Context.SetCellState(siblingIndex, Index, position.x, position.y, scale);
            };
        }

        void LateUpdate()
        {
            image.rectTransform.localPosition = position + GetFluctuation();
        }

        Vector3 GetFluctuation()
        {
            var fluctX = Mathf.Sin(Time.time + hash * 40) * 12;
            var fluctY = Mathf.Sin(Time.time + hash) * 12;
            return new Vector3(fluctX, fluctY, 0f);
        }

        public override void UpdateContent(ItemData cellData)
        {
            message.text = cellData.Message;
            SetSelection(Context.SelectedIndex == Index);
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;

            if (scrollAnimator.isActiveAndEnabled)
            {
                scrollAnimator.Play(AnimatorHash.Scroll, -1, position);
            }

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
        }

        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
