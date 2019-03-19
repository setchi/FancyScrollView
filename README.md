# FancyScrollView [![license](https://img.shields.io/badge/license-MIT-green.svg?style=flat-square)](https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
高度に柔軟なアニメーションを実装できる汎用のScrollViewコンポーネントです。 無限スクロールもサポートしています。[English](https://translate.google.com/translate?sl=ja&tl=en&u=https://github.com/setchi/FancyScrollView) (by Google Translate)

![screencast](Documents/logo.png)
![screencast](Documents/screencast1.gif)
![screencast](Documents/screencast2.gif)

## 導入
Unity 2017.1.0 (C# 6.0) 以降が必要です。
このリポジトリを Clone するか、 [Asset Store](https://assetstore.unity.com/packages/tools/gui/fancyscrollview-96530) からプロジェクトにインポートしてください。

## サンプル
[FancyScrollView/Examples/Scenes](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/Scenes/) を参照してください。

| サンプル名 | 説明 |
|:-----------|:------------|
|01_Basic|最もシンプルな構成の実装例です。|
|02_CellEventHandling|セル内で発生したイベントをハンドリングする実装例です。|
|03_InfiniteScroll|無限スクロールの実装例です。|
|04_FocusOn|ボタンで左右のセルにフォーカスする実装例です。|

## 仕組み
FancyScrollView はセルの位置を更新するとき、可視領域の正規化された値を各セルに渡します。 セル側では、0.0 ~ 1.0 の値に基づいてスクロールの外観を自由に制御できます。

## 使い方
もっともシンプルな構成では、

- セルにデータを渡すためのオブジェクト
- セル
- スクロールビュー

の実装が必要です。

### スクリプトの実装
セルにデータを渡すためのオブジェクトを定義します。
```csharp
public class MyCellData
{
    public string Message;
}
```
`FancyScrollViewCell<TCellData>` を継承して自分のセルを実装します。
```csharp
using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;

public class MyScrollViewCell : FancyScrollViewCell<MyCellData>
{
    [SerializeField] Text message;

    public override void UpdateContent(MyCellData cellData)
    {
        message.text = cellData.Message;
    }

    public override void UpdatePosition(float position)
    {
        // position は 0.0 ~ 1.0 の値です
        // position に基づいてスクロールの外観を自由に制御できます
    }
}
```
`FancyScrollView<TCellData>` を継承して自分のスクロールビューを実装します。
```csharp
using UnityEngine;
using System.Linq;
using FancyScrollView;

public class MyScrollView : FancyScrollView<MyCellData>
{
    [SerializeField] ScrollPositionController scrollPositionController;
    [SerializeField] GameObject cellPrefab;

    protected override GameObject CellPrefab => cellPrefab;

    void Start()
    {
        scrollPositionController.OnUpdatePosition(p => base.UpdatePosition(p));
    }

    public void UpdateData(IList<MyCellData> cellData)
    {
        base.UpdateContents(cellData);
        scrollPositionController.SetDataCount(cellData.Count);
    }
}
```
スクロールビューにデータを流し込みます。
```csharp
using UnityEngine;
using System.Linq;
using FancyScrollView;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] MyScrollView myScrollView;

    void Start()
    {
        var cellData = Enumerable.Range(0, 50)
            .Select(i => new MyCellData {Message = $"Cell {i}"})
            .ToArray();

        myScrollView.UpdateData(cellData);
    }
}
```

### インスペクタ上の設定
![screencast](Documents/inspector.png)
#### My Scroll View
| プロパティ | 説明 |
|:-----------|:------------|
|Cell Spacing|セル同士の間隔を float.Epsilon ~ 1.0 の間で指定します。|
|Scroll Offset|スクロールのオフセットを指定します。 たとえば、 0.5 を指定してスクロール位置が 0 の場合、最初のセルの位置は 0.5 になります。|
|Loop|オンにするとセルが循環し、最初のセルの前に最後のセル、最後のセルの後に最初のセルが並ぶようになります。無限スクロールさせたい場合はオンにします。|
|Cell Prefab|セルの Prefab を指定します。|
|Cell Container| セルの親要素となる Transform を指定します。 |

#### Scroll Position Controller
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
|Data Count|アイテムのデータ件数の総数です。基本的にスクリプトから設定します。|

## Q&A

#### データ件数が多くてもパフォーマンスは大丈夫？
表示に必要なセル数のみが生成されるため、データ件数がパフォーマンスに与える影響はわずかです。 セル間のスペース（同時に存在するセルの数）とセルの演出は、データ件数よりもパフォーマンスに大きな影響を与えます。

#### 自分でスクロール位置を制御したいんだけど？
`ScrollPositionController` の下記の API を使用できます。
```csharp
public void ScrollTo(int index, float duration)
```
```csharp
public void JumpTo(int index)
```
また、サンプルで使われている `ScrollPositionController` を使わずにあなた自身の実装で全く違った振る舞いをさせることもできます。

#### セルで発生したイベントを受け取れる？
セル内で発生したあらゆるイベントをハンドリングできます。セル内で発生したイベントをハンドリングする実装例（[Examples/02_CellEventHandling](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/02_CellEventHandling)）が含まれていますので、参考にして実装してください。

#### セルを無限スクロール（ループ）させたいんだけど？
無限スクロールをサポートしています。実装手順は下記の通りです。
1. `ScrollView` の `Loop` をオンにするとセルが循環し、最初のセルの前に最後のセル、最後のセルの後に最初のセルが並ぶようになります。
1. サンプルで使用されている `ScrollPositionController` を使うときは、 `Movement Type` を `Unrestricted` に設定することで、スクロール範囲が無制限になります。 1. と組み合わせることで無限スクロールを実現できます。

実装例（[Examples/03_InfiniteScroll](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/03_InfiniteScroll)）が含まれていますので、こちらも参考にしてください。

![screencast](Documents/infiniteScrollSettings.png)

## 開発環境
Unity 2018.2.11f1

## Author
[setchi](https://github.com/setchi)

## License
MIT
