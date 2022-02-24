using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace KetosGames.SceneTransition.Example
{
    public class GoScript : MonoBehaviour
    {
        public string ToScene;

        public void GoToNextScene()
        {
            Debug.Log("GoToNextScene: " + ToScene);

            try
            {
                LoaderUtility.Deinitialize();   // deinitialize arsession subsystem (https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/api/UnityEngine.XR.ARFoundation.LoaderUtility.html)
            }
            catch(Exception e)
            {
                Debug.Log("GoToNextScene catch: " + e.ToString());
            }
            Invoke("callScene", 1f);
        }

        void callScene()
        {
            //SceneManager.LoadScene(ToScene);
            SceneLoader.LoadScene(ToScene);
        }
    }
}
