using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{


    [SerializeField]
    TextMeshProUGUI TitleText, Message, ButtonText;

    [SerializeField]
    Button button;


    public void SetDialogBox(string title, string message, string buttonText,bool isButtonRequired = true, bool isButtonActive = true) {

        TitleText.text = title;
        Message.text = message;
        ButtonText.text = buttonText;

        if (isButtonRequired)
            button.gameObject.SetActive(true);
        else
            button.gameObject.SetActive(false);

        button.interactable = isButtonActive;
    }

    public void ButtonAction() {
        Debug.Log("ButtonClick");
        this.gameObject.SetActive(false);
        if (ButtonText.text == "Done")
        {
            Ecommerce.ProductList.Instance.RefreshListData();
            Ecommerce.ECommerceManager.Instance.GetFeaturedProduct();
            CartController.Instance.ClearCartList();
        }
    }
}
