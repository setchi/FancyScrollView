# FancyScrollView [![license](https://img.shields.io/badge/license-MIT-green.svg?style=flat-square)](https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
A generic ScrollView component that can implement highly flexible animations. It also supports infinite scrolling.

![screencast](Documents/logo.png)
![screencast](Documents/screencast1.gif)
![screencast](Documents/screencast2.gif)

## Introduction
Requires Unity 2018.3 (C # 7.3) or later. Clone this repository 
or import it from the [Asset Store](https://assetstore.unity.com/packages/tools/gui/fancyscrollview-96530) into the project.

## sample
[FancyScrollView/Examples/Scenes](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/Scenes/) を参照してください。

| Sample name | 	Description |
|:-----------|:------------|
|01_Basic|This is an example of the simplest configuration.|
|02_CellEventHandling|This is an implementation example of handling an event that occurred in a cell.|
|03_InfiniteScroll|It is an implementation example of infinite scroll.|
|04_FocusOn|This is an implementation example that focuses on the left and right cells with a button.。|

## How it works
When updating the position of a cell, FancyScrollView passes 
the normalized value of the visible area to each cell. On the 
cell side, you have complete control over the appearance of 
the scroll based on values ​​between 0.0 and 1.0.

## How to use
In the simplest configuration,

- An object for passing data to a cell
- cell
- Scroll view

Implementation is required.

### Script implementation
Defines an object for passing data to cells.
```csharp
public class ItemData
{
    public string Message;
}
```
Implement your own cell by inheriting `FancyScrollViewCell<TItemData>`.
```csharp
using UnityEngine;
using UnityEngine.UI;
using FancyScrollView;

public class MyScrollViewCell : FancyScrollViewCell<ItemData>
{
    [SerializeField] Text message;

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
Implement your own scroll view by inheriting `FancyScrollView<TItemData>`.
```csharp
using UnityEngine;
using System.Linq;
using FancyScrollView;

public class MyScrollView : FancyScrollView<ItemData>
{
    [SerializeField] Scroller scroller;
    [SerializeField] GameObject cellPrefab;

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

Flow data into the scroll view.
```csharp
using UnityEngine;
using System.Linq;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] MyScrollView myScrollView;

    void Start()
    {
        var items = Enumerable.Range(0, 50)
            .Select(i => new ItemData {Message = $"Cell {i}"})
            .ToArray();

        myScrollView.UpdateData(items);
    }
}
```

### Settings on the inspector
#### My Scroll View
| Property | Description |
|:-----------|:------------|
|Cell Spacing|Specify the spacing between cells between float.Epsilon ~ 1.0.|
|Scroll Offset|Specifies the scroll offset. For example, if you specify 0.5 and the scroll position is 0, then the position of the first cell is 0.5.|
|Loop|When turned on, the cells will cycle, with the last cell before the first cell and the first cell after the last cell. Turn on if you want to scroll infinitely.|
|Cell Prefab|Specifies the cell's Prefab.|
|Cell Container|Specifies the Transform that is the parent element of the cell.|

#### Scroller
| Property | Description |
|:-----------|:------------|
|Viewport|Specifies a RectTransform to be a viewport. Gesture detection is performed within the range of RectTransform specified here.|
|Direction Of Recognize|Specify the direction to recognize gesture as Vertical or Horizontal.|
|Movement Type|	Specifies the behavior to use when content moves beyond the scroll range.|
|Elasticity|Specifies the amount of elasticity to use when content moves beyond the scroll range.|
|Scroll Sensitivity|Specifies the scroll sensitivity.|
|Inertia|Specifies inertia on / off.|
|Deceleration Rate|Valid only when Inertia is on. Specifies the deceleration rate.|
|Snap - Enable|Turn on Snap to enable.|
|Snap - Velocity Threshold|Specifies the threshold speed at which Snap starts.|
|Snap - Duration|Specify the move time for Snap in seconds.|

## FAQ

#### Is performance OK even if the number of data is large?
Because only the number of cells needed to display is generated, 
the impact of data count on performance is minimal. The space 
between cells (the number of simultaneously existing cells) and 
the representation of cells have a greater effect on performance 
than the number of data.

#### You want to control the scroll position yourself?
You can update the scroll position using the following APIs of `FancyScrollView`.
```csharp
protected void UpdatePosition(float position)
```
If you use the `Scroller` used in the sample, you can use 
the following API to update the scroll position of `FancyScrollView`.
```csharp
public void ScrollTo(int index, float duration)
```
```csharp
public void JumpTo(int index)
```
```csharp
public void OnValueChanged(Action<float> callback)
```
You can even make your own implementation behave completely 
differently without using `Scroller`.

#### Can I get an event that occurred in a cell?
It can handle any event that occurred in the cell. An 
implementation example ([Examples/02_CellEventHandling](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/02_CellEventHandling)) 
for handling events generated in a cell is included. Please 
refer to the implementation.

#### I want to scroll the cell infinitely (loop).
Supports infinite scrolling. The mounting procedure is as follows.
1. `ScrollView` you turn on `Loop` the cells will cycle, so the 
last cell before the first cell and the first cell after the 
last cell.
1. When using the `Scroller` used in the sample, setting the 
`Movement Type` to  `Unrestricted` makes the scroll range 
unlimited. You can achieve infinite scrolling by combining with 1.

An implementation example ([Examples/03_InfiniteScroll](https://github.com/setchi/FancyScrollView/tree/master/Assets/FancyScrollView/Examples/03_InfiniteScroll)) 
is included, so please refer to this as well.

## Development environment
Unity 2018.3.6f1

## Author
[setchi](https://github.com/setchi)

## License
MIT
