/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example06
{
    class SlideScreenTransition : MonoBehaviour
    {
        [SerializeField] RectTransform targetTransform = default;
        [SerializeField] GraphicRaycaster graphicRaycaster = default;

        const float Duration = 0.3f; // example purpose, a fixed number, the same with scroll view duration

        bool shouldAnimate, isOutAnimation;
        float timer, startX, endX;

        public void In(MovementDirection direction) => Animate(direction, false);

        public void Out(MovementDirection direction) => Animate(direction, true);

        void Animate(MovementDirection direction, bool isOut)
        {
            if (shouldAnimate)
            {
                return;
            }

            timer = Duration;
            isOutAnimation = isOut;
            shouldAnimate = true;
            graphicRaycaster.enabled = false;

            if (!isOutAnimation)
            {
                gameObject.SetActive(true);
            }

            switch (direction)
            {
                case MovementDirection.Left:
                    endX = -targetTransform.rect.width;
                    break;

                case MovementDirection.Right:
                    endX = targetTransform.rect.width;
                    break;

                default:
                    Debug.LogWarning("Example only support horizontal direction.");
                    break;
            }

            startX = isOutAnimation ? 0 : -endX;
            endX = isOutAnimation ? endX : 0;

            UpdatePosition(0f);
        }

        void Update()
        {
            if (!shouldAnimate)
            {
                return;
            }

            timer -= Time.deltaTime;

            if (timer > 0)
            {
                UpdatePosition(1f - timer / Duration);
                return;
            }

            shouldAnimate = false;
            graphicRaycaster.enabled = true;

            if (isOutAnimation)
            {
                gameObject.SetActive(false);
            }

            UpdatePosition(1f);
        }

        void UpdatePosition(float position)
        {
            var x = Mathf.Lerp(startX, endX, position);
            targetTransform.anchoredPosition = new Vector2(x, targetTransform.anchoredPosition.y);
        }
    }
}
