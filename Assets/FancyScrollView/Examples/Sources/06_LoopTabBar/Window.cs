using UnityEngine;

namespace FancyScrollView.Example06
{
    public class Window : MonoBehaviour
    {
        [SerializeField] SlideScreenTransition inAnim = default;
        [SerializeField] SlideScreenTransition outAnim = default;

        public void OpenWindow(Scroller.MovementDirection direction)
        {
            if (inAnim != null)
            {
                inAnim.Animate(direction);
            }
        }

        public void HideWindow(Scroller.MovementDirection direction)
        {
            if (outAnim != null)
            {
                outAnim.Animate(direction);
            }
        }
    }
}
