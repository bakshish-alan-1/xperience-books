using TBEasyWebCam;
using UnityEngine;
using UnityEngine.Android;

public class GetPermission : MonoBehaviour
{
    static public GetPermission Instance = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
#if !UNITY_EDITOR
        getCameraPermission();
        getLocationPermission();
        getStoragePermission();
#endif
    }

    void getLocationPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
#endif
    }

    void getStoragePermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif
    }

    void getCameraPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#elif UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
#endif
    }

    public bool IsCameraPermissionGranted()
    {
        bool value = false;
#if UNITY_ANDROID
        value = Permission.HasUserAuthorizedPermission(Permission.Camera);
#elif UNITY_IOS
        value = Application.HasUserAuthorization(UserAuthorization.WebCam);
#endif

        return value;
    }
}
