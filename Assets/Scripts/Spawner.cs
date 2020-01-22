using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject gameController;
    int gameState;

    public GameObject playerHolder;
    public float gameStage;
    public float radius;
    public float minRadius;
    RaycastHit hit;
    RaycastHit hitPlayer;
    bool isGrounded = false;
    bool ableSpawn = true;
   
    Vector3 position;

    [Header("Enemy types")]
    public GameObject aPC;
    public GameObject policeCar;
    public GameObject tank;



    [Header("Enemy timers")]
    float timerTank = 2;
    public float timerPoliceCar = 2;
    float timerAPC = 2;
    float timerFrog;
    float timerDragon;


    [Header("FindRoot")]
    public GameObject allEnemy;



    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        allEnemy = GameObject.FindGameObjectWithTag("AllEnemy");
        gameStage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>().gameStage;
       
    }

    // Update is called once per frame
    void Update()
    {
        gameState = gameController.GetComponent<SceneController>().gameState;
        if (gameState == 1)
        {
            GroundCheck();
            WallCheck();
            CloseCheck();


            transform.LookAt(playerHolder.transform);
            if (isGrounded == true && ableSpawn == true)
            {
                SpawnPoliceCar(4 - (gameStage * 10 / 20));
                SpawnAPC(10 - (gameStage * 10 / 20));
                SpawnTank(6 - (gameStage * 10 / 20));
                // This does not yield the correct number of enemies each time it is run. 
                // Make it so that it stops when the designated number of enemies is reached

            }
            if (isGrounded == false || ableSpawn == false)
            {
                Reposition();
            }


           
        }
       
    }
    void Reposition()
    {
        position = new Vector3(Random.Range(playerHolder.transform.position.x - radius, playerHolder.transform.position.x + radius),
                                      playerHolder.transform.position.y + 0.5f,
                                      Random.Range(playerHolder.transform.position.z - radius, playerHolder.transform.position.z + radius));
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
    }

   
    void SpawnPoliceCar(float interval)
    {
        if (policeCar != null)
        {
            if (interval <= 0)
            {
                interval = 0.2f;

            }
            if (policeCar != null)
            {
                timerPoliceCar = timerPoliceCar + Time.deltaTime;
                if (timerPoliceCar >= interval)
                {
                    Reposition();
                    GameObject normalPoliceCar = SCR_Pool.GetFreeObject(policeCar);
                    normalPoliceCar.transform.parent = allEnemy.transform;
                    normalPoliceCar.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                    //Instantiate(wasp, transform.position, transform.rotation, allEnemy.transform);
                    timerPoliceCar = 0;
                }
            }

        }

    }
    void SpawnAPC(float interval)
    {
        if (aPC != null)
        {
            if (interval <= 0)
            {
                interval = 0.3f;

            }
            if (aPC != null)
            {
                timerAPC = timerAPC + Time.deltaTime;
                if (timerAPC >= interval)
                {
                    Reposition();
                    GameObject apcCar = SCR_Pool.GetFreeObject(aPC);
                    apcCar.transform.parent = allEnemy.transform;
                    apcCar.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                    //Instantiate(wasp, transform.position, transform.rotation, allEnemy.transform);
                    timerAPC = 0;
                }
            }
        }
       

    }
    void SpawnTank(float interval)
    {
        if (tank != null)
        {
            if (interval <= 0)
            {
                interval = 0.5f;

            }
            if (aPC != null)
            {
                timerTank = timerTank + Time.deltaTime;
                if (timerTank >= interval)
                {
                    Reposition();
                    GameObject tankCar = SCR_Pool.GetFreeObject(tank);
                    tankCar.transform.parent = allEnemy.transform;
                    tankCar.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                    //Instantiate(wasp, transform.position, transform.rotation, allEnemy.transform);
                    timerTank = 0;
                }
            }

        }

    }
}
