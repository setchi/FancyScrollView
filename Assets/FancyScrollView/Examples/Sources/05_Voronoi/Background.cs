using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FancyScrollView.Example05
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
                scrollView.GetCellState().Concat(GetCorners()).ToArray()
            );
        }

        Vector4[] GetCorners()
        {
            var rect = rectTransform.rect.size * 0.5f;
            return new[]
            {
                new Vector4( rect.x, -rect.y * 1.3f, -1, 0),
                new Vector4(-rect.x,  rect.y * 1.3f, -1, 0),
                new Vector4(-rect.x, -rect.y * 1.3f, -1, 0),
                new Vector4( rect.x,  rect.y * 1.3f, -1, 0),
            };
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
                .Concat(GetCorners())
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

            const float BorderWidth = 9;
            if (distance < BorderWidth)
            {
                return;
            }

            scrollView.SelectCell(target.dataIndex);
        }
    }
}
