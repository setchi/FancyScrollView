using System;
using EasingCore;
using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example06
{
    public class SlideScreenTransition : MonoBehaviour
    {
        [SerializeField] protected bool isOutAnimation;
        [SerializeField] private RectTransform targetTransform = default;
        [SerializeField] private GraphicRaycaster graphicRaycaster = default;

        private const float Duration = 0.3f; // example purpose, a fixed number, the same with scroll view duration
        
        private bool _shouldAnimate;
        private float _timer = 0f;
        private float _startX;
        private float _endX;

        public void Animate(Scroller.MovementDirection direction) {
            if (_shouldAnimate)
            {
                return;
            }
            _startX = 0;
            graphicRaycaster.enabled = false;
            _timer = Duration;

            switch (direction) {
                case Scroller.MovementDirection.Left:
                    _endX = -targetTransform.rect.width;
                    break;
                case Scroller.MovementDirection.Right:
                    _endX = targetTransform.rect.width;
                    break;
                default:
                    Debug.LogWarning("example only support horizontal direction");
                    break;
            }

            if (!isOutAnimation)
            {
                gameObject.SetActive(true);
            }
            
            _startX = isOutAnimation ? 0 : -_endX;
            _endX = isOutAnimation ? _endX : 0;
            targetTransform.anchoredPosition = new Vector2(_startX, targetTransform.anchoredPosition.y);
            _shouldAnimate = true;
        }

        private void Update()
        {
            if (_shouldAnimate)
            {
                if (_timer > 0)
                {
                    _timer -= Time.deltaTime;
                    var x = Mathf.Lerp(_endX, _startX, _timer / Duration);
                    if (_timer <= 0)
                    {
                        _shouldAnimate = false;
                        graphicRaycaster.enabled = true;
                        x = _endX;
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