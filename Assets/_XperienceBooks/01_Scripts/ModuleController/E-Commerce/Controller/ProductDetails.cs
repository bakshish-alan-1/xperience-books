using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Ecommerce
{
    public class ProductDetails : MonoBehaviour
    {
        public static ProductDetails Instance = null;

        #region Product_ScrollSnap
        [SerializeField]
        protected GameObject panel, toggle, clearfillterBtn;
        [SerializeField]
        private float toggleWidth;
        [SerializeField]
        private SimpleScrollSnap sss;
        #endregion

        public Product m_ProductDetails;

        [SerializeField]
        TextMeshProUGUI m_ProductName, m_ProductPrice, m_ProductDiscription;

        [SerializeField]
        ProductAttributes color;

        #region ProductInfo

        public float m_ColorAdditionalPrice = 0.0f;
        public float m_SizeAdditionalPrice = 0.0f;


        float FinalPrice = 0.0f;
        float productPrice = 0.0f;
        #endregion

        public int SelectedColor = -1;
        public int SelectedSize = -1;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            clearfillterBtn.SetActive(false);
            // toggleWidth = toggle.GetComponent<RectTransform>().sizeDelta.x * (Screen.width / 2048f);


            /* for (int i = 0; i < 10; i++)
             {
                 Add(sss.NumberOfPanels);
             }
            */
        }

        private void OnEnable()
        {
            ToggleSelector.OnSelected += ToggleSelected;
        }

        private void OnDisable()
        {
            ToggleSelector.OnSelected -= ToggleSelected;
        }

        public void ToggleSelected(int index, int type)
        {
            try
            {
                switch (type)
                {
                    case 0:

                        Debug.Log(" color: " + index + " Type : " + type);
                        clearfillterBtn.SetActive(true);
                        SelectedColor = index;
                        m_ColorAdditionalPrice = float.Parse(m_ProductDetails.attributes[SelectedColor].color_price);
                        FinalPrice = m_ColorAdditionalPrice;

                        Reset();
                        Add(sss.NumberOfPanels, "", m_ProductDetails.attributes[SelectedColor].color_image);

                        color.ResetSizeAttributeObject();
                        if (m_ProductDetails.attributes[SelectedColor].sizes.Count >= 1)
                        {
                            color.sizeAttribute.SetActive(true);
                            color.sizeObjectList.Clear();
                            for (int j = 0; j < m_ProductDetails.attributes[SelectedColor].sizes.Count; j++)
                            {
                                color.LoadSizeAttributesData(j, m_ProductDetails.attributes[SelectedColor].sizes[j].size_name);
                            }
                        }
                        else
                        {
                            color.sizeAttribute.SetActive(false);
                            SelectedSize = -1;
                        }
                        break;
                    case 1:

                        Debug.Log("size of colorindex: " + SelectedColor + ", size index: " + index);
                        SelectedSize = index;
                        m_SizeAdditionalPrice = float.Parse(m_ProductDetails.attributes[SelectedColor].sizes[SelectedSize].size_price);
                        FinalPrice = m_SizeAdditionalPrice;
                        break;
                }

                //   float total = 0.0f;

                //FinalPrice = float.Parse(m_ProductDetails.price) + m_ColorAdditionalPrice + m_SizeAdditionalPrice;

                m_ProductPrice.text = "$" + FinalPrice;
            }
            catch (Exception ex)
            {
                Debug.LogError("Issue in atribute selection :" + ex.Message);
            }
        }

        public void clearFilterSelection()
        {
            ResetAll();
            Reset();
            color.objectList.Clear();
            m_ProductPrice.text = "$" + productPrice;
            FinalPrice = productPrice;

            Invoke("createColorAttribute", 0.3f);

            for (int i = 0; i < m_ProductDetails.image.Count; i++)
            {
                Add(sss.NumberOfPanels, "", m_ProductDetails.image[i]);
            }
        }

        void createColorAttribute()
        {
            if (m_ProductDetails.attributes.Count >= 1)
            {
                color.gameObject.SetActive(true);
                for (int i = 0; i < m_ProductDetails.attributes.Count; i++)
                {
                    color.LoadAttributeData(m_ProductDetails.attributes[i], i, "Color");
                }
            }
        }

        public void UpdateProductDetails(Product product)
        {
            try
            {
                m_ProductDetails = new Product();
                m_ProductDetails = product;
                SelectedColor = -1;
                SelectedSize = -1;

                for (int i = 0; i < m_ProductDetails.image.Count; i++)
                {
                    Add(sss.NumberOfPanels, "", product.image[i]);
                }

                m_ProductName.text = product.name;
                m_ProductPrice.text = "$"+product.price;
                m_ProductDiscription.text = product.desc;
                FinalPrice = float.Parse(product.price);
                productPrice = FinalPrice;

                color.gameObject.SetActive(false);

                if (product.attributes.Count >= 1) {
                    color.ResetColorAttributeObject();
                    color.gameObject.SetActive(true);
                    for (int i = 0; i < product.attributes.Count; i++)
                    {
                        color.LoadAttributeData(product.attributes[i], i, "Color");
                    }
                }
            }
            catch (Exception EX)
            {
                Debug.LogError("ProductDetails : " + EX.Message);
            }

        }

        public void ResetAll()
        {
            color.ResetObject();
            clearfillterBtn.SetActive(false);
            color.sizeAttribute.SetActive(false);
            color.ResetColorAttributeObject();
            color.ResetSizeAttributeObject();
            SelectedColor = -1;
            SelectedSize = -1;
        }

        #region ScrollSnap Logic

        private void Add(int index,string localURL , string serverURL ,bool isLocal = false)
        {
            //Pagination
            Instantiate(toggle, sss.pagination.transform.position + new Vector3(toggleWidth * (sss.NumberOfPanels + 1), 0, 0), Quaternion.identity, sss.pagination.transform);
            sss.pagination.transform.position -= new Vector3(toggleWidth / 2f, 0, 0);

            //Panel
           // panel.GetComponent<Image>().color = new Color(UnityEngine.Random.Range(0, 255) / 255f, UnityEngine.Random.Range(0, 255) / 255f, UnityEngine.Random.Range(0, 255) / 255f);

            //need to set local , server path runtime **** Remain ****
            panel.GetComponent<LoadImageTexture>().m_LocalURL = localURL;
            panel.GetComponent<LoadImageTexture>().m_ServerURL = serverURL;
            panel.GetComponent<LoadImageTexture>().isLocal = isLocal;
            sss.Add(panel, index);

        }

        public void Reset()
        {
            if (sss.NumberOfPanels > 0)
            {

                for (int i = sss.NumberOfPanels-1; i >=0; i--)
                {
                    //Pagination
                    DestroyImmediate(sss.pagination.transform.GetChild(sss.NumberOfPanels - 1).gameObject);
                    sss.pagination.transform.position += new Vector3(toggleWidth / 2f, 0, 0);

                    //Panel
                    sss.Remove(i);

                }
            }
        }

        #endregion

        public void AddToCart(bool callFromBuyNowBtn) {

            if (CartController.Instance.OnCheckItemQuantity(m_ProductDetails.id, int.Parse(m_ProductDetails.qty)))
            {
                Reset();

                color.ResetObject();
                color.sizeAttribute.SetActive(false);
                CartProduct product = new CartProduct();

                product.m_product = m_ProductDetails;
                product.m_SelectedAttributes.Add(SelectedColor);
                product.m_SelectedAttributes.Add(SelectedSize);
                product.m_TotalQty = 1;

                product.m_FinalPrice = FinalPrice;
                Debug.Log("AddToCart product: " + product.ToString());
                ECommerceManager.Instance.AddToCart(product, callFromBuyNowBtn);
            }
            else
            {
                ApiManager.Instance.errorWindow.SetErrorMessage("", "Currently unavailable.", "OKAY", ErrorWindow.ResponseData.JustClose, false);
            }
        }

        private bool OnCheckItemQuantity(int id, int maxQty)
        {
            bool available = true;
            int qty = 0;
            for (int i = 0; i < CartController.Instance.container.transform.childCount; i++)
            {
                if (CartController.Instance.container.transform.GetChild(i).GetComponent<CartProductCell>().cartProduct.isActive)
                {
                    if (id == CartController.Instance.container.transform.GetChild(i).GetComponent<CartProductCell>().cartProduct.m_product.id)
                        qty += CartController.Instance.container.transform.GetChild(i).GetComponent<CartProductCell>().qty;
                }
            }

            Debug.Log("Qty: " + qty + ", " + maxQty);
            if (qty >= maxQty)
                available = false;

            return available;
        }
    }
}
