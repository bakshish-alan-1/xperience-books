using System;
using UnityEngine.UI.Extensions;

namespace Ecommerce.Category
{
    public class Context : FancyGridViewContext
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}