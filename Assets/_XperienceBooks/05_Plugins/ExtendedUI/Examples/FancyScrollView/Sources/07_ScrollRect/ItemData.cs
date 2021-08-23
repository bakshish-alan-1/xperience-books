/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample07
{
    class ItemData
    {
        public string message { get; }
        public Product m_Data;

        public ItemData(string data , Product product)
        {
            message = data;
            m_Data = product;
        }
    }
}
