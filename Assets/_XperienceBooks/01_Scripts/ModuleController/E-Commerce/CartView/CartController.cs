using System.Collections;
using System.Collections.Generic;
using Ecommerce;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

public class CartController : MonoBehaviour
{
    public static CartController Instance = null;


    public GameObject cartPrefab;
    public GameObject container;
    public Button placeBtnOrader;
    public List<CartProduct> m_FinalCart = new List<CartProduct>();
    public TextMeshProUGUI t_continueToCheckout;

    public float t_TotalCheckoutPrice = 0;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        placeBtnOrader.interactable = false;
    }


    public void UpdateCart(CartProduct product) {
        GameObject obj = Instantiate(cartPrefab, container.transform,false);
        obj.GetComponent<CartProductCell>().SetData(product, m_FinalCart.Count);
        m_FinalCart.Add(obj.GetComponent<CartProductCell>().cartProduct);

        UpdateFinalPrice();
    }

    public void ClearCartList()
    {
        m_FinalCart.Clear();
        t_TotalCheckoutPrice = 0;
        t_continueToCheckout.text = "Place this order: $ " + t_TotalCheckoutPrice;
        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }
    }

    public bool OnCheckItemQuantity(int id, int orderQty, int selectedColor, int selectedAttribute)
    {
        Debug.Log("OnCheckItemQuantity color: " + selectedColor + ", size: " + selectedAttribute);
        bool available = true;
        int qty = 0;
        List<int> nodes = new List<int>();

        for (int i = 0; i < container.transform.childCount; i++)
        {
            if (container.transform.GetChild(i).GetComponent<CartProductCell>().cartProduct.isActive)
            {
                if (id == container.transform.GetChild(i).GetComponent<CartProductCell>().cartProduct.m_product.id)
                {
                    CartProductCell cartProductObj = container.transform.GetChild(i).GetComponent<CartProductCell>();
                    if (cartProductObj.cartProduct.m_SelectedAttributes[0] >= 0 && cartProductObj.cartProduct.m_SelectedAttributes[0] == selectedColor && cartProductObj.cartProduct.m_SelectedAttributes[1] == selectedAttribute)
                    {
                        nodes.Add(i);
                    }
                    else if (cartProductObj.cartProduct.m_SelectedAttributes[1] >= 0 && cartProductObj.cartProduct.m_SelectedAttributes[0] == selectedColor && cartProductObj.cartProduct.m_SelectedAttributes[1] == selectedAttribute)
                    {
                        nodes.Add(i);
                    }
                }
            }
        }

        Debug.Log("node size: " + nodes.Count);
        if (nodes.Count == 0)
        {
            return true;
        }

        for (int j = 0; j < nodes.Count; j++)
        {
            Debug.Log("nodes[" + j + "]: " + nodes[j]);
            CartProductCell cartProductObj = container.transform.GetChild(nodes[j]).GetComponent<CartProductCell>();
            if (cartProductObj.cartProduct.m_SelectedAttributes[0] >= 0)
            {
                qty += cartProductObj.qty;
            }
            else if (cartProductObj.cartProduct.m_SelectedAttributes[1] >= 0)
            {
                qty += cartProductObj.qty;
            }
        }

        Debug.Log("Qty: " + qty + ", " + orderQty);
        if (qty >= orderQty)
            available = false;

        return available;
    }

    public void UpdateFinalPrice() {

        t_TotalCheckoutPrice = m_FinalCart.Where(X => X.isActive == true).Select(Y => Y.m_FinalPrice).Sum();

        t_continueToCheckout.text = "Place this order: $ " + t_TotalCheckoutPrice;

        if (t_TotalCheckoutPrice > 0)
            placeBtnOrader.interactable = true;
        else
            placeBtnOrader.interactable = false;
    }
}
