# FancyScrollView

[![license](https://img.shields.io/badge/license-MIT-green.svg?style=flat&cacheSeconds=2592000)](https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
[![WebGL Demo](https://img.shields.io/badge/demo-WebGL-orange.svg?style=flat&logo=google-chrome&logoColor=white&cacheSeconds=2592000)](https://setchi.jp/FancyScrollView/demo)
[![API Documentation](https://img.shields.io/badge/API-Documentation-ff69b4.svg?style=flat&logo=c-sharp&cacheSeconds=2592000)](https://setchi.jp/FancyScrollView/api/FancyScrollView.html)
[![openupm](https://img.shields.io/npm/v/jp.setchi.fancyscrollview?label=openupm&registry_uri=https://package.openupm.com&style=flat)](https://openupm.com/packages/jp.setchi.fancyscrollview/)

A generic ScrollView component that allows you to implement highly flexible animations. It also supports infinite scrolling.

<img src="https://user-images.githubusercontent.com/8326814/69004520-d2b36b80-0957-11ea-8277-06bfd3e8f033.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/70638335-0b571400-1c7c-11ea-8701-a0d1ae0cb7e3.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/59548448-a3549900-8f8a-11e9-9a27-b04f1410a7b5.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/59548462-b8c9c300-8f8a-11e9-8985-5f1c2e610309.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/59550410-7f528100-8fa5-11e9-8f1b-41e59b645571.gif" width="320"><img src="https://user-images.githubusercontent.com/8326814/59550411-7f528100-8fa5-11e9-8bfb-bd42da47f7a0.gif" width="320">

## Requirements
[![Unity 2019.4+](https://img.shields.io/badge/unity-2019.4+-black.svg?style=flat&logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)
[![.NET 4.x Scripting Runtime](https://img.shields.io/badge/.NET-4.x-blueviolet.svg?style=flat&cacheSeconds=2592000)](https://docs.unity3d.com/2018.3/Documentation/Manual/ScriptingRuntimeUpgrade.html)

## Installation
### Unity Asset Store
Purchase from the [Unity Asset Store](https://assetstore.unity.com/packages/tools/gui/fancyscrollview-96530) and consider further development support.

### OpenUPM
Add the package to the Unity Project from the [OpenUPM](https://openupm.com/) registry.

```
openupm add jp.setchi.fancyscrollview
```

### Unity Package Manager
Add a reference to the repository to [`Packages/manifest.json`](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html#project-manifests) file in your project directory.

```json
{
  "dependencies": {
    "jp.setchi.fancyscrollview": "https://github.com/setchi/FancyScrollView.git#upm"
  }
}
```

## Features
### You can freely implement scroll animation
When FancyScrollView updates the scroll position, it passes the normalized position of the viewport range to each cell. On the cell side, [a cell itself controls the scrolling position and appearance](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyCell-2.html#FancyScrollView_FancyCell_2_UpdatePosition_System_Single_) based on the value between `0.0`-`1.0`.
The sample uses Animator and mathematical formulas to implement scrolling movements.

### It works fast even if the number of data is large
Only the number of cells needed for display will be generated and the cells will be reused. You can check the operation while actually increasing the number of data in this [Demo](https://setchi.jp/FancyScrollView/demo/). In [FancyScrollRect](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyScrollRect-2.html) and [FancyGridView](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyGridView-2.html), [number of cell before and after reuse](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyScrollRect-2.html#FancyScrollView_FancyScrollRect_2_reuseCellMarginCount) can also be specified.

### You can freely exchange messages between cells and scroll views
With `Context`, you can simply implement the process of detecting that a cell has been clicked in the scroll view and issuing instructions to the cell from the scroll view. An implementation example（[Examples/02_FocusOn](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/Sources/02_FocusOn)）is included, so please refer to it.

### You can scroll or jump to a specific cell
You can also specify the number of seconds to move and Easing. See [Class Scroller](https://setchi.jp/FancyScrollView/api/FancyScrollView.Scroller.html#FancyScrollView_Scroller_ScrollTo_System_Single_System_Single_EasingCore_Ease_System_Action_) in [API Documentation](https://setchi.jp/FancyScrollView/api/FancyScrollView.html) for more information.

### You can fine tune the scroll behavior
You can set the behavior related to scrolling, such as the presence or absence of inertia and the deceleration rate. See [Class Scroller](https://setchi.jp/FancyScrollView/api/FancyScrollView.Scroller.html) in [API Documentation](https://setchi.jp/FancyScrollView/api/FancyScrollView.html) for more information.

### Supports snaps
When snapping is enabled, it will move to the nearest cell just before scrolling stops. You can specify a threshold for the speed at which snaps start, the number of seconds to move, and Easing. [FancyScrollRect](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyScrollRect-2.html) and [FancyGridView](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyGridView-2.html) do not support snapping.

### Supports infinite scrolling
Infinite scrolling can be implemented by setting the following in Inspector
1. When `Loop` in `FancyScrollView` is enabled, the cells will loop with the last cell before the first cell and the first cell after the last cell.
1. By setting `Movement Type` to `Unrestricted` in `Scroller`, scroll range can be made unrestricted. Infinite scrolling can be achieved by combining with (1).

An implementation example（[Examples/03_InfiniteScroll](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples)）is included, so please refer to it as well. [FancyScrollRect](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyScrollRect-2.html) and [FancyGridView](https://setchi.jp/FancyScrollView/api/FancyScrollView.FancyGridView-2.html) do not support infinite scrolling.

## Examples
[![WebGL Demo](https://img.shields.io/badge/demo-WebGL-orange.svg?style=flat&logo=google-chrome&logoColor=white&cacheSeconds=2592000)](https://setchi.jp/FancyScrollView/demo)

[FancyScrollView/Examples](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples) を参照してください。

| Name | Description |
|:-----------|:------------|
|01_Basic|This is an implementation example of the simplest configuration.|
|02_FocusOn|This is an implementation example that focuses on the left and right cells with a button.|
|03_InfiniteScroll|This is an implementation example of infinite scrolling.|
|04_Metaball|This is an example of implementing a metaball using a shader.|
|05_Voronoi| This is an example of Voronoi implementation using a shader.|
|06_LoopTabBar|This is an implementation example of switching screens with tabs.|
|07_ScrollRect|This is an implementation example of a style with a scroll bar in `ScrollRect`|
|08_GridView|This is an implementation example of grid layout.|
|09_LoadTexture|This is an implementation example of loading and displaying a texture.|

## Usage
In the simplest configuration,

- Object for passing data to a cell
- Cell
- Scroll View

needs to be implemented.

### Implementation
Define an object to pass data to a cell.
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
Implement your own cell by inheriting `FancyCell<TItemData>`.
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
        // position is a value between 0.0 and 1.0 
        // You can freely control the appearance of scrolling based on position
    }
}
```
Implement your own scroll view by inheriting `FancyScrollView<TItemData>`.
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
Pour data into the scroll view.
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

See [Examples](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples) and [API Documentation](https://setchi.jp/FancyScrollView/api/FancyScrollView.html) for more details.

## Author
[setchi](https://github.com/setchi)

## License
[MIT](https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
