/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

namespace UnityEngine.UI.Extensions.Examples.FancyScrollViewExample07
{
    class RowData : FancyScrollRectCell<ItemData, Context>
    {
        [SerializeField] Text message = default;
       // [SerializeField] Image image = default;
       // [SerializeField] Button button = default;
        [SerializeField] Product data = new Product();

        public override void Initialize()
        {
           // button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        }

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.message;
            data = itemData.m_Data;

            Debug.Log(data.id + " " + data.name);
            var selected = Context.SelectedIndex == Index;
         //   image.color = selected
             //   ? new Color32(0, 255, 255, 100)
             //   : new Color32(255, 255, 255, 77);
        }

        protected override void UpdatePosition(float normalizedPosition, float localPosition)
        {
            base.UpdatePosition(normalizedPosition, localPosition);

           // var wave = Mathf.Sin(normalizedPosition * Mathf.PI * 2) * 65;
            transform.localPosition += Vector3.right * 1;
        }
    }
}
