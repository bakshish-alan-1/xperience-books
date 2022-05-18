//#if UNITY_EDITOR
using Firebase.Extensions;
using Firebase.Messaging;
//#endif
using UnityEngine;

public class FirebaseNotificaitonManager : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("firebaseTokenSaved", 0);
        InitializeFirebase();
    }

    public void OnDeInitializeToken()
    {
        Debug.Log("OnDeInitializeToken");
        FirebaseMessaging.DeleteTokenAsync();
    }

    void InitializeFirebase()
    {
        Debug.Log("inside InitializeFirebase");

        FirebaseMessaging.SubscribeAsync("Xperience Books").ContinueWithOnMainThread(task => {
            //LogTaskCompletion(task, "SubscribeAsync");
        });
        Debug.Log("Firebase Messaging Initialized");

        FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
          task => {
              //LogTaskCompletion(task, "RequestPermissionAsync");
          }
        );
    }

    private void OnEnable()
    {
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    private void OnDestroy()
    {
        FirebaseMessaging.MessageReceived -= OnMessageReceived;
        FirebaseMessaging.TokenReceived -= OnTokenReceived;
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        Debug.Log("Firebase message: " + e.Message.From);
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        Debug.Log("Firebase token: " + e.Token);
        GameManager.Instance.FirebaseToken = e.Token;
        if (PlayerPrefs.GetInt(StaticKeywords.Login) == 1)
        {
#if !UNITY_EDITOR
        ApiManager.Instance.SetNotificationToken(e.Token);
#endif
        }
    }
}
