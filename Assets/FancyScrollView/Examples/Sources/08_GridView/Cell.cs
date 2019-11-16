using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example08
{
    public class Cell : FancyScrollViewCell<ItemData, Context>
    {
        [SerializeField] Text message = default;
        [SerializeField] Image image = default;

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Index.ToString();

            var selected = Context.SelectedItemIndex == Index;
            image.color = selected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
        }

        public override void UpdatePosition(float position) { }
    }
}
