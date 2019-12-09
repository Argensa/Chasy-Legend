using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SafeZone : MonoBehaviour
{
    public GameObject gameController;
    bool goAgain;
    Vector3 thisPos;
    Vector3 direction;
    public GameObject playerHolder;
    public GameObject countText;
    float count = 5;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        countText = GameObject.FindGameObjectWithTag("CountText");
        if (playerHolder != null)
        {
            thisPos = new Vector3(transform.position.x, playerHolder.transform.position.y, transform.position.z);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (goAgain == false)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.Space))
            {
                goAgain = true;
     
            }
        }
        if (count <= 0)
        {
            goAgain = true;
            gameController.GetComponent<SceneController>().playerAggro = 1;
            countText.GetComponent<Text>().text = "No longer safe";
        }

        if (Vector3.Distance(transform.position, playerHolder.transform.position ) > 200f)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHolder"))
        {
            goAgain = false;
            other.gameObject.transform.LookAt(thisPos);
            direction = thisPos - playerHolder.transform.position;
           
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHolder"))
        {
            if (goAgain == false)
                {
                gameController.GetComponent<SceneController>().playerAggro = 0;
                if (Vector3.Distance(thisPos,other.gameObject.transform.position) > 2f)
                    {
                        
                        other.gameObject.GetComponent<Rigidbody>().velocity = direction * 1;
                     } else
                    {
                      other.gameObject.GetComponent<Rigidbody>().velocity = direction * 0 ;
                    }
                count = count - Time.deltaTime;
                countText.GetComponent<Text>().text = count.ToString("00");
                }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHolder"))
        {
            gameController.GetComponent<SceneController>().playerAggro = 1;
            count = 5;
            countText.GetComponent<Text>().text = "";
        }
    }
}
