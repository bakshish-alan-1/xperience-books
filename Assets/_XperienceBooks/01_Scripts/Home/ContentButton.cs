using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ContentButton : MonoBehaviour
{

    [SerializeField] RawImage m_Sprite;
    public int m_ContentIndex;


    public async Task SetDataAsync(int moduleNo, Texture2D img, int index) {

        //Debug.Log("data from call : " + moduleNo);

        string name = "module_" + (moduleNo + 1) + ".png";

        Texture2D thisTexture = new Texture2D(100, 100);

        if (File.Exists(GameManager.Instance.GetThemePath() + "/" + name))
        {
            byte[] bytes = File.ReadAllBytes(GameManager.Instance.GetThemePath() + "/" + name);

            while (!thisTexture.LoadImage(bytes))
                await Task.Yield();

            m_Sprite.texture = thisTexture;
        }
        else
            m_Sprite.texture = img;

        m_ContentIndex = index;
    }

    public void OnItemClicked()
    {
        GameManager.Instance.GetModuleData(m_ContentIndex);
    }
}
