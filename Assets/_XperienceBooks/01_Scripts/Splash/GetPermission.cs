using System;
using System.Collections;
using System.Collections.Generic;
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

    bool isItPermissionTime = false;
    string nextPermission;
    Stack<string> permissions = new Stack<string>();

    void Start()
    {
        OpenAllPermissions();
    }

    public void OpenAllPermissions()
    {
        isItPermissionTime = true;
        CreatePermissionList();

    }
    void CreatePermissionList()
    {
        permissions = new Stack<string>();
#if UNITY_ANDROID
        permissions.Push(Permission.Camera);
        permissions.Push(Permission.ExternalStorageWrite);
        permissions.Push(Permission.FineLocation);
        permissions.Push("android.permission.POST_NOTIFICATIONS");
#elif UNITY_IOS
        permissions.Push(Permission.Camera);
#endif
        AskForPermissions();
    }
    void AskForPermissions()
    {
        if (permissions == null || permissions.Count <= 0)
        {
            isItPermissionTime = false;
            return;
        }
        nextPermission = permissions.Pop();

        if (nextPermission == null)
        {
            isItPermissionTime = false;
            return;
        }
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(nextPermission))
        {
            Permission.RequestUserPermission(nextPermission);
        }
#elif UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
#endif

        else
        {
            if (isItPermissionTime == true)
                AskForPermissions();
        }
        Debug.Log("Unity>> permission " + nextPermission + "  status ;" + Permission.HasUserAuthorizedPermission(nextPermission));
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("Unity>> focus ....  " + focus + "   isPermissionTime : " + isItPermissionTime);
        if (focus == true && isItPermissionTime == true)
        {
            AskForPermissions();
        }
    }

}
