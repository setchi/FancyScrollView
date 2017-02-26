using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollViewExamples
{
    public class Example01ScrollViewCell : FancyScrollViewCell<Example01CellDto>
    {
        [SerializeField]
        Animator animator;
        [SerializeField]
        Text message;

        readonly int scrollTriggerHash = Animator.StringToHash("scroll");

        void Start()
        {
            var rectTransform = transform as RectTransform;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchoredPosition3D = Vector3.zero;
            UpdatePosition(0);
        }

        /// <summary>
        /// セルの内容を更新します
        /// </summary>
        /// <param name="itemData"></param>
        public override void UpdateContent(Example01CellDto itemData)
        {
            message.text = itemData.Message;
        }

        /// <summary>
        /// セルの位置を更新します
        /// </summary>
        /// <param name="position"></param>
        public override void UpdatePosition(float position)
        {
            animator.Play(scrollTriggerHash, -1, position);
            animator.speed = 0;
        }
    }
}
