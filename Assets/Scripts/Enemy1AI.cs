using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1AI : MonoBehaviour
{
    public GameObject playerHolder;
    public bool hasbeenChained = false;
    public Rigidbody rb;
    public GameObject gameController;
    
    //Move variables
    public float maxSpeed;
    public float speed;
    public float acceleration;
    public float deceleration;
    public bool isGrounded;
    public GameObject trailLeft;
    public GameObject trailRight;
    public GameObject smokeTrail;
    TrailRenderer left;
    TrailRenderer right;
    ParticleSystem smoke;
    //Brake variables
    bool shouldBrake = false;
    
    //Look at player variables
    float lookSpeed = 75f;
    float lookStep;
    Quaternion lookAtPlayer;

    //FieldOfView variables
    float angle;
    bool playerDetected;

    //De-Aggro variables
    GameObject[] waypointDeAggro;
    int i;


    //Light variables
    public GameObject redLight;
    public GameObject blueLight;
    Light red;
    Light blue;
    public float lightTime;
    // Start is called before the first frame update
    void Start()
    {
        
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        
        rb = GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        waypointDeAggro = GameObject.FindGameObjectsWithTag("WaypointDeAggro");
        i = Random.Range(0, waypointDeAggro.Length);
        left = trailLeft.GetComponent<TrailRenderer>();
        right = trailRight.GetComponent<TrailRenderer>();
        smoke = smokeTrail.GetComponent<ParticleSystem>();
        red = redLight.GetComponent<Light>();
        blue = blueLight.GetComponent<Light>();
    }
   
    // Update is called once per frame
    void FixedUpdate()
    {
        Light(0.1f);
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
                    left.emitting = true;
                    right.emitting = true;
                    smoke.Play();
                   
                }
                else if (playerDetected == true)
                {
                    shouldBrake = false;
                    left.emitting = false;
                    right.emitting = false;
                    smoke.Stop();

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
    void Light(float interval)
    {
        lightTime += Time.deltaTime;
        if (lightTime <= interval)
        {

            red.enabled = false;
            blue.enabled = true;
        } else if (lightTime > interval && lightTime <= interval *2 )
        {
            red.enabled = true;
            blue.enabled = false;
        } else if (lightTime > interval *2)
        {
            lightTime = 0;
        }
    }
    void FieldOfView() //Do I see the player for not?
    {
        Vector3 targetDir = playerHolder.transform.position - transform.position;
        angle = Vector3.Angle(targetDir, transform.forward);

        if (angle <= 10f)
        {
            playerDetected = true;
        } else if (angle > 10f)
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
