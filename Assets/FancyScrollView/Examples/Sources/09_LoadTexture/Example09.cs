/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;

namespace FancyScrollView.Example09
{
    public class Example09 : MonoBehaviour
    {
        readonly ItemData[] itemData =
        {
            new ItemData(
                "01_Basic",
                "Example of simplest implementation.",
                "https://user-images.githubusercontent.com/8326814/70628608-d0e57b00-1c6b-11ea-8755-d6b246611af1.png"
            ),
            new ItemData(
                "02_FocusOn",
                "Example of focusing on the left and right cells with buttons.",
                "https://user-images.githubusercontent.com/8326814/70628610-d0e57b00-1c6b-11ea-9693-c19903dbf74f.png"
            ),
            new ItemData(
                "03_InfiniteScroll",
                "Example of infinite scroll implementation.",
                "https://user-images.githubusercontent.com/8326814/70628611-d0e57b00-1c6b-11ea-8d7c-8d8fc80981ca.png"
            ),
            new ItemData(
                "04_Metaball",
                "Example of metaball implementation using shaders.",
                "https://user-images.githubusercontent.com/8326814/70628612-d0e57b00-1c6b-11ea-9acf-754b440cfae9.png"
            ),
            new ItemData(
                "05_Voronoi",
                "Example of voronoi implementation using shaders.",
                "https://user-images.githubusercontent.com/8326814/70628613-d17e1180-1c6b-11ea-8356-f98e04d87d23.png"
            ),
            new ItemData(
                "06_LoopTabBar",
                "Example of switching screens with tabs.",
                "https://user-images.githubusercontent.com/8326814/70628614-d17e1180-1c6b-11ea-92c8-991bb88ad4f0.png"
            ),
            new ItemData(
                "07_ScrollRect",
                "Example of ScrollRect style implementation with scroll bar.",
                "https://user-images.githubusercontent.com/8326814/70628615-d17e1180-1c6b-11ea-9928-6f09916e1bda.png"
            ),
            new ItemData(
                "08_GridView",
                "Example of grid layout implementation.",
                "https://user-images.githubusercontent.com/8326814/70628617-d216a800-1c6b-11ea-908e-a0052678ef06.png"
            ),
            new ItemData(
                "09_LoadTexture",
                "Example of load texture implementation.",
                "https://user-images.githubusercontent.com/8326814/70631409-7995d980-1c70-11ea-9753-dbd34f8b361b.png"
            )
        };

        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            scrollView.UpdateData(itemData);
        }
    }
}
