/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;

namespace FancyScrollView.Example06
{
    public class Window : MonoBehaviour
    {
        [SerializeField] SlideScreenTransition transition = default;

        public void In(MovementDirection direction)
        {
            if (transition != null)
            {
                transition.In(direction);
            }
        }

        public void Out(MovementDirection direction)
        {
            if (transition != null)
            {
                transition.Out(direction);
            }
        }
    }
}
