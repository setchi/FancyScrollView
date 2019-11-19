﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using EasingCore;

namespace FancyScrollView
{
    /// <summary>
    /// スクロール位置の制御を行うコンポーネント.
    /// </summary>
    public class Scroller : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        /// <summary>
        /// ビューポートとなる RectTransform を指定します.
        /// </summary>
        [SerializeField] RectTransform viewport = default;

        /// <summary>
        /// スクロール方向を指定します.
        /// <see cref="FancyScrollView.ScrollDirection"/>
        /// </summary>
        [SerializeField] ScrollDirection scrollDirection = ScrollDirection.Vertical;

        /// <summary>
        /// コンテンツがスクロール範囲を越えて移動するときに使用する挙動を指定します.
        /// <see cref="FancyScrollView.MovementType"/>
        /// </summary>
        [SerializeField] MovementType movementType = MovementType.Elastic;

        /// <summary>
        /// コンテンツがスクロール範囲を越えて移動するときに使用する弾力性の量を指定します.
        /// </summary>
        [SerializeField] float elasticity = 0.1f;

        /// <summary>
        /// スクロールの感度を指定します.
        /// </summary>
        [SerializeField] float scrollSensitivity = 1f;

        /// <summary>
        /// true を指定すると慣性が有効に, false を指定すると慣性が無効になります.
        /// </summary>
        [SerializeField] bool inertia = true;

        /// <summary>
        /// <see cref="inertia"/> が true の場合のみ有効です. スクロールの減速率を指定します.
        /// </summary>
        [SerializeField] float decelerationRate = 0.03f;

        /// <summary>
        /// スナップに関する挙動を指定します.
        /// </summary>
        [SerializeField] Snap snap = new Snap {
            Enable = true,
            VelocityThreshold = 0.5f,
            Duration = 0.3f,
            Easing = Ease.InOutCubic
        };

        /// <summary>
        /// Drag 入力を受付けるかどうかを指定します.
        /// </summary>
        [SerializeField] bool draggable = true;

        /// <summary>
        /// スクロールバーのオブジェクトを指定します(任意).
        /// </summary>
        [SerializeField] Scrollbar scrollbar = default;

        public ScrollDirection ScrollDirection => scrollDirection;

        public MovementType MovementType
        {
            get => movementType;
            set => movementType = value;
        }

        public float ViewportSize => scrollDirection == ScrollDirection.Horizontal
            ? viewport.rect.size.x
            : viewport.rect.size.y;

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

        public float Position
        {
            get => currentPosition;
            set {
                autoScrollState.Reset();
                velocity = 0f;
                dragging = false;

                UpdatePosition(value);
            }
        }

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
                scrollbar.onValueChanged.AddListener(x => UpdatePosition(x * (totalCount - 1f), false));
            }
        }

        /// <summary>
        /// スクロール位置が変化したときのコールバックを設定します.
        /// </summary>
        /// <param name="callback">スクロール位置が変化したときのコールバック.</param>
        public void OnValueChanged(Action<float> callback) => onValueChanged = callback;

        /// <summary>
        /// 選択位置が変化したときのコールバックを設定します.
        /// </summary>
        /// <param name="callback">選択セルが変化したときのコールバック.</param>
        public void OnSelectionChanged(Action<int> callback) => onSelectionChanged = callback;

        /// <summary>
        /// アイテムの総数を設定します.
        /// </summary>
        /// <remarks>
        /// この値を元に最大スクロール位置を計算します.
        /// </remarks>
        /// <param name="totalCount">アイテムの総数.</param>
        public void SetTotalCount(int totalCount) => this.totalCount = totalCount;

        /// <summary>
        /// 指定した位置まで移動します.
        /// </summary>
        /// <param name="position">スクロール位置. 0.0 ~ totalCount - 1 の範囲.</param>
        /// <param name="duration">移動にかける秒数.</param>
        /// <param name="onComplete">移動が完了した際に呼び出されるコールバック.</param>
        public void ScrollTo(float position, float duration, Action onComplete = null) => ScrollTo(position, duration, Ease.OutCubic, onComplete);

        /// <summary>
        /// 指定した位置まで移動します.
        /// </summary>
        /// <param name="position">スクロール位置. 0.0 ~ totalCount - 1 の範囲.</param>
        /// <param name="duration">移動にかける秒数.</param>
        /// <param name="easing">移動に使用するイージング.</param>
        /// <param name="onComplete">移動が完了した際に呼び出されるコールバック.</param>
        public void ScrollTo(float position, float duration, Ease easing, Action onComplete = null) => ScrollTo(position, duration, EasingFunction.Get(easing), onComplete);

        /// <summary>
        /// 指定した位置まで移動します.
        /// </summary>
        /// <param name="position">スクロール位置. 0.0 ~ totalCount - 1 の範囲.</param>
        /// <param name="duration">移動にかける秒数.</param>
        /// <param name="easingFunction">移動に使用するイージング関数.</param>
        /// <param name="onComplete">移動が完了した際に呼び出されるコールバック.</param>
        public void ScrollTo(float position, float duration, Func<float, float> easingFunction, Action onComplete = null)
        {
            if (duration <= 0f)
            {
                Position = CircularPosition(position, totalCount);
                onComplete?.Invoke();
                return;
            }

            autoScrollState.Reset();
            autoScrollState.Enable = true;
            autoScrollState.Duration = duration;
            autoScrollState.EasingFunction = easingFunction ?? DefaultEasingFunction;
            autoScrollState.StartTime = Time.unscaledTime;
            autoScrollState.EndPosition = currentPosition + CalculateMovementAmount(currentPosition, position);
            autoScrollState.OnComplete = onComplete;

            velocity = 0f;
            scrollStartPosition = currentPosition;

            UpdateSelection(Mathf.RoundToInt(CircularPosition(autoScrollState.EndPosition, totalCount)));
        }

        /// <summary>
        /// 指定したインデックスの位置までジャンプします.
        /// </summary>
        /// <param name="index">アイテムのインデックス.</param>
        public void JumpTo(int index)
        {
            if (index < 0 || index > totalCount - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            autoScrollState.Reset();

            velocity = 0f;
            dragging = false;

            UpdateSelection(index);
            UpdatePosition(index);
        }

        /// <summary>
        /// <paramref name="sourceIndex"/> から <paramref name="destIndex"/> に移動する際の移動方向を返します.
        /// スクロール範囲が無制限に設定されている場合は, 最短距離の移動方向を返します.
        /// </summary>
        /// <param name="sourceIndex">移動元のインデックス.</param>
        /// <param name="destIndex">移動先のインデックス.</param>
        /// <returns></returns>
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

            UpdatePosition(position);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!draggable || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            dragging = false;
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

            if (position > totalCount - 1)
            {
                return totalCount - 1 - position;
            }

            return 0f;
        }

        void UpdatePosition(float position, bool updateScrollbar = true)
        {
            onValueChanged?.Invoke(currentPosition = position);

            if (scrollbar && updateScrollbar)
            {
                scrollbar.value = Mathf.Clamp01(position / Mathf.Max(totalCount - 1f, 1e-4f));
            }
        }

        void UpdateSelection(int index) => onSelectionChanged?.Invoke(index);

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

                UpdatePosition(position);
            }
            else if (!dragging && (!Mathf.Approximately(offset, 0f) || !Mathf.Approximately(velocity, 0f)))
            {
                var position = currentPosition;

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
                            UpdateSelection(Mathf.RoundToInt(position));
                        }
                    }

                    UpdatePosition(position);
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
    }
}
