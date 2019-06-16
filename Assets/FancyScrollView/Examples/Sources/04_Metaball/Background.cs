using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FancyScrollView.Example04
{
    public class Background : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image background = default;
        [SerializeField] ScrollView scrollView = default;

        RectTransform rectTransform;

        static class ShaderID
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
            background.material.SetVector(ShaderID.Resolution, rectTransform.rect.size);
            background.material.SetVectorArray(
                ShaderID.CellState,
                scrollView.GetCellState().Concat(GetObjects()).ToArray());
        }

        Vector4[] GetObjects()
        {
            return new[]
            {
                new Vector4( 500, -330 + Mathf.Sin(Time.time) * 60, -1, 2.5f),
                new Vector4(-500, -330 + Mathf.Sin(Time.time) * 60, -1, 2.5f)
            };
        }

        bool MetaballContains(Vector2 p, IEnumerable<Vector4> cellState)
        {
            float f(Vector2 v) => 1f / (v.x * v.x + v.y * v.y + 0.0001f);

            float scale = 4600f;
            float d = cellState.Sum(x => f(p - new Vector2(x.x, x.y)) * x.w);
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
            if (!MetaballContains(clickPosition, cellState.Concat(GetObjects())))
            {
                return;
            }

            var dataIndex = cellState
                .Select(s => (
                    index: Mathf.RoundToInt(s.z),
                    distance: (new Vector2(s.x, s.y) - clickPosition).sqrMagnitude
                ))
                .OrderBy(x => x.distance)
                .FirstOrDefault()
                .index;

            scrollView.SelectCell(dataIndex);
        }
    }
}
