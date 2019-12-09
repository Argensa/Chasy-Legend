using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject gameController;
    public bool pausing;
    // Start is called before the first frame update
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }
    public void PauseGame()
    {

        if (gameController.GetComponent<SceneController>().pause == false)
        {
            
            pausing = true;

        }
        if (gameController.GetComponent<SceneController>().pause == true)
        {
         
            pausing = false;
        }
    }

}
