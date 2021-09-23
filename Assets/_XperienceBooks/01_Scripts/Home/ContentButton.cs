using System.IO;
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



    public void SetData(int moduleNo, Sprite img, int index) {

        Debug.Log("data from call : " + moduleNo);

        string name = "module_" + (moduleNo + 1) + ".png";

        if (File.Exists(GameManager.Instance.GetThemePath() + "/" + name))
            ThemeManager.Instance.OnLoadImage(GameManager.Instance.GetThemePath(), name, m_Sprite);
        else
            m_Sprite.sprite = img;

        m_ContentIndex = index;
    }

    private void OnItemClicked()
    {
        GameManager.Instance.GetModuleData(m_ContentIndex);
    }
}
