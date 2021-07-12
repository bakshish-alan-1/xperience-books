using UnityEngine;
using UnityEngine.UI;

public class PrivacyPolicies : MonoBehaviour
{
    [SerializeField] Toggle agreeBtn;
    [SerializeField] Scrollbar scrollbar;
    [SerializeField] Button goBtn;

    private void Start()
    {
        scrollbar.value = 1f;
    }

    public void GoBtn()
    {
        if (agreeBtn.isOn)
        {
            PlayerPrefs.SetInt("IsAgreed", 1);
            Debug.Log("1 IsAgreed: " + PlayerPrefs.GetInt("IsAgreed"));
            WindowManager.Instance.OpenPanel("RegesterForm");
        }
    }

    public void AgreeToggleChange()
    {
        if (agreeBtn.isOn)
            goBtn.interactable = true;
        else
            goBtn.interactable = false;
    }
}
