using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AchievementText : MonoBehaviour
{
    public TextMeshProUGUI time_TMP;
    public TextMeshProUGUI kill_TMP;
    public TextMeshProUGUI money_TMP;

    float surviveTime;
    private void Start()
    {
        kill_TMP.text = "Most car destroyed:  " +  PlayerPrefs.GetInt("KillCount", 0).ToString();
        money_TMP.text = "Most money collected:  " + PlayerPrefs.GetInt("Highscore", 0).ToString();
        surviveTime = PlayerPrefs.GetInt("SurviveTime", 0);
        
        string minutes = Mathf.Floor(surviveTime / 60).ToString("00");
        string seconds = (surviveTime % 60).ToString("00");
        time_TMP.text = "Longest time survived:  " + string.Format("{0}:{1}", minutes, seconds);
    }
}
