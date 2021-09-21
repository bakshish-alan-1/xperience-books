using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Intellify.core;
using TMPro;


public class Register : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField firstname;
    [SerializeField]
    private TMP_InputField lastname;
    [SerializeField]
    private TMP_InputField email, parentEmail;
    [SerializeField] Image parentEmailRoot;
    [SerializeField]
    private TMP_InputField age;
    [SerializeField]
    private Toggle male, female, other;
    [SerializeField]
    private ToggleGroup gender;
    [SerializeField]
    private GameObject errorText;
    [SerializeField] private int AgeLimit;

    [SerializeField]
    Color errorColor,defaultColor;

    public void setUserData(UserData userData)
    {
        firstname.text = userData.user.firstname;
        lastname.text = userData.user.lastname;
        email.text = userData.user.email;
        parentEmail.text = userData.user.parent_email;
        age.text = userData.user.age;

        switch (int.Parse(userData.user.gender))
        {
            case 1:
                male.isOn = true;
                female.isOn = false;
                other.isOn = false;
                break;
            case 2:
                male.isOn = false;
                female.isOn = true;
                other.isOn = false;
                break;
            case 3:
                male.isOn = false;
                female.isOn = false;
                other.isOn = true;
                break;
        }
    }

    public void RegisterUser()
    {
        try
        {
            if (ValidateInput()) {
                User user = new User();
                user.firstname = firstname.text;
                user.lastname = lastname.text;
                user.email = email.text;
                user.parent_email = parentEmail.text;
                user.age = age.text;
                user.is_agreed = true;
                Toggle selected = gender.ActiveToggles().FirstOrDefault();
                user.gender = getGenderNumber(selected.name).ToString();
                ApiManager.Instance.NewRegister(user);
                ResetField();
            }
        }
        catch (Exception ex) {

            Debug.LogError("Register : "+ex.Message);
        }
    }

    private int getGenderNumber(string SelectedGender)
    {
        int index = 1;
        string name = SelectedGender.ToLower();
        switch(name)
        {
            case "m":
                index = 1;
                break;
            case "f":
                index = 2;
                break;
            case "other":
                index = 3;
                break;
        }

        return index;
    }

    public bool ValidateInput()
    {
        bool confirm = true;


        if (string.IsNullOrEmpty(firstname.text))
        {
            SetError(firstname);

            return false;
        }

        if (string.IsNullOrEmpty(lastname.text))
        {
            SetError(lastname);

            return false;
        }

        if (string.IsNullOrEmpty(email.text))
        {
            SetError(email);

            return false;
        }

        if (parentEmail.interactable == true)
        {
            if (string.IsNullOrEmpty(parentEmail.text))
            {
                SetError(parentEmail);

                return false;
            }
        }

        if (string.IsNullOrEmpty(age.text))
        {
            SetError(age);
            return false;
        }

        if (!gender.AnyTogglesOn()) {
            errorText.SetActive(true);
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

        Debug.Log("Function Called"+ emailID.text);
        if (Validator.validateEmail(emailID.text))
        {

            Debug.Log("Done");
        }
        else
        {
            emailID.text = "";
            emailID.placeholder.GetComponent<Text>().color = errorColor;
            emailID.placeholder.GetComponent<Text>().text = "Email-id not valid.";
        }

    }

    public void CheckAgeValidation()
    {
        if (int.Parse(age.text) >= AgeLimit)
        {
            parentEmail.text = "";
            parentEmail.interactable = false;
            Color32 color = new Color32(200, 200, 200, 255);
            parentEmailRoot.color = color;
        }
        else
        {
            parentEmail.text = "";
            parentEmail.interactable = true;
            parentEmailRoot.color = Color.white;
        }
        ResetError(parentEmail, "Parent Email");
    }

    public void ResetField() {

        ResetError(firstname, "First Name");
        ResetError(lastname, "Last Name");
        ResetError(email, "Email");
        ResetError(parentEmail, "Parent Email");
        ResetError(age, "Age");
        errorText.SetActive(false);
        gender.SetAllTogglesOff();
        male.isOn = true;
        parentEmailRoot.color = Color.white;
    }


    public void SetError(TMP_InputField field) {

        field.text = "";
        field.placeholder.GetComponent<TMP_Text>().color = errorColor;
        field.placeholder.GetComponent<TMP_Text>().text = "Cannot be empty.";

    }

    public void SetSameFieldError(TMP_InputField field)
    {

        field.text = "";
        field.placeholder.GetComponent<TMP_Text>().color = errorColor;
        field.placeholder.GetComponent<TMP_Text>().text = "Cannot be same.";

    }


    public void ResetError(TMP_InputField field, string placeHolder) {

        field.text = "";
        field.placeholder.GetComponent<TMP_Text>().color = defaultColor;
        field.placeholder.GetComponent<TMP_Text>().text = placeHolder;

    }
}
