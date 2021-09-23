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
        SceneLoader.LoadScene(1);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }
}
