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
        [SerializeField] ScrollDirection scrollDirection = ScrollDirection.Vertical;
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
        [SerializeField] bool draggable = true;
        [SerializeField] Scrollbar scrollbar = default;

        public ScrollDirection ScrollDirection => scrollDirection;
        public RectTransform Viewport => viewport;
        public bool SnapEnabled
        {
            get => snap.Enable;
            set => snap.Enable = value;
        }
        public float ScrollSensitivity
        {
            get => scrollSensitivity;
            set => scrollSensitivity = value;
        }
        public bool Draggable
        {
            get => draggable;
            set => draggable = value;
        }
        public Scrollbar Scrollbar => scrollbar;

        readonly AutoScrollState autoScrollState = new AutoScrollState();

        Action<float> onValueChanged;
        Action<int> onSelectionChanged;

        Vector2 beginDragPointerPosition;
        float scrollStartPosition;
        float prevPosition;
        float currentPosition;

        int totalCount;

        bool dragging;
        float velocity;

        [Serializable]
        class Snap
        {
            public bool Enable;
            public float VelocityThreshold;
            public float Duration;
            public Ease Easing;
        }

        static readonly Func<float, float> DefaultEasingFunction = EasingFunction.Get(Ease.OutCubic);

        class AutoScrollState
        {
            public bool Enable;
            public bool Elastic;
            public float Duration;
            public Func<float, float> EasingFunction;
            public float StartTime;
            public float EndPosition;

            public Action OnComplete;

            public void Reset()
            {
                Enable = false;
                Elastic = false;
                Duration = 0f;
                StartTime = 0f;
                EasingFunction = DefaultEasingFunction;
                EndPosition = 0f;
                OnComplete = null;
            }

            public void Complete()
            {
                OnComplete?.Invoke();
                Reset();
            }
        }

        protected override void Start()
        {
            base.Start();

            if (scrollbar)
            {
                scrollbar.onValueChanged.AddListener(x => ValueChanged(x * (totalCount - 1f), false));
            }
        }

        public void OnValueChanged(Action<float> callback) => onValueChanged = callback;

        public void OnSelectionChanged(Action<int> callback) => onSelectionChanged = callback;

        public void SetTotalCount(int totalCount) => this.totalCount = totalCount;

        public void ScrollTo(int index, float duration, Action onComplete = null) => ScrollTo(index, duration, Ease.OutCubic, onComplete);

        public void ScrollTo(int index, float duration, Ease easing, Action onComplete = null) => ScrollTo(index, duration, EasingFunction.Get(easing), onComplete);

        public void ScrollTo(int index, float duration, Func<float, float> easingFunction, Action onComplete = null)
        {
            if (duration <= 0f)
            {
                JumpTo(CircularIndex(index, totalCount));
                return;
            }

            autoScrollState.Reset();
            autoScrollState.Enable = true;
            autoScrollState.Duration = duration;
            autoScrollState.EasingFunction = easingFunction ?? DefaultEasingFunction;
            autoScrollState.StartTime = Time.unscaledTime;
            autoScrollState.EndPosition = Mathf.Round(currentPosition + CalculateMovementAmount(currentPosition, index));
            autoScrollState.OnComplete = onComplete;

            velocity = 0f;
            scrollStartPosition = currentPosition;

            SelectionChanged(Mathf.RoundToInt(CircularPosition(autoScrollState.EndPosition, totalCount)));
        }

        public void JumpTo(int index)
        {
            if (index < 0 || index > totalCount - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            autoScrollState.Reset();

            velocity = 0f;
            dragging = false;

            SelectionChanged(index);
            ValueChanged(index);
        }

        public void UpdatePosition(float position)
        {
            autoScrollState.Reset();

            velocity = 0f;
            dragging = false;

            ValueChanged(position);
        }

        public MovementDirection GetMovementDirection(int sourceIndex, int destIndex)
        {
            var movementAmount = CalculateMovementAmount(sourceIndex, destIndex);
            return scrollDirection == ScrollDirection.Horizontal
                ? movementAmount > 0
                    ? MovementDirection.Left
                    : MovementDirection.Right
                : movementAmount > 0
                    ? MovementDirection.Up
                    : MovementDirection.Down;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewport,
                eventData.position,
                eventData.pressEventCamera,
                out beginDragPointerPosition);

            scrollStartPosition = currentPosition;
            dragging = true;
            autoScrollState.Reset();
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left || !dragging)
            {
                return;
            }

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                viewport,
                eventData.position,
                eventData.pressEventCamera,
                out var dragPointerPosition))
            {
                return;
            }

            var pointerDelta = dragPointerPosition - beginDragPointerPosition;
            var position = (scrollDirection == ScrollDirection.Horizontal ? -pointerDelta.x : pointerDelta.y)
                           / ViewportSize
                           * scrollSensitivity
                           + scrollStartPosition;

            var offset = CalculateOffset(position);
            position += offset;

            if (movementType == MovementType.Elastic)
            {
                if (offset != 0f)
                {
                    position -= RubberDelta(offset, scrollSensitivity);
                }
            }

            ValueChanged(position);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            dragging = false;
        }

        float ViewportSize => scrollDirection == ScrollDirection.Horizontal
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

        void ValueChanged(float position, bool updateScrollbar = true)
        {
            onValueChanged?.Invoke(currentPosition = position);

            if (scrollbar && updateScrollbar)
            {
                scrollbar.value = Mathf.Clamp01(position / Mathf.Max(totalCount - 1f, 1e-4f));
            }
        }

        void SelectionChanged(int index) => onSelectionChanged?.Invoke(index);

        float RubberDelta(float overStretching, float viewSize) =>
            (1 - 1 / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1)) * viewSize * Mathf.Sign(overStretching);

        void Update()
        {
            var deltaTime = Time.unscaledDeltaTime;
            var offset = CalculateOffset(currentPosition);

            if (autoScrollState.Enable)
            {
                var position = 0f;

                if (autoScrollState.Elastic)
                {
                    position = Mathf.SmoothDamp(currentPosition, currentPosition + offset, ref velocity,
                        elasticity, Mathf.Infinity, deltaTime);

                    if (Mathf.Abs(velocity) < 0.01f)
                    {
                        position = Mathf.Clamp(Mathf.RoundToInt(position), 0, totalCount - 1);
                        velocity = 0f;
                        autoScrollState.Complete();
                    }
                }
                else
                {
                    var alpha = Mathf.Clamp01((Time.unscaledTime - autoScrollState.StartTime) /
                                               Mathf.Max(autoScrollState.Duration, float.Epsilon));
                    position = Mathf.LerpUnclamped(scrollStartPosition, autoScrollState.EndPosition,
                        autoScrollState.EasingFunction(alpha));

                    if (Mathf.Approximately(alpha, 1f))
                    {
                        autoScrollState.Complete();
                    }
                }

                ValueChanged(position);
            }
            else if (!dragging && (!Mathf.Approximately(offset, 0f) || !Mathf.Approximately(velocity, 0f)))
            {
                var position = currentPosition;

                if (movementType == MovementType.Elastic && !Mathf.Approximately(offset, 0f))
                {
                    autoScrollState.Reset();
                    autoScrollState.Enable = true;
                    autoScrollState.Elastic = true;

                    SelectionChanged(Mathf.Clamp(Mathf.RoundToInt(position), 0, totalCount - 1));
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
                        ScrollTo(Mathf.RoundToInt(currentPosition), snap.Duration, snap.Easing);
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
                            SelectionChanged(Mathf.RoundToInt(position));
                        }
                    }

                    ValueChanged(position);
                }
            }

            if (!autoScrollState.Enable && dragging && inertia)
            {
                var newVelocity = (currentPosition - prevPosition) / deltaTime;
                velocity = Mathf.Lerp(velocity, newVelocity, deltaTime * 10f);
            }

            prevPosition = currentPosition;
        }

        float CalculateMovementAmount(float sourcePosition, float destPosition)
        {
            if (movementType != MovementType.Unrestricted)
            {
                return Mathf.Clamp(destPosition, 0, totalCount - 1) - sourcePosition;
            }

            var movementAmount = CircularPosition(destPosition, totalCount) - CircularPosition(sourcePosition, totalCount);

            if (Mathf.Abs(movementAmount) > totalCount * 0.5f)
            {
                movementAmount = Mathf.Sign(-movementAmount) * (totalCount - Mathf.Abs(movementAmount));
            }

            return movementAmount;
        }

        float CircularPosition(float p, int size) => size < 1 ? 0 : p < 0 ? size - 1 + (p + 1) % size : p % size;

        int CircularIndex(int i, int size) => size < 1 ? 0 : i < 0 ? size - 1 + (i + 1) % size : i % size;
    }
}
