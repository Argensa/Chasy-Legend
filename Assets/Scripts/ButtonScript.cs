using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonScript : MonoBehaviour
{
    public GameObject gameController;
    int carID;
    GameObject[] carNum;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        carNum = gameController.GetComponent<SceneController>().cars;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    public void ChangeRight ()
    {

        gameController.GetComponent<SceneController>().currentCar.SetActive(false);
        gameController.GetComponent<SceneController>().carID++;
        if (gameController.GetComponent<SceneController>().carID > carNum.Length - 1)
        {
            gameController.GetComponent<SceneController>().carID = 0;
        }
    }
    public void ChangeLeft ()
    {
        gameController.GetComponent<SceneController>().currentCar.SetActive(false);
        gameController.GetComponent<SceneController>().carID--;
        if (gameController.GetComponent<SceneController>().carID < 0)
        {
            gameController.GetComponent<SceneController>().carID = carNum.Length - 1;
        }
    }
    public void ChangeGameState()
    {
        gameController.GetComponent<SceneController>().gameState = 1;
    }
}
