using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.Extensions;

namespace Ecommerce.Category {
    public class Cell : FancyGridViewCell<ItemData, Context>
    {
        [SerializeField] TextMeshProUGUI m_CategoryName = default;
        [SerializeField] RawImage image = default;
        [SerializeField] Button button = default;

        public int ind;
        public string LocalURL;
        public string serverURL;

        public Texture2D defaultTexture;

        public LoadImageTexture textureLoader;

        public override void Initialize()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        }

        public override void UpdateContent(ItemData itemData)
        {
            m_CategoryName.text = itemData.categoryName;

            LocalURL = itemData.localURL;
            serverURL = itemData.serverURL;
            ind = itemData.index;

            //image.texture = defaultTexture;
            textureLoader.isLocal = false;
            textureLoader.m_LocalURL = itemData.localURL;
            textureLoader.m_ServerURL = itemData.serverURL;
            textureLoader.LoadImage();
        }

        protected override void UpdatePosition(float normalizedPosition, float localPosition)
        {
            base.UpdatePosition(normalizedPosition, localPosition);

            transform.localPosition += Vector3.right * 1;
        }
    }
}