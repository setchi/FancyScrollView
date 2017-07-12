using UnityEngine;

public class FancyScrollViewCell<TData, TContext> : MonoBehaviour where TContext : class
{
    /// <summary>
    /// コンテキストを設定します
    /// </summary>
    /// <param name="context"></param>
    public virtual void SetContext(TContext context)
    {
    }

    /// <summary>
    /// セルの内容を更新します
    /// </summary>
    /// <param name="itemData"></param>
    public virtual void UpdateContent(TData itemData)
    {
    }

    /// <summary>
    /// セルの位置を更新します
    /// </summary>
    /// <param name="position"></param>
    public virtual void UpdatePosition(float position)
    {
    }

    /// <summary>
    /// セルの表示/非表示を設定します
    /// </summary>
    /// <param name="visible"></param>
    public virtual void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    /// <summary>
    /// このセルで表示しているデータのインデックス
    /// </summary>
    public int DataIndex { get; set; }
}

public class FancyScrollViewCell<TData> : FancyScrollViewCell<TData, FancyScrollViewNullContext>
{

}
