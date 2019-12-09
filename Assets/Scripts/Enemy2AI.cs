using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2AI : MonoBehaviour
{
    public bool hasbeenChained = false;
    public Rigidbody rb;
    public GameObject gameController;

    //Move variables
    public float maxSpeed;
    public float speed;
    public float acceleration;
    public float deceleration;
    public bool isGrounded;


    float lookSpeed = 90f;
    public float lookStep;
    Quaternion lookAtPlayer;

    //FieldOfView variables
    float angle;
    public bool playerDetected; 
    public GameObject playerHolder;

    //Destroy Timer
    float destroyTime;

    //De-Aggro variables
    GameObject[] waypointDeAggro;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        waypointDeAggro = GameObject.FindGameObjectsWithTag("WaypointDeAggro");
        i = Random.Range(0, waypointDeAggro.Length);
        if (playerHolder != null)
        {
            transform.LookAt(new Vector3(playerHolder.transform.position.x, transform.position.y, playerHolder.transform.position.z));
        }
  
       
        rb = GetComponent<Rigidbody>();

    }
    
        // Update is called once per frame
    void FixedUpdate()
    {
        if (playerHolder != null)
        {
            if (gameController.GetComponent<SceneController>().playerAggro == 1)
            {
                FieldOfView();
                if (playerDetected == false)
                {
                    destroyTime = destroyTime + Time.deltaTime;
                    if (destroyTime > 12)
                    {
                        gameObject.SetActive(false);
                        destroyTime = 0;
                    }
                }
                if (Vector3.Distance(transform.position, playerHolder.transform.position) >= 25 && playerDetected == true)
                {
                    lookStep = lookSpeed * Time.deltaTime;
                    lookAtPlayer = Quaternion.LookRotation(playerHolder.transform.position - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtPlayer, lookStep);
                }
                else
                {
                    lookSpeed = 0f;
                }


                if (isGrounded == true) // Forward Movement 
                {
                    if (speed < maxSpeed)
                    {
                        speed = speed + acceleration * Time.deltaTime;
                    }
                    else if (speed > -maxSpeed)
                    {
                        speed = speed - acceleration * Time.deltaTime;
                    }
                    rb.AddForce(transform.forward * speed * Time.deltaTime * 60);
                }
            }
            else if (gameController.GetComponent<SceneController>().playerAggro == 0)
            {

                Vector3 newDir = waypointDeAggro[i].transform.position - transform.position;

                lookStep = lookSpeed * Time.deltaTime * 4;
                lookAtPlayer = Quaternion.LookRotation(waypointDeAggro[i].transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtPlayer, lookStep);
            }
            rb.AddForce(transform.forward * speed * Time.deltaTime * 60);
        }

    }
    void FieldOfView() //Do I see the player for not?
    {
        Vector3 targetDir = playerHolder.transform.position - transform.position;
        angle = Vector3.Angle(targetDir, transform.forward);

        if (angle <= 30f)
        {
            playerDetected = true;
        }
        else if (angle > 30f)
        {
            playerDetected = false;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = true;

        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = false;
        }
    }
}
