using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public bool scanComplete = false;

    

    void Awake()
    {
        ResetPlayerPref();


        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
        


    public void ClearScanData() {
        scanComplete = false;
    }

    public void ResetPlayerPref() {

        PlayerPrefs.SetInt("isTutorialOver", 0);
    }

}
