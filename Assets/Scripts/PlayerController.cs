using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
    RaycastHit hit;
    public int gameState;

    [Header("Look at Mouse")]
    public Camera mainCamera;

    [Header("Control")]
    bool grounded = false;
    public float speed = 0f;
    public float acceleration;
    public float deceleration;
    public float maxSpeed;
    public float turnRate;
    public bool braking = false;
    float orgTurnRate;
    float brakeTurnRate;
    float jumpTime;
    Vector3 touchPosition;

    [Header("Targeting")]
    public float sumEnemy;
    public GameObject[] eachEnemy;
    public GameObject allEnemy;

    [Header("Attack")]
    public float attackTime;
    public float attackRange;
    public float attackSpeed;
    public float attackDamage;
    public GameObject currentTarget = null;
    public GameObject[] potentialTarget = new GameObject[100];
    public bool ableFire = true;
    public GameObject playerBullet;
    public GameObject gunHead;
    public GameObject gunObj;


    [Header("Trail")]
    public GameObject trailLeft;
    public GameObject trailRight;
    public GameObject smokeTrail;
    TrailRenderer left;
    TrailRenderer right;
    ParticleSystem smoke;


    [Header("Tilter")]
    public GameObject tilter;
    public GameObject turretTilt;
    float bankAmount;
    bool tiltLeft;
    bool tiltRight;


    [Header("PlayerDeath")]
    public GameObject gameController;
    public GameObject explosion;
    bool gameOver = false;
    [Header("CameraShake")]
    public CameraShake cameraShake;
    

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        allEnemy = GameObject.FindGameObjectWithTag("AllEnemy");
        float allChild = allEnemy.transform.childCount;
        turretTilt = GameObject.FindGameObjectWithTag("TurretTilt");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        orgTurnRate = turnRate;
        brakeTurnRate = turnRate * 2;
        left = trailLeft.GetComponent<TrailRenderer>();
        right = trailRight.GetComponent<TrailRenderer>();
        smoke = smokeTrail.GetComponent<ParticleSystem>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameState = gameController.GetComponent<SceneController>().gameState;
        if (gameState == 1)
        {
            Move();
            TargetFunc();
            AttackFunc();
            turretTilt.transform.eulerAngles = tilter.transform.eulerAngles;
        }
      
    }
    void Move()
    {
        jumpTime += Time.deltaTime;
        Vector3 Direction = new Vector3(transform.forward.x, rb.velocity.y / speed, transform.forward.z);
        //rb.velocity = Direction * speed * Time.deltaTime;
       
        if (Input.touchCount > 0)
        {
           
                Touch touch = Input.GetTouch(0);
                touchPosition = touch.position;
            
           

            if (touchPosition.x < Screen.width / 2 || Input.GetKey(KeyCode.A))
            {
                if (grounded == true)
                {
                    rb.drag = 1.2f;
                    transform.Rotate(0, -30 * Time.deltaTime * turnRate, 0, Space.Self);
                    braking = true;
                    speed = speed - deceleration / 40 * Time.deltaTime;
                    if (speed >= 30)
                    {
                        if (bankAmount < 2)
                        {
                            bankAmount += Time.deltaTime;
                        }


                        left.emitting = true;
                        right.emitting = true;
                        smoke.Play();
                        tilter.transform.localEulerAngles = new Vector3(0, 0, 7 * bankAmount);

                    }

                    //transform.localEulerAngles = new Vector3(0, 0, 5);
                }
            }
            //else if (Input.GetKey(KeyCode.D))
            else if (touchPosition.x > Screen.width / 2 || Input.GetKey(KeyCode.D))
            {
                if (grounded == true)
                {
                    rb.drag = 1.2f;
                    transform.Rotate(0, 30 * Time.deltaTime * turnRate, 0, Space.Self);
                    braking = true;
                    speed = speed - deceleration / 40 * Time.deltaTime;
                    if (speed >= 30)
                    {
                        if (bankAmount > -2)
                        {
                            bankAmount -= Time.deltaTime;
                        }
                        left.emitting = true;
                        right.emitting = true;
                        smoke.Play();
                        tilter.transform.localEulerAngles = new Vector3(0, 0, 7 * bankAmount);

                    }
                    //transform.localEulerAngles = new Vector3(0, 0, -5);
                }
            }
            //   }
           
        }
        else
        {
            if (bankAmount < -0.02f)
            {
                bankAmount += Time.deltaTime * 3;
            }
            if (bankAmount > 0.02f)
            {
                bankAmount -= Time.deltaTime * 3;
            }
            tilter.transform.localEulerAngles = new Vector3(0, 0, 8 * bankAmount);

            braking = false;
            rb.drag = 1f;
            left.emitting = false;
            right.emitting = false;
            smoke.Stop();

        }
        if (Input.touchCount >= 2)
            {
                // if (Input.GetKey(KeyCode.Space))
                //{
                if (grounded == true)
                {
                //Debug.Log("Space Down");
                /* braking = true;
                 turnRate = brakeTurnRate;
                 if (speed > -maxSpeed)
                 {
                     speed = speed - deceleration * Time.deltaTime;
                 }
               */
                   if (jumpTime > 5)
                    {
                        rb.AddForce(0, 25, 0, ForceMode.Impulse);
                        jumpTime = 0;
                    }
                


                }
            }
        else if (Input.touchCount < 2)
            {
            braking = false;
            if (braking == false)
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
            turnRate = orgTurnRate;
               
            }
        if (gameOver == false)
        {
            rb.AddForce(Direction * speed * Time.deltaTime * 60);
        }
        else if (gameOver == true)
        {
            Time.timeScale = 0.5f;
        }



    }
    void ForceField()
    {

    }

    //Check if grounded or not
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            grounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            grounded = false;
        }
    }

    void LookAtMouse () //not using
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) //throw out a raycast from the camera
        {
            Transform objectHit = hit.transform;


            if (objectHit.gameObject.layer == 9) //if that ray hits the ground then
            {

                Vector3 hitY = new Vector3(hit.point.x, transform.position.y, hit.point.z); //create a vector3 that points toward that place, however keep the elevation of the object
                transform.LookAt(hitY); //look at the place

            }
        }
    }
    void TiltFunc () // not using
    {
        RaycastHit hit2;
        Ray groundRay = new Ray(transform.position, -transform.up);
        //Fire a ray at the ground and get the ground "normal" information 
        // rotate the player accordingly
       if (Physics.Raycast(groundRay, out hit2))
        {
            Transform objectHit = hit2.transform;
          
           
            if (objectHit.gameObject.layer == 9)
            {
                transform.rotation = Quaternion.LookRotation(Vector3.Cross(Vector3.right, hit2.normal), hit2.normal);
                
            }
        }
    }

    void TargetFunc ()
    {
        //allEnemy.GetComponentsInChildren<Transform>();
        

    /*
        for (int j = 0; j < allEnemy.transform.childCount; j++)
        {
            for (int i = 0; i < allEnemy.transform.childCount - 1; i++)
            {
                if (Vector3.Distance(transform.position, allEnemy.transform.GetChild(i).position) > Vector3.Distance(transform.position, allEnemy.transform.GetChild(i + 1).position))
                {

                    indexNumber = allEnemy.transform.GetChild(i).GetSiblingIndex();
                    allEnemy.transform.GetChild(i + 1).SetSiblingIndex(indexNumber);

                    allEnemy.transform.GetChild(i).SetSiblingIndex(indexNumber + 1);


                }
            }


        } 
    */
        foreach (Transform enemy in allEnemy.transform)
        {
            //if the enemy that the foreach loop check is near the player
            if ( Vector3.Distance(enemy.position, transform.position) <= attackRange)
            {
                //Debug.Log("Nho hon AttackRange");
                if (currentTarget == null) //if you currently have no target, make it a target
                {
                    
                  //  currentTarget = enemy.gameObject;
                   
                   
                }
                if (currentTarget != null) //if you have a target then look at that target
                {
                   // gunObj.transform.LookAt(currentTarget.transform);
                   
                    //currentTarget.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                //if target ==null -> attack 
                //This results in the first enemy that enter your range being attacked
                //if he dies because the code is in update it will choose another enemy to attack, though not necessarily the closest one
                //At least now we know where the enemies are in the editor 

            }
            if (currentTarget != null)
            {
                if (Vector3.Distance(currentTarget.transform.position, transform.position) >= attackRange)
                {
                    //if too far then stop looking/targeting it

                    currentTarget = null;
                }
            }
           
        }
    }

    void AttackFunc ()
    {
        attackTime = attackTime + Time.deltaTime;
       
        if (currentTarget != null)
        {
            //gunObj.transform.LookAt(currentTarget.transform);
            ableFire = true;
            gunObj.transform.LookAt(new Vector3(currentTarget.GetComponent<EnemyTargetScript>().thisTarget.transform.position.x,
                                    gunObj.transform.position.y,
                                    currentTarget.GetComponent<EnemyTargetScript>().thisTarget.transform.position.z));
            if (Input.GetKeyDown(KeyCode.E))
            {
                currentTarget.SetActive(false);
            }
            if (attackTime >= attackSpeed && ableFire == true)
            {
                
                GameObject bullet = SCR_Pool.GetFreeObject(playerBullet);
                bullet.GetComponent<PlayerBulletScript>().Spawn(gunHead.transform.position, gunObj.transform.rotation);
                //Instantiate(playerBullet, gunHead.transform.position, gunObj.transform.rotation);
                //Instanstiate bullet that flies towards currentTarget
                attackTime = 0;
            }
        }
        else if (currentTarget == null)
        {
            ableFire = false;
        }


        //seems like I don't even need this lol
      /*  if (currentTarget == null)
        {
            for (int i = 1; i <= sumEnemy; i++)
            {
                if (Vector3.Distance(potentialTarget[i].transform.position, transform.position) <= attackRange)
                {
                    currentTarget = potentialTarget[i];
                    Debug.Log(currentTarget);
                }
            }
        } */
        //we need a base attack time that is changable
        // if target != null && ableFire => attack
        //bullet properties will be in bulletScript
    }


    //Bullet properties: 
    // Chain, Fork, Pierce, AoE, DoT

    //How do I port to IOS
    //How do I use 3D touch to block attacks?
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 16 || collision.gameObject.layer == 11 || collision.gameObject.layer == 10 || collision.gameObject.layer == 17)
        {
            if (collision.gameObject.activeSelf == true)
            {
                this.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
                GameObject explosionObj = SCR_Pool.GetFreeObject(explosion);
                explosionObj.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                
                gameController.GetComponent<SceneController>().gameOver = true;
                rb.AddForce(5, 20, 5,ForceMode.Impulse);
                if (collision.gameObject.GetComponent<Rigidbody>() != null)
                {
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(5, 5, 5, ForceMode.Impulse);
                }
                gameOver = true;
                
                //gameObject.SetActive(false);
            }
           
        }
        
    }
   
}
