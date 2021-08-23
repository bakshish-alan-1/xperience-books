using Intellify.core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{

    public static Profile Instance;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        }
    }

    [SerializeField]
    private TMP_Text m_UserId;

    [SerializeField]
    private TMP_InputField m_FirstName, m_LastName, m_Email, m_parentEmail, m_Age, m_password, m_confirmPassword;

    [SerializeField]
    private Toggle m_Gender_Male, m_Gender_Female, m_Gender_Other;

    [SerializeField]
    private ToggleGroup m_Gender_Group;

    [SerializeField]
    private TMP_Text m_errorText;

    [SerializeField]
    private TMP_Text m_LogoutButtonText;

    [SerializeField] GameObject ConfirmPasswordOBJ;


    // Start is called before the first frame update
    void Start()
    {
        m_errorText.text = "";
    }

    public void callSetProfileData()
    {
        SetProfileData(GameManager.Instance.m_UserData);
    }

    public void SetProfileData(UserData data) {

        ConfirmPasswordOBJ.SetActive(false);
        m_UserId.text = data.user.id.ToString();
        m_FirstName.text = data.user.firstname;
        m_LastName.text = data.user.lastname;
        m_Email.text = data.user.email;
        m_parentEmail.text = data.user.parent_email;
        m_Age.text = data.user.age;
        m_password.text = data.user.password;

        if (data.user.password == "")
            m_password.text = PlayerPrefs.GetString("Password");


        Debug.Log("Gender: " + data.user.gender);
        switch (int.Parse(data.user.gender))
        {
            case 1:
                m_Gender_Male.isOn = true;
                m_Gender_Female.isOn = false;
                m_Gender_Other.isOn = false;
                break;
            case 2:
                m_Gender_Male.isOn = false;
                m_Gender_Female.isOn = true;
                m_Gender_Other.isOn = false;
                break;
            case 3:
                m_Gender_Male.isOn = false;
                m_Gender_Female.isOn = false;
                m_Gender_Other.isOn = true;
                break;
        }

    }

    public void OnProfileChange()
    {
        m_LogoutButtonText.text = "UPDATE";
    }

    public void OnPasswordEdited()
    {
        ConfirmPasswordOBJ.SetActive(true);
    }

    public void OnResetButtonText(string text)
    {
        if (m_LogoutButtonText.text.ToLower() == "update" && text.ToLower() == "back")
        { SetProfileData(GameManager.Instance.m_UserData); }

        m_LogoutButtonText.text = "LOGOUT";
        m_errorText.text = "";
    }

    void resetErrorText()
    {
        m_errorText.text = "";
    }

    public void OnLogoutButtonHit()
    {
        if (m_LogoutButtonText.text.ToLower() == "update")
        {
            bool isUpdate = false;
            if (ConfirmPasswordOBJ.activeInHierarchy)
            {
                if (m_confirmPassword.text == "")
                {
                    CancelInvoke("resetErrorText");
                    Invoke("resetErrorText", 5f);
                    m_errorText.text = "Confirm password cannot be empty.";
                }
                else if (m_confirmPassword.text != m_password.text)
                {
                    CancelInvoke("resetErrorText");
                    Invoke("resetErrorText", 5f);
                    m_errorText.text = "Password miss match";
                }
                else
                    isUpdate = true;
            }
            else
                isUpdate = true;


            if (isUpdate == true)
            {
                // call update api here
                User user = new User();
                user.id = GameManager.Instance.m_UserData.user.id;
                user.firstname = m_FirstName.text;
                user.lastname = m_LastName.text;
                user.email = m_Email.text;
                user.parent_email = m_parentEmail.text;
                user.age = m_Age.text;
                user.password = m_password.text;
                user.new_password = m_confirmPassword.text;

                if (m_Gender_Male.isOn == true)
                {
                    user.gender = "1";
                }
                else if (m_Gender_Female.isOn == true)
                {
                    user.gender = "2";
                }
                else if(m_Gender_Other.isOn == true)
                {
                    user.gender = "3";
                }
                Debug.Log("updated gender are: " + user.gender);
                m_confirmPassword.text = "";
                // update user details on gamemenager user
                GameManager.Instance.m_UserData.user.firstname = m_FirstName.text;
                GameManager.Instance.m_UserData.user.lastname = m_LastName.text;
                GameManager.Instance.m_UserData.user.email = m_Email.text;
                GameManager.Instance.m_UserData.user.parent_email = m_parentEmail.text;
                GameManager.Instance.m_UserData.user.age = m_Age.text;
                GameManager.Instance.m_UserData.user.gender = user.gender;
                GameManager.Instance.m_UserData.user.password = m_password.text;
                PlayerPrefs.SetString("Password", m_password.text);
                ApiManager.Instance.UpdateUserProfile(user);
            }
        }
        else
        {
            PlayerPrefs.SetInt("IsAgreed", 0);
            WindowManager.Instance.LogOut();
        }
    }
}
