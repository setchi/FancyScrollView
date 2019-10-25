using System;
using EasingCore;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example06
{
    public class SlideScreenTransition : MonoBehaviour
    {
        [SerializeField] bool isOutAnimation;
        [SerializeField] RectTransform targetTransform = default;
        [SerializeField] GraphicRaycaster graphicRaycaster = default;

        const float Duration = 0.3f; // example purpose, a fixed number, the same with scroll view duration

        bool shouldAnimate;
        float timer = 0f;
        float startX;
        float endX;

        public void Animate(Scroller.MovementDirection direction)
        {
            if (shouldAnimate)
            {
                return;
            }

            startX = 0;
            graphicRaycaster.enabled = false;
            timer = Duration;

            switch (direction)
            {
                case Scroller.MovementDirection.Left:
                    endX = -targetTransform.rect.width;
                    break;
                case Scroller.MovementDirection.Right:
                    endX = targetTransform.rect.width;
                    break;
                default:
                    Debug.LogWarning("example only support horizontal direction");
                    break;
            }

            if (!isOutAnimation)
            {
                gameObject.SetActive(true);
            }

            startX = isOutAnimation ? 0 : -endX;
            endX = isOutAnimation ? endX : 0;
            targetTransform.anchoredPosition = new Vector2(startX, targetTransform.anchoredPosition.y);
            shouldAnimate = true;
        }

        void Update()
        {
            if (shouldAnimate)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    var x = Mathf.Lerp(endX, startX, timer / Duration);

                    if (timer <= 0)
                    {
                        shouldAnimate = false;
                        graphicRaycaster.enabled = true;
                        x = endX;

                        if (isOutAnimation)
                        {
                            gameObject.SetActive(false);
                        }
                    }

                    targetTransform.anchoredPosition = new Vector2(x, targetTransform.anchoredPosition.y);
                }
            }
        }
    }
}
