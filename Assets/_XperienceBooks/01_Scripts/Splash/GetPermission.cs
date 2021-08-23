using TBEasyWebCam;
using UnityEngine;
using UnityEngine.Android;

public class GetPermission : MonoBehaviour
{
    void Start()
    {

#if !UNITY_EDITOR && UNITY_ANDROID
        getCameraPermission();
        getLocationPermission();
        getStoragePermission();
#endif

    }

    void getLocationPermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }

    void getStoragePermission()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }

    void getCameraPermission()
    {
        CameraPermissionsController.RequestPermission(new[] { EasyWebCam.CAMERA_PERMISSION }, new AndroidPermissionCallback(
            grantedPermission =>
            {
                Debug.Log("Camera Permission Granted");
                // The permission was successfully granted, restart the change avatar routine
            },
            deniedPermission =>
            {
                // The permission was denied
                Debug.Log("Camera Permission denied");
            },
            deniedPermissionAndDontAskAgain =>
            {
                // The permission was denied, and the user has selected "Don't ask again"
                // Show in-game pop-up message stating that the user can change permissions in Android Application Settings
                // if he changes his mind (also required by Google Featuring program)
            }));
    }
}
