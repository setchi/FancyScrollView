/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FancyScrollView.Example05
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
            var rect = rectTransform.rect.size * 0.5f;
            var offset = scrollView.CellInstanceCount;

            scrollView.SetCellState(offset + 0, -1,  rect.x, -rect.y * 1.3f, 0f);
            scrollView.SetCellState(offset + 1, -1, -rect.x,  rect.y * 1.3f, 0f);
            scrollView.SetCellState(offset + 2, -1, -rect.x, -rect.y * 1.3f, 0f);
            scrollView.SetCellState(offset + 3, -1,  rect.x,  rect.y * 1.3f, 0f);

            background.material.SetVector(Uniform.Resolution, rectTransform.rect.size);
            background.material.SetVectorArray(Uniform.CellState, scrollView.GetCellState());
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

            var cellState = scrollView.GetCellState()
                .Select((s, i) => (
                    index: i,
                    dataIndex: Mathf.RoundToInt(s.z),
                    position: new Vector2(s.x, s.y)
                ));

            var target = cellState
                .OrderBy(x => (x.position - clickPosition).sqrMagnitude)
                .First();

            var distance = cellState
                .Where(x => x.index != target.index)
                .Min(x => Vector2.Dot(
                    clickPosition - (target.position + x.position) * 0.5f,
                    (target.position - x.position).normalized
                ));

            const float borderWidth = 9;
            if (distance < borderWidth)
            {
                return;
            }

            scrollView.SelectCell(target.dataIndex);
        }
    }
}
