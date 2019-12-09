using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3AI : MonoBehaviour
{
    public bool hasbeenChained = false;
    public Rigidbody rb;
    public GameObject gameController;

    //Move variables
    public float maxSpeed;
    public float speed;
    public float acceleration;
    public float deceleration;
    bool isGrounded;


    float lookSpeed = 40f;
    public float lookStep;
    Quaternion lookAtPlayer;

    //FieldOfView variables
    float angle;
    bool playerDetected;
    public GameObject playerHolder;

    //Destroy Timer
    float destroyTime;

    //Brake variables
    bool shouldBrake = false;

    //De-Aggro variables
    GameObject[] waypointDeAggro;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        if (playerHolder != null)
        {
            transform.LookAt(playerHolder.transform);
        }
        gameController = GameObject.FindGameObjectWithTag("GameController");
        waypointDeAggro = GameObject.FindGameObjectsWithTag("WaypointDeAggro");
        i = Random.Range(0, waypointDeAggro.Length);
        rb = GetComponent<Rigidbody>();

    }
    // Start is called before the first frame update
    void FieldOfView() //Do I see the player for not?
    {
        if (playerHolder != null)
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
      
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerHolder != null)
        {
            if (gameController.GetComponent<SceneController>().playerAggro == 1)
            {
                FieldOfView();
                Vector3 Direction = new Vector3(playerHolder.transform.position.x - transform.position.x,
                                                playerHolder.transform.position.y - transform.position.y,
                                                playerHolder.transform.position.z - transform.position.z);
                //transform.LookAt(playerHolder.transform);

                lookStep = lookSpeed * Time.deltaTime;
                lookAtPlayer = Quaternion.LookRotation(playerHolder.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookAtPlayer, lookStep);

                if (playerDetected == false)
                {
                    shouldBrake = true;
                }
                else if (playerDetected == true)
                {
                    shouldBrake = false;
                }

                if (isGrounded == true) // Forward Movement 
                {
                    if (shouldBrake == false)
                    {
                        if (speed < maxSpeed)
                        {
                            speed = speed + acceleration * Time.deltaTime;
                        }
                        else if (speed > -maxSpeed)
                        {
                            speed = speed - acceleration * Time.deltaTime;
                        }

                    }
                    else if (shouldBrake == true)
                    {
                        speed = speed - deceleration * Time.deltaTime;
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
