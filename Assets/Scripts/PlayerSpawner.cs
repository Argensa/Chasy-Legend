using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public float radius;
    RaycastHit hit;
    RaycastHit hitPlayer;
    bool isGrounded = false;
    bool ableSpawn = true;


    public GameObject arrow;
    public GameObject dangerSign;
    public GameObject gun;
    public GameObject player;
    public GameObject safeCircle;

    public bool tooClose = false;
    Vector3 position;
    void Awake()
    {
        Reposition();
        GroundCheck();
        WallCheck();
        CloseCheck();


        if (isGrounded == true && ableSpawn == true)
        {
          

        }
        if (isGrounded == false || ableSpawn == false)
        {
            Reposition();
        }
    }

    // Update is called once per frame
    void Update()
    {

        
    }
    void Reposition()
    {
        position = new Vector3(Random.Range(0 - radius, 0 + radius),-0.8f, Random.Range(0 - radius, 0 + radius));
        tooClose = false;
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
        Vector3 rayToMiddle = new Vector3(0, transform.position.y, 0);
        Ray rayToMiddleRay = new Ray(this.transform.position, rayToMiddle);
        Debug.DrawLine(transform.position, rayToMiddle, Color.blue);

        if (Physics.Raycast(rayToMiddleRay, out hitPlayer)) //Cast a ray from the spawner towards the player. If there is a cliff preset between them then don't spawn anything on the other side of the cliff. I am not going to write an AI that can bypass these cliffs before shooting
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
    void CloseCheck()
    {
        if (tooClose == true)
        {
            Reposition();
        }
    }
    
}
