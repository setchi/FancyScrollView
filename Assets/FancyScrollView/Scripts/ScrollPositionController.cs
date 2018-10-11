using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FancyScrollView
{
    public class ScrollPositionController : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [Serializable]
        struct Snap
        {
            public bool Enable;
            public float VelocityThreshold;
            public float Duration;
        }

        enum ScrollDirection
        {
            Vertical,
            Horizontal,
        }

        enum MovementType
        {
            Unrestricted = ScrollRect.MovementType.Unrestricted,
            Elastic = ScrollRect.MovementType.Elastic,
            Clamped = ScrollRect.MovementType.Clamped
        }

        [SerializeField]
        RectTransform viewport;
        [SerializeField]
        ScrollDirection directionOfRecognize = ScrollDirection.Vertical;
        [SerializeField]
        MovementType movementType = MovementType.Elastic;
        [SerializeField]
        float scrollSensitivity = 1f;
        [SerializeField]
        bool inertia = true;
        [SerializeField, Tooltip("Only used when inertia is enabled")]
        float decelerationRate = 0.03f;
        [SerializeField, Tooltip("Only used when inertia is enabled")]
        Snap snap = new Snap { Enable = true, VelocityThreshold = 0.5f, Duration = 0.3f };
        [SerializeField]
        int dataCount;

        Action<float> onUpdatePosition;
        Action<int> onItemSelected;

        Vector2 pointerStartLocalPosition;
        float dragStartScrollPosition;
        float currentScrollPosition;
        bool dragging;

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
                           / GetViewportSize()
                           * scrollSensitivity
                           + dragStartScrollPosition;

            var offset = CalculateOffset(position);
            position += offset;

            if (movementType == MovementType.Elastic)
            {
                if (offset != 0)
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

        float GetViewportSize()
        {
            return directionOfRecognize == ScrollDirection.Horizontal
                ? viewport.rect.size.x
                : viewport.rect.size.y;
        }

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

            if (position > dataCount - 1)
            {
                return dataCount - 1 - position;
            }

            return 0f;
        }

        void UpdatePosition(float position)
        {
            currentScrollPosition = position;

            if (onUpdatePosition != null)
            {
                onUpdatePosition(currentScrollPosition);
            }
        }

        void ItemSelected(int index)
        {
            if (onItemSelected != null)
            {
                onItemSelected(index);
            }
        }

        float RubberDelta(float overStretching, float viewSize)
        {
            return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
        }

        public void OnUpdatePosition(Action<float> onUpdatePosition)
        {
            this.onUpdatePosition = onUpdatePosition;
        }

        public void OnItemSelected(Action<int> onItemSelected)
        {
            this.onItemSelected = onItemSelected;
        }

        public void SetDataCount(int dataCount)
        {
            this.dataCount = dataCount;
        }

        float velocity;
        float prevScrollPosition;

        class AutoScrollState
        {
            public bool Enable;
            public float Duration;
            public float StartTime;
            public float EndScrollPosition;
        }

        readonly AutoScrollState autoScrollState = new AutoScrollState();

        void Update()
        {
            var deltaTime = Time.unscaledDeltaTime;
            var offset = CalculateOffset(currentScrollPosition);

            if (autoScrollState.Enable)
            {
                var alpha = Mathf.Clamp01((Time.unscaledTime - autoScrollState.StartTime) / Mathf.Max(autoScrollState.Duration, float.Epsilon));
                var position = Mathf.Lerp(dragStartScrollPosition, autoScrollState.EndScrollPosition, EaseInOutCubic(0, 1, alpha));

                if (Mathf.Approximately(alpha, 1f))
                {
                    autoScrollState.Enable = false;
                }

                UpdatePosition(position);
            }
            else if (!dragging && (!Mathf.Approximately(offset, 0f) || !Mathf.Approximately(velocity, 0f)))
            {
                var position = currentScrollPosition;

                if (movementType == MovementType.Elastic && !Mathf.Approximately(offset, 0f))
                {
                    ScrollTo(Mathf.RoundToInt(position + offset), 0.35f);
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
                        ScrollTo(Mathf.RoundToInt(currentScrollPosition), snap.Duration);
                    }
                }
                // If we have neither elaticity or friction, there shouldn't be any velocity.
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

                        if (Mathf.Approximately(position, 0f) || Mathf.Approximately(position, dataCount - 1f))
                        {
                            velocity = 0f;
                            ItemSelected(Mathf.RoundToInt(position));
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

            if (currentScrollPosition != prevScrollPosition)
            {
                prevScrollPosition = currentScrollPosition;
            }
        }

        public void ScrollTo(int index, float duration)
        {
            velocity = 0f;
            dragStartScrollPosition = currentScrollPosition;

            autoScrollState.Enable = true;
            autoScrollState.Duration = duration;
            autoScrollState.StartTime = Time.unscaledTime;
            autoScrollState.EndScrollPosition = CalculateDestinationIndex(index);

            ItemSelected(Mathf.RoundToInt(GetCircularPosition(autoScrollState.EndScrollPosition, dataCount)));
        }

        public void JumpTo(int index)
        {
            velocity = 0f;
            dragging = false;
            autoScrollState.Enable = false;

            index = CalculateDestinationIndex(index);

            ItemSelected(index);
            UpdatePosition(index);
        }

        int CalculateDestinationIndex(int index)
        {
            return movementType == MovementType.Unrestricted
                ? CalculateClosestIndex(index)
                : Mathf.Clamp(index, 0, dataCount - 1);
        }

        int CalculateClosestIndex(int index)
        {
            var diff = GetCircularPosition(index, dataCount)
                       - GetCircularPosition(currentScrollPosition, dataCount);

            if (Mathf.Abs(diff) > dataCount * 0.5f)
            {
                diff = Mathf.Sign(-diff) * (dataCount - Mathf.Abs(diff));
            }

            return Mathf.RoundToInt(diff + currentScrollPosition);
        }

        float GetCircularPosition(float position, int length)
        {
            return position < 0 ? length - 1 + (position + 1) % length : position % length;
        }

        float EaseInOutCubic(float start, float end, float value)
        {
            value /= 0.5f;
            end -= start;

            if (value < 1f)
            {
                return end * 0.5f * value * value * value + start;
            }

            value -= 2f;
            return end * 0.5f * (value * value * value + 2f) + start;
        }
    }
}
