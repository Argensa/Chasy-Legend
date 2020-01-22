using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSpawner : MonoBehaviour
{
    public GameObject gameController;
    int gameState;


    public GameObject playerHolder;
    public int gameStage;
    public float radius;
    public float minRadius;
    RaycastHit hit;
    RaycastHit hitPlayer;
    bool isGrounded = false;
    bool ableSpawn = true;
    Rigidbody rb;
    public Transform map;
    int money;
    Vector3 position;
    Vector3 direction;
    bool doAlready;

   
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
       
        
        rb = GetComponent<Rigidbody>();
        gameStage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>().gameStage;
       
    }

    // Update is called once per frame
    void Update()
    {
        Spin();
        gameState = gameController.GetComponent<SceneController>().gameState;
        if (gameState == 1)
        {
            if (doAlready == false)
            {
                playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
                Reposition();
                SpawnMoney();
                doAlready = true;
            }
            
            
            if (Vector3.Distance(transform.position, playerHolder.transform.position) <= 25f && Vector3.Distance(transform.position, playerHolder.transform.position) >= 5f)
            {
                direction = playerHolder.transform.position - transform.position;
                rb.velocity = direction.normalized * 45;
            }
        }
      
    }
    void SpawnMoney()
    {
        //GroundCheck();
        //CloseCheck();
        rb.velocity = Vector3.zero;
        Reposition();
        if (isGrounded == true && ableSpawn == true)
        {
            
            //SpawnMoney(10);
        }
        if (isGrounded == false || ableSpawn == false)
        {
            Reposition();
        }
    }
    void Reposition()
    {
        position = new Vector3(Random.Range(playerHolder.transform.position.x - minRadius - radius, playerHolder.transform.position.x + minRadius + radius),
                               playerHolder.transform.position.y,
                               Random.Range(playerHolder.transform.position.z - minRadius - radius, playerHolder.transform.position.z + minRadius + radius));
        transform.position = position;
        money = Random.Range(25, gameStage * 2 + 50);
       // Debug.Log(money);
    }
    void GroundCheck() //check to see if this thing is above the ground or not
    {
        //split this function into Move and GroundCheck

       
        Vector3 rayDirection = new Vector3(transform.position.x, -2000, transform.position.z);
        Debug.DrawLine(transform.position, rayDirection, Color.red);
        Ray ray = new Ray(this.transform.position, rayDirection);
        if (Physics.Raycast(ray, out hit))
        {
            {
                Transform objectHit = hit.transform;

                if (objectHit.gameObject.layer == 9 || objectHit.gameObject.layer == 10) //if that ray hits the ground then
                { //10 == walls

                    isGrounded = true; //this thing is considered grounded (or ABOVE THE GROUND)

                }
                else isGrounded = false;

            }

        }


    }
  
    void CloseCheck() //check if it is too close 
    {
        if (Vector3.Distance(transform.position, playerHolder.transform.position) <= minRadius)
        {
            ableSpawn = false;
        }
        if (Vector3.Distance(transform.position, Vector3.zero) >= 450)
        {
            ableSpawn = false;
        }
    }
    void Spin()
    {
        transform.Rotate(0, 2, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHolder"))
        {
            SpawnMoney();
            gameStage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>().money += money;
        }
    }

}
