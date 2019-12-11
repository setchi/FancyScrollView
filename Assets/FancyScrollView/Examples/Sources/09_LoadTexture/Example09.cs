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
                "最もシンプルな構成の実装例です。",
                "https://user-images.githubusercontent.com/8326814/70611606-ce255e80-1c48-11ea-8d3b-2baa5ffa83a4.png"
            ),
            new ItemData(
                "02_FocusOn",
                "ボタンで左右のセルにフォーカスする実装例です。",
                "https://user-images.githubusercontent.com/8326814/70611607-cebdf500-1c48-11ea-99ed-4efd11a90df0.png"
            ),
            new ItemData(
                "03_InfiniteScroll",
                "無限スクロールの実装例です。",
                "https://user-images.githubusercontent.com/8326814/70611608-cebdf500-1c48-11ea-8bdf-531284063cca.png"
            ),
            new ItemData(
                "04_Metaball",
                "シェーダーを使用したメタボールの実装例です。",
                "https://user-images.githubusercontent.com/8326814/70611609-cebdf500-1c48-11ea-8b8f-2e7410dc4985.png"
            ),
            new ItemData(
                "05_Voronoi",
                "シェーダーを使用したボロノイの実装例です。",
                "https://user-images.githubusercontent.com/8326814/70611610-cebdf500-1c48-11ea-81af-aa054c98ee76.png"
            ),
            new ItemData(
                "06_LoopTabBar",
                "タブで画面を切り替える実装例です。",
                "https://user-images.githubusercontent.com/8326814/70611612-cf568b80-1c48-11ea-86bf-c8f6747dc640.png"
            ),
            new ItemData(
                "07_ScrollRect",
                "スクロールバー付きの ScrollRect スタイルの実装例です。",
                "https://user-images.githubusercontent.com/8326814/70611613-cf568b80-1c48-11ea-8f02-1b9003a29fe5.png"
            ),
            new ItemData(
                "08_GridView",
                "グリッドレイアウトの実装例です。",
                "https://user-images.githubusercontent.com/8326814/70611614-cf568b80-1c48-11ea-8abd-8fd6aa577a4d.png"
            ),
            new ItemData(
                "09_LoadTexture",
                "Description",
                "https://user-images.githubusercontent.com/8326814/70611614-cf568b80-1c48-11ea-8abd-8fd6aa577a4d.png"
            )
        };

        [SerializeField] ScrollView scrollView = default;

        void Start()
        {
            scrollView.UpdateData(itemData);
        }
    }
}
