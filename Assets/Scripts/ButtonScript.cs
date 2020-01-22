using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour
{
    public GameObject gameController;
    int carID;
    GameObject[] carNum;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        if (gameController != null)
        {
            carNum = gameController.GetComponent<SceneController>().cars;
        }
       
    }

 
    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("Blip");
        SceneManager.LoadScene(1);
    }
    public void Home()
    {
        FindObjectOfType<AudioManager>().Play("Blip");
        SceneManager.LoadScene(0);
    }
    public void BuyCars()
    {
        PlayerPrefs.SetInt("Unlocked", 1);
    }
    public void ShoppingMenu()
    {
        SceneManager.LoadScene(2);
    }
    public void ChangeRight ()
    {
        
        FindObjectOfType<AudioManager>().Play("Blip");
        gameController.GetComponent<SceneController>().currentCar.SetActive(false);
        gameController.GetComponent<SceneController>().carID++;
        

        if (gameController.GetComponent<SceneController>().carID > carNum.Length - 1)
        {
            gameController.GetComponent<SceneController>().carID = 0;
           
            
        }
        PlayerPrefs.SetInt("PreferredCar", gameController.GetComponent<SceneController>().carID);
       

    }
    public void ChangeLeft ()
    {
        
        FindObjectOfType<AudioManager>().Play("Blip");
        gameController.GetComponent<SceneController>().currentCar.SetActive(false);
        gameController.GetComponent<SceneController>().carID--;


        if (gameController.GetComponent<SceneController>().carID < 0)
        {
            gameController.GetComponent<SceneController>().carID = carNum.Length - 1;
            
            
        }
        PlayerPrefs.SetInt("PreferredCar", gameController.GetComponent<SceneController>().carID);
       

    }
    public void ChangeGameState()
    {
        FindObjectOfType<AudioManager>().Play("Blip");
        
        if (gameController.GetComponent<SceneController>().carsUnlockedOrNot[carID] == true)
        {
           
            gameController.GetComponent<SceneController>().gameState = 1;
        }
        else if (gameController.GetComponent<SceneController>().carsUnlockedOrNot[carID] == false)
        {
            
        }
    }
    public void Mute_SFX()
    {
        FindObjectOfType<AudioManager>().MuteSFX();
    }
    public void Mute_Music()
    {
        FindObjectOfType<AudioManager>().MuteMusic();
    }

}
