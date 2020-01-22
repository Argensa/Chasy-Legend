using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    
    public GameObject mapBuilder;
    public GameObject playerHolder;

    public GameObject gameController;
    int gameState;
    Transform tile;
    bool doAlready = false;
    // Start is called before the first frame update
    void Start()
    {
        mapBuilder = GameObject.FindGameObjectWithTag("MapBuilder");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");

    }

    // Update is called once per frame
   void Update()
    {
            if (gameState == 0)
            {
                gameState = gameController.GetComponent<SceneController>().gameState;
            }
            if (gameState == 1)
            {
                if (doAlready == false)
                {
                    playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
                    doAlready = true;
                }
               
                if (playerHolder != null)
                {
                    if (Vector3.Distance(transform.position, playerHolder.transform.position) >= 250f)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        gameObject.SetActive(true);
                    }
                }
        
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHolder"))
        {
            mapBuilder.GetComponent<MapBuilder>().onTile = transform;  
        }
    }
  
}
