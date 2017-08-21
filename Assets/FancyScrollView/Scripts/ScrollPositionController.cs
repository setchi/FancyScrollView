using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    float elasticity = 0.1f;
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
            return 0;
        }
        if (position < 0)
        {
            return -position;
        }
        if (position > dataCount - 1)
        {
            return (dataCount - 1) - position;
        }
        return 0;
    }

    void UpdatePosition(float position)
    {
        currentScrollPosition = position;

        if (onUpdatePosition != null)
        {
            onUpdatePosition(currentScrollPosition);
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

    public void SetDataCount(int dataCont)
    {
        this.dataCount = dataCont;
    }

    float velocity;
    float prevScrollPosition;

    bool autoScrolling;
    float autoScrollDuration;
    float autoScrollStartTime;
    float autoScrollPosition;

    void Update()
    {
        var deltaTime = Time.unscaledDeltaTime;
        var offset = CalculateOffset(currentScrollPosition);

        if (autoScrolling)
        {
            var alpha = Mathf.Clamp01((Time.unscaledTime - autoScrollStartTime) / Mathf.Max(autoScrollDuration, float.Epsilon));
            var position = Mathf.Lerp(dragStartScrollPosition, autoScrollPosition, EaseInOutCubic(0, 1, alpha));
            UpdatePosition(position);

            if (Mathf.Approximately(alpha, 1f))
            {
                autoScrolling = false;
            }
        }
        else if (!dragging && (offset != 0 || velocity != 0))
        {
            var position = currentScrollPosition;
            // Apply spring physics if movement is elastic and content has an offset from the view.
            if (movementType == MovementType.Elastic && offset != 0)
            {
                var speed = velocity;
                position = Mathf.SmoothDamp(currentScrollPosition, currentScrollPosition + offset, ref speed, elasticity, Mathf.Infinity, deltaTime);
                velocity = speed;
            }
            // Else move content according to velocity with deceleration applied.
            else if (inertia)
            {
                velocity *= Mathf.Pow(decelerationRate, deltaTime);
                if (Mathf.Abs(velocity) < 0.001f)
                    velocity = 0;
                position += velocity * deltaTime;

                if (snap.Enable && Mathf.Abs(velocity) < snap.VelocityThreshold)
                {
                    ScrollTo(Mathf.RoundToInt(currentScrollPosition), snap.Duration);
                }
            }
            // If we have neither elaticity or friction, there shouldn't be any velocity.
            else
            {
                velocity = 0;
            }

            if (velocity != 0)
            {
                if (movementType == MovementType.Clamped)
                {
                    offset = CalculateOffset(position);
                    position += offset;
                }
                UpdatePosition(position);
            }
        }

        if (!autoScrolling && dragging && inertia)
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
        velocity = 0;
        autoScrolling = true;
        autoScrollDuration = duration;
        autoScrollStartTime = Time.unscaledTime;
        dragStartScrollPosition = currentScrollPosition;

        autoScrollPosition = movementType == MovementType.Unrestricted
            ? CalculateClosestPosition(index)
            : index;
    }

    float CalculateClosestPosition(int index)
    {
        var diff = GetLoopPosition(index, dataCount)
                   - GetLoopPosition(currentScrollPosition, dataCount);

        if (Mathf.Abs(diff) > dataCount * 0.5f)
        {
            diff = Mathf.Sign(-diff) * (dataCount - Mathf.Abs(diff));
        }
        return diff + currentScrollPosition;
    }

    float GetLoopPosition(float position, int length)
    {
        if (position < 0)
        {
            position = (length - 1) + (position + 1) % length;
        }
        else if (position > length - 1)
        {
            position = position % length;
        }
        return position;
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
