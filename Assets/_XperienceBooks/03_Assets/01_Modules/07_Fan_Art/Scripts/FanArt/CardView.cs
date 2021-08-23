using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
public class CardView : MonoBehaviour
{

    public Image img;
    public TextMeshProUGUI title;
    public TextMeshProUGUI discription;
    public GameObject fullCardView;
    public Animator anim;

    private void Start()
    {
        if (GameManager.Instance.TitleFont != null)
        {
            title.font = GameManager.Instance.TitleFont;
        }

        if (GameManager.Instance.DetailFont != null)
            discription.font = GameManager.Instance.DetailFont;
    }

    public void ActivateFullView(Texture2D texture , GalleryViewData data) {
        img.sprite = Utility.Texture2DToSprite(texture);
        img.preserveAspect = true;
        title.text = data.m_ArtistName;
        discription.text = data.m_ArtDiscription;
        anim.Play("Window In");
    }


    public void CloseView() {
        anim.Play("Window Out");
        fullCardView.SetActive(false);
    }
}
