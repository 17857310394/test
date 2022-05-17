using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    public Text LevelTime;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public static void UpdateTimeText(float Time)
    {
        int minute = (int)Time / 60;
        int second = (int)Time % 60;
        instance.LevelTime.text = minute.ToString("00") + ":" + second.ToString("00");
    }
}
