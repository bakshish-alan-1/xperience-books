using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace KetosGames.SceneTransition.Example
{
    public class GoScript : MonoBehaviour
    {
        public string ToScene;

        public void GoToNextScene()
        {
            Debug.Log("GoToNextScene: " + ToScene);
            LoaderUtility.Deinitialize();   // deinitialize arsession subsystem (https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@4.1/api/UnityEngine.XR.ARFoundation.LoaderUtility.html)
            SceneLoader.LoadScene(ToScene);
        }
    }
}
