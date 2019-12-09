using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{

    public GameObject playerHolder;
    public float radius;
    RaycastHit hit;
    RaycastHit hitPlayer;
    bool isGrounded = false;
    bool ableSpawn = true;

    Vector3 position;

    [Header("Powerup types")]
    public GameObject giftBox;

    [Header("Timer")]
    float timer;

    [Header("Spawn")]
    int randomNum;

    int gameState;
    public GameObject gameController;
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        gameController = GameObject.FindGameObjectWithTag("GameController");
       

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

                SpawnPower(10);
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
                                      playerHolder.transform.position.y,
                                      Random.Range(playerHolder.transform.position.z - radius, playerHolder.transform.position.z + radius));
    }
    void GroundCheck() //check to see if this thing is above the ground or not
    {
     

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
        if (Vector3.Distance(transform.position, playerHolder.transform.position) <= 15f)
        {
            ableSpawn = false;
        }
    }
    void SpawnPower(float interval)
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            Reposition();
            GameObject powerBox = SCR_Pool.GetFreeObject(giftBox);
            powerBox.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
            //Instantiate(wasp, transform.position, transform.rotation, allEnemy.transform);
            timer = 0;
        }
    }
}
