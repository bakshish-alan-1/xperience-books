using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PasswordChange
{
    public string old_password;
    public string new_password;
}

public class ChangePassword : MonoBehaviour
{
    [SerializeField] TMP_InputField nPassword;
    [SerializeField] TMP_InputField confirm_password;
    [SerializeField] TMP_Text m_errorText;

    void resetErrorText()
    {
        m_errorText.text = "";
    }

    public void OnUpdatePassword()
    {
        string pass = PlayerPrefs.GetString("Password");
        
        if ((nPassword.text == pass || confirm_password.text == pass))
        {
            CancelInvoke("resetErrorText");
            Invoke("resetErrorText", 5f);
            m_errorText.text = "Enter new password";
        }
        else if (nPassword.text == "" || confirm_password.text == "")
        {
            CancelInvoke("resetErrorText");
            Invoke("resetErrorText", 5f);
            m_errorText.text = "Password not empty";
        }
        else if (confirm_password.text != nPassword.text)
        {
            CancelInvoke("resetErrorText");
            Invoke("resetErrorText", 5f);
            m_errorText.text = "Password miss match";
        }
        else
        {
            PasswordChange password = new PasswordChange();
            password.old_password = pass;
            password.new_password = confirm_password.text;

            nPassword.text = "";
            confirm_password.text = "";

            PlayerPrefs.SetString("Password", password.new_password);
            ApiManager.Instance.UpdatePassword(password);
        }
    }
}
