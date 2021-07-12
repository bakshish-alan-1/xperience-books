using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class Forgot
{
    public string otp;
    public string email;
    public string password;
}

public class ForgotPassword : MonoBehaviour
{
    public static ForgotPassword Instance = null;

    [SerializeField] private TMP_InputField forgotEmail, otp, newPassword, confirmPassword;
    [SerializeField] private TMP_Text errorText;

    [SerializeField] private GameObject GetOTPData, ForgotPasswardData;
    string userEmail = "";

    void Start()
    {
        if (Instance == null)
            Instance = this;

        GetOTPData.SetActive(true);
        ForgotPasswardData.SetActive(false);
    }

    public void OnForgotPasswordBtnHit()
    {
        try
        {
            if (ValidateInput())
            {
                userEmail = forgotEmail.text;
                ApiManager.Instance.GetForgotPasswordOTP(forgotEmail.text);
            }

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public void OnBackBtn()
    {
        if (GetOTPData.activeSelf == true)
        {
            ResetField();
            WindowManager.Instance.OpenPanel("LoginForm");
        }
        else
        {
            GetOTPData.SetActive(true);
            ForgotPasswardData.SetActive(false);
            ResetError(newPassword);
            ResetError(confirmPassword);
            ResetError(otp);
        }
    }

    public void GetOTPSuccess(string msg)
    {
        callTostMessage(msg);
        GetOTPData.SetActive(false);
        ForgotPasswardData.SetActive(true);
    }

    public void ResetField()
    {
        forgotEmail.text = "";
        GetOTPData.SetActive(true);
        ForgotPasswardData.SetActive(false);
        ResetError(forgotEmail);
    }

    void callTostMessage(string msg)
    {
        CancelInvoke("resetErrorText");
        Invoke("resetErrorText", 5f);
        errorText.text = msg;
    }

    public void OnForgotPassword()
    {
        if (newPassword.text == "" || confirmPassword.text == "" || otp.text == "")
        {
            callTostMessage("Password not empty");
        }
        else if (confirmPassword.text != newPassword.text)
        {
            callTostMessage("Password miss match");
        }
        else
        {
            Forgot password = new Forgot();
            password.otp = otp.text;
            password.email = userEmail;
            password.password = confirmPassword.text;

            PlayerPrefs.SetString("Password", password.password);
            ResetError(otp);
            ResetError(confirmPassword);
            ResetError(newPassword);
            ApiManager.Instance.ForgotUserPassword(password);
        }
    }

    public void ValidateEmail()
    {

        if (Validator.validateEmail(forgotEmail.text))
        {

#if UNITY_EDITOR
            Debug.Log("<color=green>Email ID in Correct format</green>");
#endif
        }
        else
        {

            forgotEmail.text = "";
            forgotEmail.placeholder.GetComponent<Text>().color = Color.red;
            forgotEmail.placeholder.GetComponent<Text>().text = "Email-id not valid !";
        }

    }

    public bool ValidateInput()
    {
        bool confirm = true;


        if (string.IsNullOrEmpty(forgotEmail.text))
        {
            SetError(forgotEmail);

            return false;
        }

#if UNITY_EDITOR
        Debug.Log("<color=green>Valid Input</green>");
#endif

        return confirm;
    }

    public void SetError(TMP_InputField field)
    {

        field.text = "";
        field.placeholder.GetComponent<TMP_Text>().color = Color.red;
        field.placeholder.GetComponent<TMP_Text>().text = "Cannot be empty.";

    }

    public void ResetError(TMP_InputField field)
    {

        field.text = "";
        field.placeholder.GetComponent<TMP_Text>().color = Color.grey;
        field.placeholder.GetComponent<TMP_Text>().text = "Enter text...";

    }
}
