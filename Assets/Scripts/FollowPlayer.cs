using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject playerHolder;
    public GameObject gameController;
    bool doAlready;
    Vector3 offset;
    public GameObject gunPlatform;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");

    }

    // Update is called once per frame
    void Update()
    {
        if (gameController.GetComponent<SceneController>().gameState == 1 )
        {
            if (doAlready == false)
            {
                playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
                offset = playerHolder.transform.position - transform.position;
                doAlready = true;

              
            }
            //Vector3 updatePos = new Vector3(playerHolder.transform.position.x, playerHolder.transform.position.y, playerHolder.transform.position.z);
            //transform.position = updatePos;
            transform.position = playerHolder.transform.position - offset;
            if (this.gameObject.CompareTag("Ground"))
            {
                Vector3 updatePosGround = new Vector3(playerHolder.transform.position.x, -2, playerHolder.transform.position.z);
                transform.position = updatePosGround;
            }
        }
       
      
    }
    
}
