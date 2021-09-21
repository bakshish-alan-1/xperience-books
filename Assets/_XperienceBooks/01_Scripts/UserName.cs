using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserName : MonoBehaviour
{

    public TextMeshProUGUI userName;

    // Start is called before the first frame update
    void Start()
    {
        if (userName) {
            userName.text = "Hello "+GameManager.Instance.m_UserData.user.firstname;
        }
    }

    
}
