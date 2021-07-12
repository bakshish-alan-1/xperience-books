using UnityEngine;
using UnityEngine.UI;

public class ContentButton : MonoBehaviour
{

    private Image m_Sprite;
    private Button m_ContentButton;
    public int m_ContentIndex;
    // Start is called before the first frame update


    private void Awake()
    {

        m_ContentButton = GetComponent<Button>();
        m_Sprite = GetComponent<Image>();
    }

    void Start()
    {

        m_ContentButton.onClick.AddListener(() =>
        {
            OnItemClicked();
        });
    }



    public void SetData(Sprite img, int index) {


        m_Sprite.sprite = img;
        m_ContentIndex = index;
    }

    private void OnItemClicked()
    {
        GameManager.Instance.GetModuleData(m_ContentIndex);
    }
}
