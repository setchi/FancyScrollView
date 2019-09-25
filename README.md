# FancyScrollView [![license](https://img.shields.io/badge/license-MIT-green.svg?style=flat-square)](https://github.com/setchi/FancyScrollView/blob/master/LICENSE)

[English](https://translate.google.com/translate?sl=ja&tl=en&u=https://github.com/setchi/FancyScrollView) (by Google Translate)

高度に柔軟なアニメーションを実装できる汎用の ScrollView コンポーネントです。 無限スクロールもサポートしています。

![screencast](https://user-images.githubusercontent.com/8326814/59548501-5fae5f00-8f8b-11e9-9740-c98afd9aa785.png)
<img src="https://user-images.githubusercontent.com/8326814/59548448-a3549900-8f8a-11e9-9a27-b04f1410a7b5.gif" width="320">
<img src="https://user-images.githubusercontent.com/8326814/59548462-b8c9c300-8f8a-11e9-8985-5f1c2e610309.gif" width="320">
<img src="https://user-images.githubusercontent.com/8326814/59550410-7f528100-8fa5-11e9-8f1b-41e59b645571.gif" width="320">
<img src="https://user-images.githubusercontent.com/8326814/59550411-7f528100-8fa5-11e9-8bfb-bd42da47f7a0.gif" width="320">

## Demo
https://setchi.jp/FancyScrollView/

## Requirements
- Unity 2018.3 or later.
- [.NET 4.x Scripting Runtime](https://docs.unity3d.com/2018.3/Documentation/Manual/ScriptingRuntimeUpgrade.html)

## Installation
### Unity Asset Store
[Asset Store](https://assetstore.unity.com/packages/tools/gui/fancyscrollview-96530) からパッケージをプロジェクトにインストールします。

### Unity Package Manager *(Example scenes not included)*
プロジェクトディレクトリの [`Packages/manifest.json`](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html#project-manifests) ファイルにリポジトリへの参照を追加します。

```json
{
  "dependencies": {
    "jp.setchi.fancyscrollview": "https://github.com/setchi/FancyScrollView.git#upm"
  }
}
```

### Manual
このリポジトリを Clone または Download します。

## Features
### 自由にスクロールアニメーションを実装できます
FancyScrollView はセルの位置を更新するとき、可視領域の正規化された値を各セルに渡します。セル側では、0.0 ~ 1.0 の値に基づいてスクロールの外観を自由に制御できます。サンプルでは Animator を使用してセルの動きを制御しています。

### データ件数が多くても軽快に動作します
表示に必要なセル数のみが生成され、セルは再利用されます。

### セルとスクロールビュー間で自由にメッセージのやりとりができます
`Context` 経由で、セルがクリックされたことをスクロールビューで検知したり、スクロールビューからセルに指示を出す処理がシンプルに実装できます。実装例（[Examples/02_FocusOn](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/Sources/02_FocusOn)）が含まれていますので、参考にしてください。

### 特定のセルにスクロールやジャンプができます
移動にかける秒数や Easing の指定もできます。詳しくは API Reference の [Scroller - Methods](https://github.com/setchi/FancyScrollView/blob/master/README.md#methods-2) を参照してください。

### スクロールの挙動を細かく設定できます
慣性の有無、減速率などスクロールに関する挙動の設定ができます。詳しくは API Reference の [Scroller - Inspector](https://github.com/setchi/FancyScrollView/blob/master/README.md#inspector-1) を参照してください。

### スナップをサポートしています
スナップを有効にすると、スクロールが止まる直前に最寄りのセルへ移動します。スナップがはじまる速度のしきい値、移動にかける秒数、 Easing を指定できます。

### 無限スクロールをサポートしています
Inspector で下記の設定をすることで無限スクロールを実装できます。
1. `FancyScrollView` の `Loop` をオンにするとセルが循環し、先頭のセルの前に末尾のセル、末尾のセルの後に先頭のセルが並ぶようになります。
1. サンプルで使用されている `Scroller` を使うときは、 `Movement Type` を `Unrestricted` に設定することで、スクロール範囲が無制限になります。 1. と組み合わせることで無限スクロールを実現できます。

実装例（[Examples/03_InfiniteScroll](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples)）が含まれていますので、こちらも参考にしてください。

## Examples
[FancyScrollView/Examples](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples) を参照してください。

| Name | Description |
|:-----------|:------------|
|01_Basic|最もシンプルな構成の実装例です。|
|02_FocusOn|ボタンで左右のセルにフォーカスする実装例です。|
|03_InfiniteScroll|無限スクロールの実装例です。|

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
    public string Message { get; }

    public ItemData(string message)
    {
        Message = message;
    }
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
            .Select(i => new ItemData($"Cell {i}"))
            .ToArray();

        myScrollView.UpdateData(items);
    }
}
```

---

## API Reference
### `FancyScrollView<TItemData, TContext>`
セルを制御するスクロールビューの抽象基底クラスです。
```csharp
public abstract class FancyScrollView<TItemData, TContext>
    : MonoBehaviour where TContext : class, new()
```
`Context` が不要な場合はこちらを使用します。
```csharp
public abstract class FancyScrollView<TItemData>
    : FancyScrollView<TItemData, FancyScrollViewNullContext>
```

#### Inspector
| Type | Name | Summary |
|:-----------|:------------|:------------|
|`float`|`Cell Interval`|セル同士の間隔を float.Epsilon ~ 1.0 の間で指定します.|
|`float`|`Scroll Offset`|スクロールのオフセットを指定します.たとえば、 0.5 を指定してスクロール位置が 0 の場合、最初のセルの位置は 0.5 になります.|
|`bool`|`Loop`|オンにするとセルが循環し、最初のセルの前に最後のセル、最後のセルの後に最初のセルが並ぶようになります.無限スクロールさせたい場合はオンにします.|
|`Transform`|`Cell Container`|セルの親要素となる Transform を指定します. |

#### Properties
| Type | Name | Summary |
|:-----------|:------------|:------------|
|`GameObject`|`CellPrefab`|Cell prefab.|
|`IList<TItemData>`|`ItemsSource`|Items source.|
|`TContext`|`Context`|Context.|

#### Methods
| Type | Name | Summary |
|:-----------|:------------|:------------|
|`void`|`UpdateContents(IList<TItemData> itemsSource)`|Updates the contents.|
|`void`|`Refresh()`|Refreshes the cells.|
|`void`|`UpdatePosition(float position)`|Updates the scroll position.|

---
### `FancyScrollViewCell<TItemData, TContext>`
セルの抽象基底クラスです。
```csharp
public abstract class FancyScrollViewCell<TItemData, TContext>
    : MonoBehaviour where TContext : class, new()
```
`Context` が不要な場合はこちらを使用します。
```csharp
public abstract class FancyScrollViewCell<TItemData>
    : FancyScrollViewCell<TItemData, FancyScrollViewNullContext>
```

#### Properties
| Type | Name | Summary |
|:-----------|:------------|:------------|
|`int`|`Index`|Gets or sets the index of the data.|
|`bool`|`IsVisible`|Gets a value indicating whether this cell is visible.|
|`TContext`|`Context`|Context.|

#### Methods
| Type | Name | Summary |
|:-----------|:------------|:------------|
|`void`|`SetupContext(TContext context)`|Setup the context.|
|`void`|`SetVisible(bool visible)`|Sets the visible.|
|`void`|`UpdateContent(TItemData itemData)`|Updates the content.|
|`void`|`UpdatePosition(float position)`|Updates the position.|

---
### `Scroller`
スクロール位置を制御するコンポーネントです。
```csharp
public class Scroller
    : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
```

#### Inspector
| Type | Name | Summary |
|:-----------|:------------|:------------|
|`RectTransform`|`Viewport`|ビューポートとなる RectTransform を指定します.ここで指定された RectTransform の範囲内でジェスチャーの検出を行います.|
|`ScrollDirection`|`Direction Of Recognize`|ジェスチャーを認識する方向を Vertical か Horizontal で指定します.|
|`MovementType`|`Movement Type`|コンテンツがスクロール範囲を越えて移動するときに使用する挙動を指定します.|
|`float`|`Elasticity`|コンテンツがスクロール範囲を越えて移動するときに使用する弾力性の量を指定します.|
|`float`|`Scroll Sensitivity`|スクロールの感度を指定します.|
|`bool`|`Inertia`|慣性のオン/オフを指定します.|
|`float`|`Deceleration Rate`|Inertia がオンの場合のみ有効です.減速率を指定します.|
|`bool`|`Snap.Enable`|Snap を有効にする場合オンにします.|
|`float`|`Snap.Velocity Threshold`|Snap がはじまる閾値となる速度を指定します.|
|`float`|`Snap.Duration`|Snap 時の移動時間を秒数で指定します.|
|`Ease`|`Snap.Easing`|Snap 時の Easing を指定します.|

#### Methods
| Type | Name | Summary |
|:-----------|:------------|:------------|
|`void`|`OnValueChanged(Action<float> callback)`|スクロール位置が変化したときのコールバックを設定します.|
|`void`|`OnSelectionChanged(Action<int> callback)`|選択セルが変化したときのコールバックを設定します.|
|`void`|`JumpTo(int index)`|指定したセルまでジャンプします.|
|`void`|`ScrollTo(int index, float duration, Action onComplete = null)`|指定したセルまでスクロールします.|
|`void`|`ScrollTo(int index, float duration, Ease easing, Action onComplete = null)`|指定したセルまでスクロールします.|
|`void`|`ScrollTo(int index, float duration, Func<float, float> easingFunction, Action onComplete = null)`|指定したセルまでスクロールします.|
|`void`|`SetTotalCount(int totalCount)`|アイテムの総数を設定します. ( index: 0 ~ totalCount - 1 )|

---

## Author
[setchi](https://github.com/setchi)

## License
MIT
