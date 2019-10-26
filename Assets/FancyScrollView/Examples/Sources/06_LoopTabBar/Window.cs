using UnityEngine;

namespace FancyScrollView.Example06
{
    public class Window : MonoBehaviour
    {
        [SerializeField] SlideScreenTransition transition = default;

        public void In(Scroller.MovementDirection direction)
        {
            if (transition != null)
            {
                transition.In(direction);
            }
        }

        public void Out(Scroller.MovementDirection direction)
        {
            if (transition != null)
            {
                transition.Out(direction);
            }
        }
    }
}
