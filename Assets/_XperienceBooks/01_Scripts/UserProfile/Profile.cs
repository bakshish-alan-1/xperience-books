using System.IO;
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
    [SerializeField] Image m_Male_checkmark, m_Female_checkmark, m_Other_checkmark;

    [SerializeField]
    private ToggleGroup m_Gender_Group;

    [SerializeField]
    private TMP_Text m_errorText;

    [SerializeField]
    private TMP_Text m_LogoutButtonText;

    [SerializeField] GameObject ConfirmPasswordOBJ;

    bool isThemeSet = false;
    [Header("Theme")]
    [SerializeField] Image BGImage;
    [SerializeField] Image UpdateImg;
    [SerializeField] Image BackIcon;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text ID_Lbl;
    [SerializeField] TMP_Text FName_Lbl;
    [SerializeField] TMP_Text LName_Lbl;
    [SerializeField] TMP_Text Email_Lbl;
    [SerializeField] TMP_Text ParentEmail_Lbl;
    [SerializeField] TMP_Text Age_Lbl;
    [SerializeField] TMP_Text Gender_Lbl;
    [SerializeField] Text Male_Lbl;
    [SerializeField] Text Female_Lbl;
    [SerializeField] Text Other_Lbl;
    [SerializeField] TMP_Text Password_Lbl;
    [SerializeField] TMP_Text CPassword_Lbl;


    // Start is called before the first frame update
    void Start()
    {
        m_errorText.text = "";
    }

    public void OnSetThem()
    {
        if (!isThemeSet || GameManager.Instance.isNewThemeDownload)
        {
            isThemeSet = true;

            if (!File.Exists(GameManager.Instance.GetThemePath() + "/" + StaticKeywords.BGTheme))
            {
                m_Male_checkmark.color = new Color32(95, 93, 169, 255);
                m_Female_checkmark.color = new Color32(95, 93, 169, 255);
                m_Other_checkmark.color = new Color32(95, 93, 169, 255);
            }
            else
            {
                m_Male_checkmark.color = new Color32(255, 255, 255, 255);
                m_Female_checkmark.color = new Color32(255, 255, 255, 255);
                m_Other_checkmark.color = new Color32(255, 255, 255, 255);
            }
            BGImage.sprite = (ThemeManager.Instance.background);
            BackIcon.sprite = (ThemeManager.Instance.backBtn);
            UpdateImg.sprite = (ThemeManager.Instance.commonBtn);

            if (GameManager.Instance.TitleFont != null)
            {
                title.font = GameManager.Instance.TitleFont;
                m_LogoutButtonText.font = GameManager.Instance.TitleFont;
            }
            Color newCol;
            if (ColorUtility.TryParseHtmlString(GameManager.Instance.selectedSeries.theme.color_code, out newCol))
            {
                title.color = newCol;
                m_UserId.color = newCol;
                ID_Lbl.color = newCol;
                FName_Lbl.color = newCol;
                LName_Lbl.color = newCol;
                Email_Lbl.color = newCol;
                ParentEmail_Lbl.color = newCol;
                Age_Lbl.color = newCol;
                Gender_Lbl.color = newCol;
                Male_Lbl.color = newCol;
                Female_Lbl.color = newCol;
                Other_Lbl.color = newCol;
                Password_Lbl.color = newCol;
                CPassword_Lbl.color = newCol;
                m_LogoutButtonText.color = newCol;
            }
        }
    }

    public void callSetProfileData()
    {
        SetProfileData(GameManager.Instance.m_UserData);
    }

    public void SetProfileData(UserData data) {
        //OnSetThem();

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
                user.old_password = PlayerPrefs.GetString("Password");
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
