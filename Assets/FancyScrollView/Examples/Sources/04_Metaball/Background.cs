/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FancyScrollView.Example04
{
    class Background : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image background = default;
        [SerializeField] ScrollView scrollView = default;

        RectTransform rectTransform;

        static class Uniform
        {
            public static readonly int Resolution = Shader.PropertyToID("_Resolution");
            public static readonly int CellState = Shader.PropertyToID("_CellState");
        }

        void Start()
        {
            rectTransform = transform as RectTransform;
        }

        void LateUpdate()
        {
            var offset = scrollView.CellInstanceCount;

            scrollView.SetCellState(offset + 0, -1,  500, -330 + Mathf.Sin(Time.time) * 60, 2.5f);
            scrollView.SetCellState(offset + 1, -1, -500, -330 + Mathf.Sin(Time.time) * 60, 2.5f);

            background.material.SetVector(Uniform.Resolution, rectTransform.rect.size);
            background.material.SetVectorArray(Uniform.CellState, scrollView.GetCellState());
        }

        bool MetaballContains(Vector2 p, Vector4[] cellState)
        {
            float f(Vector2 v) => 1f / (v.x * v.x + v.y * v.y + 0.0001f);

            const float scale = 4600f;
            var d = cellState.Sum(x => f(p - new Vector2(x.x, x.y)) * x.w);
            return d * scale > 0.46f;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var clickPosition
            );

            var cellState = scrollView.GetCellState();
            if (!MetaballContains(clickPosition, cellState))
            {
                return;
            }

            var dataIndex = cellState
                .Take(scrollView.CellInstanceCount)
                .Select(s => (
                    index: Mathf.RoundToInt(s.z),
                    distance: (new Vector2(s.x, s.y) - clickPosition).sqrMagnitude
                ))
                .Aggregate((min, x) => x.distance < min.distance ? x : min)
                .index;

            scrollView.SelectCell(dataIndex);
        }
    }
}
