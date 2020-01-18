/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;

namespace FancyScrollView.Example09
{
    class Example09 : MonoBehaviour
    {
        readonly ItemData[] itemData =
        {
            new ItemData(
                "FancyScrollView",
                "A scrollview component that can implement highly flexible animation.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/00.png"
            ),
            new ItemData(
                "01_Basic",
                "Example of simplest implementation.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/01.png"
            ),
            new ItemData(
                "02_FocusOn",
                "Example of focusing on the left and right cells with buttons.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/02.png"
            ),
            new ItemData(
                "03_InfiniteScroll",
                "Example of infinite scroll implementation.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/03.png"
            ),
            new ItemData(
                "04_Metaball",
                "Example of metaball implementation using shaders.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/04.png"
            ),
            new ItemData(
                "05_Voronoi",
                "Example of voronoi implementation using shaders.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/05.png"
            ),
            new ItemData(
                "06_LoopTabBar",
                "Example of switching screens with tabs.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/06.png"
            ),
            new ItemData(
                "07_ScrollRect",
                "Example of ScrollRect style implementation with scroll bar.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/07.png"
            ),
            new ItemData(
                "08_GridView",
                "Example of grid layout implementation.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/08.png"
            ),
            new ItemData(
                "09_LoadTexture",
                "Example of load texture implementation.",
                "https://setchi.jp/FancyScrollView/09_LoadTexture/Images/09.png"
            )
        };

        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            scrollView.UpdateData(itemData);
        }
    }
}
