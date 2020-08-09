# FancyScrollView

[![license](https://img.shields.io/badge/license-MIT-green.svg?style=flat&cacheSeconds=2592000)](https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
[![WebGL Demo](https://img.shields.io/badge/demo-WebGL-orange.svg?style=flat&logo=google-chrome&logoColor=white&cacheSeconds=2592000)](https://setchi.jp/FancyScrollView/demo)
[![API Documentation](https://img.shields.io/badge/API-Documentation-ff69b4.svg?style=flat&logo=c-sharp&cacheSeconds=2592000)](https://setchi.jp/FancyScrollView/api/FancyScrollView.html)
[![openupm](https://img.shields.io/npm/v/jp.setchi.fancyscrollview?label=openupm&registry_uri=https://package.openupm.com&style=flat)](https://openupm.com/packages/jp.setchi.fancyscrollview/)

[English](https://translate.google.com/translate?sl=ja&tl=en&u=https://github.com/setchi/FancyScrollView) (by Google Translate)

高度に柔軟なアニメーションを実装できる汎用の ScrollView コンポーネントです。 無限スクロールもサポートしています。

<img src="https://user-images.githubusercontent.com/8326814/69004520-d2b36b80-0957-11ea-8277-06bfd3e8f033.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/70638335-0b571400-1c7c-11ea-8701-a0d1ae0cb7e3.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/59548448-a3549900-8f8a-11e9-9a27-b04f1410a7b5.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/59548462-b8c9c300-8f8a-11e9-8985-5f1c2e610309.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/59550410-7f528100-8fa5-11e9-8f1b-41e59b645571.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/59550411-7f528100-8fa5-11e9-8bfb-bd42da47f7a0.gif" width="320">

## Requirements
[![Unity 2019.4+](https://img.shields.io/badge/unity-2019.4+-black.svg?style=flat&logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)
[![.NET 4.x Scripting Runtime](https://img.shields.io/badge/.NET-4.x-blueviolet.svg?style=flat&cacheSeconds=2592000)](https://docs.unity3d.com/2018.3/Documentation/Manual/ScriptingRuntimeUpgrade.html)

## Installation
### Unity Asset Store
[Unity Asset Store](https://assetstore.unity.com/packages/tools/gui/fancyscrollview-96530) から購入して、さらなる開発のサポートを検討してください。

### OpenUPM
[OpenUPM](https://openupm.com/) レジストリからパッケージを Unity Project に追加します。

```
openupm add jp.setchi.fancyscrollview
```

### Unity Package Manager
プロジェクトディレクトリの [`Packages/manifest.json`](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html#project-manifests) ファイルにリポジトリへの参照を追加します。

```json
{
  "dependencies": {
    "jp.setchi.fancyscrollview": "https://github.com/setchi/FancyScrollView.git#upm"
  }
}
```

## Features
### 自由にスクロールアニメーションを実装できます
FancyScrollView はスクロール位置を更新するとき、ビューポート範囲の正規化された位置を各セルに渡します。セル側では `0.0` ~ `1.0` の値に基づいてスクロール中の位置や見た目を[セル自身で制御](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyCell-2.html#FancyScrollView_FancyCell_2_UpdatePosition_System_Single_)します。サンプルでは Animator や数式を使用してスクロール中の動きを実装しています。

### データ件数が多くても軽快に動作します
表示に必要なセル数のみが生成され、セルは再利用されます。 [Demo](https://setchi.jp/FancyScrollView/demo/) で実際にデータ件数を増やしながら動作を確認できます。 [FancyScrollRect](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyScrollRect-2.html) および [FancyGridView](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyGridView-2.html) では、[スクロール中にセルが再利用されるまでの余白](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyScrollRect-2.html#FancyScrollView_FancyScrollRect_2_reuseCellMarginCount)も指定できます。

### セルとスクロールビュー間で自由にメッセージのやりとりができます
`Context` 経由で、セルがクリックされたことをスクロールビューで検知したり、スクロールビューからセルに指示を出す処理がシンプルに実装できます。実装例（[Examples/02_FocusOn](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/Sources/02_FocusOn)）が含まれていますので、参考にしてください。

### 特定のセルにスクロールやジャンプができます
移動にかける秒数や Easing の指定もできます。詳しくは [API Documentation](https://setchi.jp/FancyScrollView/api/FancyScrollView.html) の [Class Scroller](https://setchi.jp/FancyScrollView/api/FancyScrollView.Scroller.html#FancyScrollView_Scroller_ScrollTo_System_Single_System_Single_EasingCore_Ease_System_Action_) を参照してください。

### スクロールの挙動を細かく設定できます
慣性の有無、減速率などスクロールに関する挙動の設定ができます。詳しくは [API Documentation](https://setchi.jp/FancyScrollView/api/FancyScrollView.html) の [Class Scroller](https://setchi.jp/FancyScrollView/api/FancyScrollView.Scroller.html) を参照してください。

### スナップをサポートしています
スナップを有効にすると、スクロールが止まる直前に最寄りのセルへ移動します。スナップがはじまる速度のしきい値、移動にかける秒数、 Easing を指定できます。[FancyScrollRect](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyScrollRect-2.html) および [FancyGridView](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyGridView-2.html) はスナップをサポートしていません。

### 無限スクロールをサポートしています
Inspector で下記の設定をすることで無限スクロールを実装できます。
1. `FancyScrollView` の `Loop` をオンにするとセルが循環し、先頭のセルの前に末尾のセル、末尾のセルの後に先頭のセルが並ぶようになります。
1. サンプルで使用されている `Scroller` を使うときは、 `Movement Type` を `Unrestricted` に設定することで、スクロール範囲が無制限になります。 1. と組み合わせることで無限スクロールを実現できます。

実装例（[Examples/03_InfiniteScroll](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples)）が含まれていますので、こちらも参考にしてください。[FancyScrollRect](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyScrollRect-2.html) および [FancyGridView](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyGridView-2.html) は無限スクロールをサポートしていません。

## Examples
[![WebGL Demo](https://img.shields.io/badge/demo-WebGL-orange.svg?style=flat&logo=google-chrome&logoColor=white&cacheSeconds=2592000)](https://setchi.jp/FancyScrollView/demo)

[FancyScrollView/Examples](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples) を参照してください。

| Name | Description |
|:-----------|:------------|
|01_Basic|最もシンプルな構成の実装例です。|
|02_FocusOn|ボタンで左右のセルにフォーカスする実装例です。|
|03_InfiniteScroll|無限スクロールの実装例です。|
|04_Metaball|シェーダーを使用したメタボールの実装例です。|
|05_Voronoi|シェーダーを使用したボロノイの実装例です。|
|06_LoopTabBar|タブで画面を切り替える実装例です。|
|07_ScrollRect|スクロールバー付きの `ScrollRect` スタイルの実装例です。|
|08_GridView|グリッドレイアウトの実装例です。|
|09_LoadTexture|テクスチャをロードして表示する実装例です。|

## Usage
もっともシンプルな構成では、

- セルにデータを渡すためのオブジェクト
- セル
- スクロールビュー

の実装が必要です。

### Implementation
セルにデータを渡すためのオブジェクトを定義します。
```csharp
class ItemData
{
    public string Message { get; }

    public ItemData(string message)
    {
        Message = message;
    }
}
```
`FancyCell<TItemData>` を継承して自分のセルを実装します。
```csharp
using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;

class MyCell : FancyCell<ItemData>
{
    [SerializeField] Text message = default;

    public override void UpdateContent(ItemData itemData)
    {
        message.text = itemData.Message;
    }

    public override void UpdatePosition(float position)
    {
        // position は 0.0 ~ 1.0 の値です
        // position に基づいてスクロールの外観を自由に制御できます
    }
}
```
`FancyScrollView<TItemData>` を継承して自分のスクロールビューを実装します。
```csharp
using UnityEngine;
using System.Linq;
using FancyScrollView;

class MyScrollView : FancyScrollView<ItemData>
{
    [SerializeField] Scroller scroller = default;
    [SerializeField] GameObject cellPrefab = default;

    protected override GameObject CellPrefab => cellPrefab;

    void Start()
    {
        scroller.OnValueChanged(base.UpdatePosition);
    }

    public void UpdateData(IList<ItemData> items)
    {
        base.UpdateContents(items);
        scroller.SetTotalCount(items.Count);
    }
}
```
スクロールビューにデータを流し込みます。
```csharp
using UnityEngine;
using System.Linq;

class EntryPoint : MonoBehaviour
{
    [SerializeField] MyScrollView myScrollView = default;

    void Start()
    {
        var items = Enumerable.Range(0, 20)
            .Select(i => new ItemData($"Cell {i}"))
            .ToArray();

        myScrollView.UpdateData(items);
    }
}
```

その他の詳細は [Examples](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples) および [API Documentation](https://setchi.jp/FancyScrollView/api/FancyScrollView.html) を参照してください。

## Author
[setchi](https://github.com/setchi)

## License
[MIT](https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
