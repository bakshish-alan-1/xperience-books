/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample02
{
    class ItemData
    {
        public string Message { get; set; }
        public Texture2D texture;

        public ItemData(string message , Texture2D texture2D)
        {
            Message = message;
            texture = texture2D;
        }

        public ItemData()
        {
        }
    }
}
