using System.IO;
using KetosGames.SceneTransition;
using UnityEngine;
using UnityEngine.UI;

public class LoadMainScene : MonoBehaviour
{
    public GameObject envirnmentSelector;

    public Dropdown env;
    public void LoadNextScene()
    {
        PlayerPrefs.SetInt("IsFreshInstall", 1);// if value is 0 then localstorage folder delete or created from gamemanager
        Debug.Log("Load home scene");

        if (Directory.Exists(GameManager.Instance.GetThemePath()))
        {
            Debug.Log("Call LoadSkinTheme from LoadMainScene");
            ThemeManager.Instance.LoadSkinTheme();
        }
        else
            SceneLoader.LoadScene(1);
    }
}
