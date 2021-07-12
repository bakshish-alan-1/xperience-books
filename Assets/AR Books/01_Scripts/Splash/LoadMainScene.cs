using UnityEngine;
using UnityEngine.UI;

public class LoadMainScene : MonoBehaviour
{
    public GameObject envirnmentSelector;

    public Dropdown env;
    public void LoadNextScene()
    {
        // remove comment for internal test with api selection drop down
        /* 
        if (PlayerPrefs.GetInt("IsFreshInstall") == 0)
        {
            envirnmentSelector.SetActive(true);
        }
        else {
           
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
        */

        PlayerPrefs.SetInt("ENV", 1);// 1 = testing , 2 = production
        PlayerPrefs.SetInt("IsFreshInstall", 1);
        Debug.Log("Load home scene");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


    public void DropBpoxUpdated() {

        if (env.value != 0) {
            PlayerPrefs.SetInt("IsFreshInstall", 1);
            PlayerPrefs.SetInt("ENV", env.value);
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);

        }
    }
}
