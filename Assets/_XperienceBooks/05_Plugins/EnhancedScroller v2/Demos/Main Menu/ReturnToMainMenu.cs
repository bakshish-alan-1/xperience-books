using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace EnhancedScrollerDemos.MainMenu
{
    public class ReturnToMainMenu : MonoBehaviour
    {
        public void ReturnToMainMenuButton_OnClick()
        {
            Debug.Log("===================================== All Download compete goto next scene");
            SceneManager.LoadScene("MainMenu");
        }
    }
}