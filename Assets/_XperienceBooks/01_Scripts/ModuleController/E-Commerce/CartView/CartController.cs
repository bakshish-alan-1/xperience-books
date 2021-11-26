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

    public bool OnCheckItemQuantity(int id, int maxQty)
    {
        bool available = true;
        int qty = 0;
        for (int i = 0; i < container.transform.childCount; i++)
        {
            if (container.transform.GetChild(i).GetComponent<CartProductCell>().cartProduct.isActive)
            {
                if (id == container.transform.GetChild(i).GetComponent<CartProductCell>().cartProduct.m_product.id)
                    qty += container.transform.GetChild(i).GetComponent<CartProductCell>().qty;
            }
        }

        Debug.Log("Qty: " + qty + ", " + maxQty);
        if (qty >= maxQty)
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
