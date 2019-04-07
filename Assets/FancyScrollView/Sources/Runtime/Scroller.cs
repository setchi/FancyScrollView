using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EasingCore;

namespace FancyScrollView
{
    public class Scroller : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] RectTransform viewport = default;
        [SerializeField] ScrollDirection directionOfRecognize = ScrollDirection.Vertical;
        [SerializeField] MovementType movementType = MovementType.Elastic;
        [SerializeField] float elasticity = 0.1f;
        [SerializeField] float scrollSensitivity = 1f;
        [SerializeField] bool inertia = true;
        [SerializeField] float decelerationRate = 0.03f;
        [SerializeField] Snap snap = new Snap {
            Enable = true,
            VelocityThreshold = 0.5f,
            Duration = 0.3f,
            Easing = Ease.InOutCubic
        };

        readonly AutoScrollState autoScrollState = new AutoScrollState();

        Action<float> onValueChanged;
        Action<int> onSelectionChanged;

        Vector2 pointerStartLocalPosition;
        float dragStartScrollPosition;
        float prevScrollPosition;
        float currentScrollPosition;

        int totalCount;

        bool dragging;
        float velocity;

        enum ScrollDirection
        {
            Vertical,
            Horizontal,
        }

        public enum MovementType
        {
            Unrestricted = ScrollRect.MovementType.Unrestricted,
            Elastic = ScrollRect.MovementType.Elastic,
            Clamped = ScrollRect.MovementType.Clamped
        }

        [Serializable]
        class Snap
        {
            public bool Enable;
            public float VelocityThreshold;
            public float Duration;
            public Ease Easing;
        }

        readonly static Func<float, float> defaultEasingFunction = EasingFunction.Get(Ease.OutCubic);

        class AutoScrollState
        {
            public bool Enable;
            public bool Elastic;
            public float Duration;
            public Func<float, float> EasingFunction;
            public float StartTime;
            public float EndScrollPosition;

            public void Reset()
            {
                Enable = false;
                Elastic = false;
                Duration = 0f;
                StartTime = 0f;
                EasingFunction = defaultEasingFunction;
                EndScrollPosition = 0f;
            }
        }

        public void OnValueChanged(Action<float> callback) => onValueChanged = callback;

        public void OnSelectionChanged(Action<int> callback) => onSelectionChanged = callback;

        public void SetTotalCount(int totalCount) => this.totalCount = totalCount;

        public void ScrollTo(int index, float duration) => ScrollTo(index, duration, Ease.OutCubic);

        public void ScrollTo(int index, float duration, Ease easing) => ScrollTo(index, duration, EasingFunction.Get(easing));

        public void ScrollTo(int index, float duration, Func<float, float> easingFunction)
        {
            if (duration <= 0f)
            {
                JumpTo(index);
                return;
            }

            autoScrollState.Reset();
            autoScrollState.Enable = true;
            autoScrollState.Duration = duration;
            autoScrollState.EasingFunction = easingFunction ?? defaultEasingFunction;
            autoScrollState.StartTime = Time.unscaledTime;
            autoScrollState.EndScrollPosition = CalculateDestinationIndex(index);

            velocity = 0f;
            dragStartScrollPosition = currentScrollPosition;

            UpdateSelection(Mathf.RoundToInt(CircularPosition(autoScrollState.EndScrollPosition, totalCount)));
        }

        public void JumpTo(int index)
        {
            autoScrollState.Reset();

            velocity = 0f;
            dragging = false;

            index = CalculateDestinationIndex(index);

            UpdateSelection(index);
            UpdatePosition(index);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            pointerStartLocalPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewport,
                eventData.position,
                eventData.pressEventCamera,
                out pointerStartLocalPosition);

            dragStartScrollPosition = currentScrollPosition;
            dragging = true;
            autoScrollState.Reset();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (!dragging)
            {
                return;
            }

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewport,
                eventData.position,
                eventData.pressEventCamera,
                out localCursor))
            {
                return;
            }

            var pointerDelta = localCursor - pointerStartLocalPosition;
            var position = (directionOfRecognize == ScrollDirection.Horizontal ? -pointerDelta.x : pointerDelta.y)
                           / ViewportSize
                           * scrollSensitivity
                           + dragStartScrollPosition;

            var offset = CalculateOffset(position);
            position += offset;

            if (movementType == MovementType.Elastic)
            {
                if (offset != 0f)
                {
                    position -= RubberDelta(offset, scrollSensitivity);
                }
            }

            UpdatePosition(position);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            dragging = false;
        }

        float ViewportSize => directionOfRecognize == ScrollDirection.Horizontal
            ? viewport.rect.size.x
            : viewport.rect.size.y;

        float CalculateOffset(float position)
        {
            if (movementType == MovementType.Unrestricted)
            {
                return 0f;
            }

            if (position < 0f)
            {
                return -position;
            }

            if (position > totalCount - 1)
            {
                return totalCount - 1 - position;
            }

            return 0f;
        }

        void UpdatePosition(float position)
        {
            currentScrollPosition = position;
            onValueChanged?.Invoke(currentScrollPosition);
        }

        void UpdateSelection(int index) => onSelectionChanged?.Invoke(index);

        float RubberDelta(float overStretching, float viewSize) =>
            (1 - 1 / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1)) * viewSize * Mathf.Sign(overStretching);

        void Update()
        {
            var deltaTime = Time.unscaledDeltaTime;
            var offset = CalculateOffset(currentScrollPosition);

            if (autoScrollState.Enable)
            {
                var position = 0f;

                if (autoScrollState.Elastic)
                {
                    position = Mathf.SmoothDamp(currentScrollPosition, currentScrollPosition + offset, ref velocity,
                        elasticity, Mathf.Infinity, deltaTime);

                    if (Mathf.Abs(velocity) < 0.01f)
                    {
                        position = Mathf.Clamp(Mathf.RoundToInt(position), 0, totalCount - 1);
                        velocity = 0f;
                        autoScrollState.Reset();
                    }
                }
                else
                {
                    var alpha = Mathf.Clamp01((Time.unscaledTime - autoScrollState.StartTime) /
                                              Mathf.Max(autoScrollState.Duration, float.Epsilon));
                    position = Mathf.LerpUnclamped(dragStartScrollPosition, autoScrollState.EndScrollPosition,
                        autoScrollState.EasingFunction(alpha));

                    if (Mathf.Approximately(alpha, 1f))
                    {
                        autoScrollState.Reset();
                    }
                }

                UpdatePosition(position);
            }
            else if (!dragging && (!Mathf.Approximately(offset, 0f) || !Mathf.Approximately(velocity, 0f)))
            {
                var position = currentScrollPosition;

                if (movementType == MovementType.Elastic && !Mathf.Approximately(offset, 0f))
                {
                    autoScrollState.Reset();
                    autoScrollState.Enable = true;
                    autoScrollState.Elastic = true;

                    UpdateSelection(Mathf.Clamp(Mathf.RoundToInt(position), 0, totalCount - 1));
                }
                else if (inertia)
                {
                    velocity *= Mathf.Pow(decelerationRate, deltaTime);

                    if (Mathf.Abs(velocity) < 0.001f)
                    {
                        velocity = 0f;
                    }

                    position += velocity * deltaTime;

                    if (snap.Enable && Mathf.Abs(velocity) < snap.VelocityThreshold)
                    {
                        ScrollTo(Mathf.RoundToInt(currentScrollPosition), snap.Duration, snap.Easing);
                    }
                }
                else
                {
                    velocity = 0f;
                }

                if (!Mathf.Approximately(velocity, 0f))
                {
                    if (movementType == MovementType.Clamped)
                    {
                        offset = CalculateOffset(position);
                        position += offset;

                        if (Mathf.Approximately(position, 0f) || Mathf.Approximately(position, totalCount - 1f))
                        {
                            velocity = 0f;
                            UpdateSelection(Mathf.RoundToInt(position));
                        }
                    }

                    UpdatePosition(position);
                }
            }

            if (!autoScrollState.Enable && dragging && inertia)
            {
                var newVelocity = (currentScrollPosition - prevScrollPosition) / deltaTime;
                velocity = Mathf.Lerp(velocity, newVelocity, deltaTime * 10f);
            }

            prevScrollPosition = currentScrollPosition;
        }

        int CalculateDestinationIndex(int index) => movementType == MovementType.Unrestricted
            ? CalculateClosestIndex(index)
            : Mathf.Clamp(index, 0, totalCount - 1);

        int CalculateClosestIndex(int index)
        {
            var diff = CircularPosition(index, totalCount)
                       - CircularPosition(currentScrollPosition, totalCount);

            if (Mathf.Abs(diff) > totalCount * 0.5f)
            {
                diff = Mathf.Sign(-diff) * (totalCount - Mathf.Abs(diff));
            }

            return Mathf.RoundToInt(diff + currentScrollPosition);
        }

        float CircularPosition(float p, int size) => size < 1 ? 0 : p < 0 ? size - 1 + (p + 1) % size : p % size;
    }
}
