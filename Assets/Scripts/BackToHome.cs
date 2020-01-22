using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHome : MonoBehaviour
{
 
    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        
    }
}
