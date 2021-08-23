using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;

namespace Ecommerce.Category {
    public class CategoryCell : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_CategoryName = default;
        [SerializeField] RawImage image = default;
        [SerializeField] Button button = default;

        public int ind;
        public string LocalURL;
        public string serverURL;

        public Texture2D defaultTexture;

        public LoadImageTexture textureLoader;

        public void setCategoryCellData(ItemData itemData)
        {
            m_CategoryName.text = itemData.categoryName;
            LocalURL = itemData.localURL;
            serverURL = itemData.serverURL;
            ind = itemData.index;

            textureLoader.isLocal = false;
            textureLoader.m_LocalURL = itemData.localURL;
            textureLoader.m_ServerURL = itemData.serverURL;
            textureLoader.LoadImage();
        }
    }
}