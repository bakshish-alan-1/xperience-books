
using System.Collections.Generic;

/*
 *  Product class for store product details
 *  Created by Hardik
 * */

[System.Serializable]
public class Product
{
    public int id; //{ get; set; } // product ID
    public string name; //{ get; set; } // Product name
    public string currency; //{ get; set; } // Product Payment currency Type : for example $, RS , etc.
    public string price; //{ get; set; } // Price of product
    public string desc; //{ get; set; } // Product discription
    public string qty; //{ get; set; } // total quantity of product 
    public List<string> image; //{ get; set; } //Produt image list
    public List<Attributes> attributes; //{ get; set; } // attributes list of product .. like size , color , material etc

}

/*
 *  AttributeSize class for All Atributes detail
 *  Created by Akash
 * */

[System.Serializable]
public class AttributeSize
{
    public int id; //{ get; set; }
    public string size_name; //{ get; set; }
    public string size_price; //{ get; set; }
    public string size_image;//{ get; set; }
    public string size_quantity = "0"; //{ get; set; }
}


/*
 *  Attributes class wrapping all product list in single object for Product class
 *  Created by Hardik
 * */

[System.Serializable]
public class Attributes
{
    public int id; //{ get; set; }
    public string attribute_values_name; //{ get; set; }
    public string color_code; //{ get; set; }
    public string color_name; //{ get; set; }
    public string color_image; //{ get; set; }
    public string attribute_image_filename; //{ get; set; }
    public string color_price; //{ get; set; }
    public string color_quantity = "0"; //{ get; set; }

    public List<AttributeSize> sizes; //{ get; set; }
}
