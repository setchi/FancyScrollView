# FancyScrollView [![license](https://img.shields.io/badge/license-MIT-green.svg?style=flat-square)](https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
高度に柔軟なアニメーションを実装できる汎用のScrollViewコンポーネントです。 無限スクロールもサポートしています。[English](https://translate.google.com/translate?sl=ja&tl=en&u=https://github.com/setchi/FancyScrollView) (by Google Translate)

![screencast](Documents/logo.png)
![screencast](Documents/screencast1.gif)
![screencast](Documents/screencast2.gif)

## Requirements
- Unity 2018.3 or later.
- [.NET 4.x Scripting Runtime](https://docs.unity3d.com/Manual/ScriptingRuntimeUpgrade.html)

## Installation
### Unity Asset Store
Install the package in your project using the [Asset Store](https://assetstore.unity.com/packages/tools/gui/fancyscrollview-96530) page.

### Unity Package Manager *(Example scenes not included)*
Add a reference to the repository in the [`Packages\manifest.json`](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html#project-manifests) file in your project directory:

```json
{
  "dependencies": {
    "jp.setchi.fancyscrollview": "https://github.com/setchi/FancyScrollView.git#upm"
  }
}
```

### Manual
Clone or download this repository.

## Examples
[FancyScrollView/Examples](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples) を参照してください。

| Name | Description |
|:-----------|:------------|
|01_Basic|最もシンプルな構成の実装例です。|
|02_FocusOn|ボタンで左右のセルにフォーカスする実装例です。|
|03_InfiniteScroll|無限スクロールの実装例です。|

## How it works
FancyScrollView はセルの位置を更新するとき、可視領域の正規化された値を各セルに渡します。セル側では、0.0 ~ 1.0 の値に基づいてスクロールの外観を自由に制御できます。

## Usage
もっともシンプルな構成では、

- セルにデータを渡すためのオブジェクト
- セル
- スクロールビュー

の実装が必要です。

### Implementation
セルにデータを渡すためのオブジェクトを定義します。
```csharp
public class ItemData
{
    public string Message;
}
```
`FancyScrollViewCell<TItemData>` を継承して自分のセルを実装します。
```csharp
using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;

public class MyScrollViewCell : FancyScrollViewCell<ItemData>
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

public class MyScrollView : FancyScrollView<ItemData>
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

public class EntryPoint : MonoBehaviour
{
    [SerializeField] MyScrollView myScrollView = default;

    void Start()
    {
        var items = Enumerable.Range(0, 20)
            .Select(i => new ItemData {Message = $"Cell {i}"})
            .ToArray();

        myScrollView.UpdateData(items);
    }
}
```

### Settings on the Inspector
#### My Scroll View
| プロパティ | 説明 |
|:-----------|:------------|
|Cell Spacing|セル同士の間隔を float.Epsilon ~ 1.0 の間で指定します。|
|Scroll Offset|スクロールのオフセットを指定します。たとえば、 0.5 を指定してスクロール位置が 0 の場合、最初のセルの位置は 0.5 になります。|
|Loop|オンにするとセルが循環し、最初のセルの前に最後のセル、最後のセルの後に最初のセルが並ぶようになります。無限スクロールさせたい場合はオンにします。|
|Cell Prefab|セルの Prefab を指定します。|
|Cell Container|セルの親要素となる Transform を指定します。 |

#### Scroller
| プロパティ | 説明 |
|:-----------|:------------|
|Viewport|ビューポートとなる RectTransform を指定します。ここで指定された RectTransform の範囲内でジェスチャーの検出を行います。|
|Direction Of Recognize|ジェスチャーを認識する方向を Vertical か Horizontal で指定します。|
|Movement Type|コンテンツがスクロール範囲を越えて移動するときに使用する挙動を指定します。|
|Elasticity|コンテンツがスクロール範囲を越えて移動するときに使用する弾力性の量を指定します。|
|Scroll Sensitivity|スクロールの感度を指定します。|
|Inertia|慣性のオン/オフを指定します。|
|Deceleration Rate|Inertia がオンの場合のみ有効です。減速率を指定します。|
|Snap - Enable|Snap を有効にする場合オンにします。|
|Snap - Velocity Threshold|Snap がはじまる閾値となる速度を指定します。|
|Snap - Duration|Snap 時の移動時間を秒数で指定します。|
|Snap - Easing|Snap 時の Easing を指定します。|

## FAQ

#### データ件数が多くてもパフォーマンスは大丈夫？
表示に必要なセル数のみが生成されるため、データ件数がパフォーマンスに与える影響はわずかです。セル間のスペース（同時に存在するセルの数）とセルの演出は、データ件数よりもパフォーマンスに大きな影響を与えます。

#### 自分でスクロール位置を制御したいんだけど？
`FancyScrollView` の次の API を使用してスクロール位置を更新できます。
```csharp
protected void UpdatePosition(float position)
```
サンプルで使われている `Scroller` を使う場合は、次の API を使用して `FancyScrollView` のスクロール位置を更新できます。
```csharp
public void JumpTo(int index)
```
```csharp
public void ScrollTo(int index, float duration)
```
```csharp
public void ScrollTo(int index, float duration, Easing easing)
```
```csharp
public void OnValueChanged(Action<float> callback)
```
`Scroller` を使わずにあなた自身の実装で全く違った振る舞いをさせることもできます。

#### セルで発生したイベントを受け取れる？
セル内で発生したあらゆるイベントをハンドリングできます。セル内で発生したイベントをハンドリングする実装例（[Examples/02_FocusOn](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/Source/02_FocusOn)）が含まれていますので、参考にして実装してください。

#### セルを無限スクロール（ループ）させたいんだけど？
無限スクロールをサポートしています。実装手順は下記の通りです。
1. `ScrollView` の `Loop` をオンにするとセルが循環し、最初のセルの前に最後のセル、最後のセルの後に最初のセルが並ぶようになります。
1. サンプルで使用されている `Scroller` を使うときは、 `Movement Type` を `Unrestricted` に設定することで、スクロール範囲が無制限になります。 1. と組み合わせることで無限スクロールを実現できます。

実装例（[Examples/03_InfiniteScroll](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples)）が含まれていますので、こちらも参考にしてください。

## Author
[setchi](https://github.com/setchi)

## License
MIT
