using System;
using Intellify.core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public static Login Instance = null;

    [SerializeField] private GameObject LoginData, ParenErrorData;

    [SerializeField] private TMP_InputField email;

    [SerializeField] private TMP_InputField forgotEmail;

    [SerializeField] private TMP_InputField password;

    [SerializeField] private TMP_Text parentEmailErrorText;
    [SerializeField] GameObject parentEmailObject;

    [SerializeField] Color errorColor, defaultColor;

    //Parent Error fields
    [SerializeField] TMP_Text user_ID;
    [SerializeField ] TMP_InputField parentEmail;

    UserData d = new UserData();

    [SerializeField] 

    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public void LoginUser() {
        try
        {
            if (ValidateInput())
            {
                //Create model for User
                User user = new User();
                user.email = email.text;
                user.password = password.text;
                PlayerPrefs.SetString("Password", password.text);
                //Call Login API
                ApiManager.Instance.NewLogin(user);
                ResetField();
            }

        }
        catch (Exception ex) {
            Debug.LogError(ex.Message);
        }

    }

    public void OnForgotPasswordTextHit()
    {
        forgotEmail.text = email.text;
        WindowManager.Instance.OpenPanel("ForgotPassword");
    }

    public bool ValidateInput() {
        bool confirm = true;


        if (string.IsNullOrEmpty(email.text))
        {
            SetError(email);

            return false;
        }

        if (string.IsNullOrEmpty(password.text))
        {
            SetError(password);

            return false;
        }

#if UNITY_EDITOR
        Debug.Log("<color=green>Valid Input</green>");
#endif

        return confirm;   
    }

    public void ValidateEmail(string name)
    {
        TMP_InputField emailID;
        if (name == "user")
            emailID = email;
        else
            emailID = parentEmail;

        if (Validator.validateEmail(emailID.text))
        {

#if UNITY_EDITOR
            Debug.Log("<color=green>Email ID in Correct format</green>");
            #endif
        }
        else
        {

            emailID.text = "";
            emailID.placeholder.GetComponent<Text>().color = Color.red;
            emailID.placeholder.GetComponent<Text>().text = "Email-id not valid !";
        }
        
    }

    public void OnBackBtn()
    {
        if (LoginData.activeSelf == true)
        {
            ResetField();
            WindowManager.Instance.OpenPanel("SignUp");
        }
        else
        {
            LoginData.SetActive(true);
            ParenErrorData.SetActive(false);
            ResetError(parentEmail);
        }
    }

    public void OnParentEmailVarifiedFail(string msg, User data)
    {
        parentEmailObject.SetActive(true);
        //parentEmailErrorText.text = msg;
        user_ID.text = data.id.ToString();
        parentEmail.text = data.parent_email;
        d.user = data;
        Debug.Log("id: " + d.user.id + ", dataID: " + data.id);
    }

    public void ParentResendTextHit()
    {
        LoginData.SetActive(false);
        ParenErrorData.SetActive(true);
    }

    public void ParentResendEmail()
    {
        d.user.parent_email = parentEmail.text;
        parentEmailObject.SetActive(false);
        ApiManager.Instance.UpdateParentEmail(d.user.id, parentEmail.text);
    }

    public void parentEmailSendSuccess()
    {
        LoginData.SetActive(true);
        ParenErrorData.SetActive(false);
    }

    public void ResetField()
    {
        ResetError(email,"abc@xyz.com");
        ResetError(password);
    }


    public void SetError(TMP_InputField field)
    {

        field.text = "";
        field.placeholder.GetComponent<TMP_Text>().color = errorColor;
        field.placeholder.GetComponent<TMP_Text>().text = "Cannot be empty.";

    }


    public void ResetError(TMP_InputField field, string msg=null)
    {

        field.text = "";
        field.placeholder.GetComponent<TMP_Text>().color = defaultColor;
        if (msg != null)
        {
            field.placeholder.GetComponent<TMP_Text>().text = msg;
        }
        else
        {
            field.placeholder.GetComponent<TMP_Text>().text = "Enter text...";
        }
    }
}
