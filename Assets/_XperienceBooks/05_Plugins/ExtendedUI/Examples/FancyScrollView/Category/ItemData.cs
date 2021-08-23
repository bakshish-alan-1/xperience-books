using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecommerce.Category
{
    public class ItemData
    {
        public int index;
        public string categoryName;
        public string localURL;
        public string serverURL;

         public ItemData(int index, string localURL,string serverURL, string categoryName) {
             this.index = index;
             this.localURL = localURL;
             this.serverURL = serverURL;
             this.categoryName = categoryName;
         }

      //  public ItemData(int index) => this.index = index;
    }
}