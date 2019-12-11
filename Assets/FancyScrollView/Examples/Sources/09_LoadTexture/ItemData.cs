/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2019 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

namespace FancyScrollView.Example09
{
    public class ItemData
    {
        public string Title { get; }
        public string Description { get; }
        public string Url { get; }

        public ItemData(string title, string description, string url)
        {
            Title = title;
            Description = description;
            Url = url;
        }
    }
}
