﻿using System;
using System.Collections;
using System.Text;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace Intellify.core
{


    #region Login || Register Response

    [System.Serializable]
    public class UserData
    {
        public string token;
        public User user;
    }

    [System.Serializable]
    public struct LoginResponse
    {
        public bool success;
        public int code;
        public UserData data;
        public string message;
    }

    [System.Serializable]
    public struct UpdateUserProfileResponse
    {
        public bool success;
        public int code;
        public User data;
        public string message;
    }
    #endregion

    #region ForceUpdate
    [Serializable]
    public class ForceUpdate
    {
        public string message;
        public bool success;
        public int code;
    }
    #endregion

    public class APIClient : BaseClient
    {
        // This will return API call Response code and data 
        public delegate void action(bool success, object data,long statusCode);

        private static bool IsValid(long code) => (int)code / 100 == 2;

        private bool HasAccessInternetConnection = false;

        //Check Connection
        #region Connection check
         private void CheckConnection(Action<bool> action)
         {
             UnityWebRequest request = new UnityWebRequest("https://google.com", "GET");

             request.SetRequestHeader("Accept", "application/json");
             request.SetRequestHeader("Accept-Language", "en_US");

             request.downloadHandler = new DownloadHandlerBuffer();

             request.SendWebRequest().completed += _ =>
             {
                 if (request.result == UnityWebRequest.Result.ConnectionError)
                 {
                     ApiManager.Instance.RaycastUnblock();
                     action(false);
                 }
                 else
                 {
                     action(true);
                 }
             };
         }
        
        #endregion


        #region API_CALL
        /// Make a raw API request to PayPal.
        /// </summary>
        /// <param name="method">HTTP method Ex. GET OR POST </param>
        /// <param name="endPoint">i.e. login , register </param>
        /// <param name="jsonData">Data to post/patch.</param>
        /// <param name="AccessToken">Data to post/patch.</param>
        /// <param name="callback">Returns server response in JSON format (status code, response string).</param>
        public void Request(string method, string endPoint, string jsonData, string accessToken , UnityAction<long, string> callback)
        {
            //Debug.Log(PayPalEnvironment.BaseUrl() + endPoint);
            Debug.Log("Api Called : " + APIEnvironment.BaseUrl() + endPoint);

                UnityWebRequest request = new UnityWebRequest(APIEnvironment.BaseUrl() + endPoint, method);

            request.SetRequestHeader("min-app-version", properties.AppVersion);

            if (!string.IsNullOrEmpty(accessToken))
                    request.SetRequestHeader("Authorization", accessToken);

                request.SetRequestHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(jsonData)) {
                    Debug.Log("*********************************************");
                    Debug.Log(jsonData);
                    request.uploadHandler = new UploadHandlerRaw(Encoding.ASCII.GetBytes(jsonData)) { contentType = "application/json" };
            }

                request.downloadHandler = new DownloadHandlerBuffer();

                request.SendWebRequest().completed += _ =>
                {
                    callback(request.responseCode, request.downloadHandler.text);
                };
        }
        #endregion


        public static void UploadPrintfulImage(string orderId)
        {
            new APIClient().printfulImage(orderId);
        }

        public void printfulImage(string orderId)
        {
            string url = APIEnvironment.BaseUrl() + properties.printfulImageUpload;
            Debug.Log("printfulImage URL: " + url);
            Debug.Log("OrderID: " + orderId);

            WWWForm formrequest = new WWWForm();

            Debug.Log("UploadMarkerImage byte length: " + GameManager.Instance.printfulImage.Length.ToString());
            formrequest.AddBinaryData("printful_image", GameManager.Instance.printfulImage, "printful.jpg");
            formrequest.AddField("system_order_id", orderId);

            UnityWebRequest request = UnityWebRequest.Post(url, formrequest);
            {
                request.SetRequestHeader("Authorization", GameManager.Instance.m_UserData.token);
                request.SetRequestHeader("Accept", "application/json");

                request.SetRequestHeader("min-app-version", properties.AppVersion);

                request.SendWebRequest();

                Debug.Log("printfulImage upload code: " + request.responseCode);

                if (request.result != UnityWebRequest.Result.Success || request.responseCode == 422 || request.responseCode == 500)
                {
                    Debug.Log("Upload printful IMG Error: " + request.error);
                }
                else
                {
                    Debug.Log("printful uploade success: " + request.downloadHandler.text);
                }
            }
        }

        #region WebAPI call
        public static void CallWebAPI(string method, string endPoint, string jsonData, string accessToken, action onComplete = null)
        {

            new APIClient().CreateAPIResponse(method, endPoint, jsonData, accessToken, onComplete);
        }

        private void CreateAPIResponse(string method, string endPoint, string jsonData, string accessToken, action onComplete)
        {
            
            Request(method, endPoint, jsonData, accessToken, (status, res) =>
            {
                Debug.Log("Full Response : " + res);
               
                //                Debug.Log("success :" + _node["success"] + " , code :" + _node["code"]);

                /*   "code" : 200 ":  "Sucess "
                 *   "code": 401 : "Runtime message " when data not available or missmatch data found
                 *   "code": 403 : "message": "unauthenticated"
                 *   "code" : 500 : Internal server Error 
                 */

                if (!IsValid(status))
                {
                    onComplete?.Invoke(false, res, status);
                    return;
                }
                else {
                    JSONNode _node = JSON.Parse(res);
                    Debug.Log("Code: " + _node["code"]);
                    if (_node["code"].AsInt == 901 || _node["code"].AsInt == 902)
                    {
                        Debug.Log("Force Update required");
                        ForceUpdate data = JsonUtility.FromJson<ForceUpdate>(res);
                        ForceUpdateData.Instance.onSetdata(data.message);
                    }
                    else if (_node["code"].AsInt != 200)
                    {
                        onComplete?.Invoke(false, res, status);
                        return;
                    }
                    else {

                        onComplete?.Invoke(true, res, status);
                    }
                }
            });
        }
        #endregion

    }
}
