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


   
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
       
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        rb = GetComponent<Rigidbody>();
        gameStage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>().gameStage;
        Reposition();
        SpawnMoney();
    }

    // Update is called once per frame
    void Update()
    {
        gameState = gameController.GetComponent<SceneController>().gameState;
        if (gameState == 1)
        {
            Spin();
            if (Vector3.Distance(transform.position, playerHolder.transform.position) <= 15f && Vector3.Distance(transform.position, playerHolder.transform.position) >= 5f)
            {
                direction = playerHolder.transform.position - transform.position;
                rb.velocity = direction * 8;
            }
        }
      
    }
    void SpawnMoney()
    {
        GroundCheck();
        WallCheck();
        CloseCheck();
        rb.velocity = Vector3.zero;
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
        position = new Vector3(Random.Range(playerHolder.transform.position.x - radius, playerHolder.transform.position.x + radius),
                                      playerHolder.transform.position.y,
                                      Random.Range(playerHolder.transform.position.z - radius, playerHolder.transform.position.z + radius));
        money = Random.Range(25, gameStage * 5 + 50);
       // Debug.Log(money);
    }
    void GroundCheck() //check to see if this thing is above the ground or not
    {
        //split this function into Move and GroundCheck

        transform.position = position;
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
    void WallCheck() //check to see if this thing has a line of sight towards the player or not (so that it does not spawn creatures outside of the arena)
    {
        Vector3 rayToPLayerDirection = new Vector3(playerHolder.transform.position.x, transform.position.y, playerHolder.transform.position.z);
        Ray rayToPlayer = new Ray(this.transform.position, rayToPLayerDirection);
        Debug.DrawLine(transform.position, rayToPLayerDirection, Color.blue);

        if (Physics.Raycast(rayToPlayer, out hitPlayer)) //Cast a ray from the spawner towards the player. If there is a cliff preset between them then don't spawn anything on the other side of the cliff. I am not going to write an AI that can bypass these cliffs before shooting
        {
            Transform objectHit = hitPlayer.transform;

            if (objectHit.gameObject.layer == 17)

            {
                Debug.Log(objectHit);
                ableSpawn = false;
            }
            else ableSpawn = true;
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
        transform.localEulerAngles = new Vector3(0, 5, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHolder"))
        {
            SpawnMoney();
            gameStage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>().money += money;
        }
    }
    /*   void SpawnMoney(float interval)
       {

           if (moneyObj != null)
           {
               moneyTime = moneyTime + Time.deltaTime;
               if (moneyTime >= interval)
               {
                   Reposition();
                   GameObject cashObj = SCR_Pool.GetFreeObject(moneyObj);
                   cashObj.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                   //Instantiate(wasp, transform.position, transform.rotation, allEnemy.transform);
                   moneyTime = 0;
               }
           }
       }
     */
}
