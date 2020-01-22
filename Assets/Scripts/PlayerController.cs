using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    Rigidbody rb;
    RaycastHit hit;
    public int gameState = 0;

    [Header("Look at Mouse")]
    public Camera mainCamera;

    [Header("Control")]
    bool grounded = false;
    public float speed = 0f;
    float turnTimeLeft;
    float turnTimeRight;
    float haveBeenTurningFor;

    [Header("Linked to GameController")]
    public float acceleration;
    public float deceleration;
    public float maxSpeed;
    public float turnRate;
    int carID;

    public bool braking = false;
    float orgTurnRate;
    float brakeTurnRate;
    float jumpTime;
    Vector3 touchPosition;

    [Header("Targeting")]
    public float sumEnemy;
    public GameObject[] eachEnemy;
    public GameObject allEnemy;
    Quaternion lookAtEnemy;

    [Header("Attack")]
    public float attackTime;
    public float attackRange;
    public float attackSpeed;
    public float attackDamage;
    public GameObject currentTarget = null;
    public GameObject[] potentialTarget = new GameObject[100];
    public bool ableFire = true;
 
    public GameObject[] gunHead;
    public GameObject[] gunObj;


    public GameObject playerBullet;

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
    [SerializeField]
    float bankAmount;
    public float bankMultiplier; //Car specific

    bool tiltLeft;
    bool tiltRight;


    [Header("PlayerDeath")]
    public GameObject gameController;
    public GameObject explosion;
    bool gameOver = false;
    public float playerHealth = 100;
    float orgHealth; 
    bool dieAlready = false;

    [Header("CameraShake")]
    public CameraShake cameraShake;

    bool doAlready;
    int phoneOrPC;

    public float dragRate;

    public GameObject flame;
    public ParticleSystem shootPE;


    Quaternion orgRotation;

    [Header("CarSpecialAbilities")]
    public float healFactor;
    // Start is called before the first frame update
    void Start()
    {
        orgRotation = transform.localRotation;
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

        flame = GameObject.FindGameObjectWithTag("Flame");
        orgHealth = playerHealth;
        shootPE = GameObject.FindGameObjectWithTag("ShootPE").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameState == 0)
        {
            gameState = gameController.GetComponent<SceneController>().gameState;
        }

        if (gameState == 1)
        {
            if (doAlready == false)
            {
                if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
                {
                    
                    phoneOrPC = 0;
                }
                else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    
                    phoneOrPC = 1;
                }
                doAlready = true;
            }

            if (phoneOrPC == 0)
            {
                ConTrolComputer();
            } else if (phoneOrPC == 1)
            {
                Move();
            }
                
                
            TargetFunc();
            AttackFunc();
            //turretTilt.transform.eulerAngles = tilter.transform.eulerAngles;
        }
        //playerHealth = playerHealth + healFactor;
        if (playerHealth <= orgHealth / 2)
        {
            flame.transform.position = transform.position;
        } else
        {
            flame.transform.position = new Vector3(0, -20, 0);
        }
        if (playerHealth <= 0)
        {
            if (dieAlready == false)
            {
                rb.AddForce(5, 20, 5, ForceMode.Impulse);
                StartCoroutine(cameraShake.Shake(.5f, .5f));
                GameObject explosionObj = SCR_Pool.GetFreeObject(explosion);
                explosionObj.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                gameController.GetComponent<SceneController>().gameOver = true;
                gameOver = true;
                dieAlready = true;
            }
          
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

            if (touchPosition.x < Screen.width / 2)
            {
                if (grounded == true)
                {
                    rb.drag = dragRate;
                    haveBeenTurningFor += Time.deltaTime;
                    transform.Rotate(0, -30 * Time.deltaTime * turnRate, 0, Space.Self);
                    braking = true;
                    speed = speed - deceleration / 40 * Time.deltaTime;

                    if (speed >= 30)
                    {

                        turnTimeLeft += Time.deltaTime;
                        if (bankAmount < 1)
                        {
                            bankAmount += Time.deltaTime;
                            tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                            if (bankAmount > 0) //Nho hon 1 cai so turn nao day tinh bang euler angle
                            {

                                //tilter.transform.Rotate(0, 0, -bankAmount * bankMultiplier, Space.Self);
                                //tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                            }
                            if (turnTimeLeft > .3f)
                            {

                                left.emitting = true;
                                right.emitting = true;
                                smoke.Play();
                            }
                        }






                        //  tilter.transform.localEulerAngles = new Vector3(0, 12 * bankAmount, 12 * bankAmount);

                    }

                    //transform.localEulerAngles = new Vector3(0, 0, 5);
                }
            }
            //else if (Input.GetKey(KeyCode.D))
            else if (touchPosition.x > Screen.width / 2)
            {
                if (grounded == true)
                {
                    rb.drag = dragRate;
                    haveBeenTurningFor += Time.deltaTime;

                    transform.Rotate(0, 30 * Time.deltaTime * turnRate, 0, Space.Self);
                    braking = true;
                    speed = speed - deceleration / 40 * Time.deltaTime;
                    if (speed >= 30)
                    {

                        turnTimeRight += Time.deltaTime;
                        if (bankAmount > -1)
                        {
                            bankAmount -= Time.deltaTime;
                            tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                            if (bankAmount < 0) //Lon hon 1 cai so turn nao day tinh bang euler angle
                            {

                                //tilter.transform.Rotate(0, 0, -bankAmount * bankMultiplier, Space.Self);
                                //tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                            }
                            if (turnTimeRight > .3f)
                            {

                                left.emitting = true;
                                right.emitting = true;
                                smoke.Play();
                            }
                        }




                        //   tilter.transform.localEulerAngles = new Vector3(0, -12 * bankAmount, -12 * bankAmount);

                    }
                    //transform.localEulerAngles = new Vector3(0, 0, -5);
                }

            }
        }

        else if (Input.touchCount == 2)
        {
            if (grounded == true)
            {

                braking = true;
                turnRate = brakeTurnRate;
                if (speed > -maxSpeed)
                {
                    speed = speed - deceleration * Time.deltaTime;
                }
            }

            haveBeenTurningFor = 0f;
            turnTimeLeft = 0f;
            turnTimeRight = 0f;
            if (bankAmount < -0.0005f)
            {
                bankAmount += Time.deltaTime * 5;
                //tilter.transform.localEulerAngles = new Vector3(0, bankAmount, bankAmount);
                tilter.transform.rotation = Quaternion.Lerp(tilter.transform.rotation, transform.rotation, .01f * Time.time);

            }
            if (bankAmount >= -0.005f && bankAmount <= 0.005f)
            {
                bankAmount = 0;
                //tilter.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (bankAmount > 0.0005f)
            {
                bankAmount -= Time.deltaTime * 5;
                //tilter.transform.localEulerAngles = new Vector3(0, bankAmount, bankAmount);
                tilter.transform.rotation = Quaternion.Lerp(tilter.transform.rotation, transform.rotation, .01f * Time.time);
            }



            braking = false;
            rb.drag = 1f;
            left.emitting = false;
            right.emitting = false;
            smoke.Stop();

        }
        else if (Input.touchCount == 0)
        {
            if (turnTimeLeft >= 0)
            {
                turnTimeLeft -= Time.deltaTime;
            }
            if (turnTimeRight >= 0)
            {
                turnTimeRight -= Time.deltaTime;
            }


            if (bankAmount < 0)
            {
                bankAmount += Time.deltaTime * 2;
                tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                //tilter.transform.rotation = Quaternion.Lerp(tilter.transform.rotation, transform.rotation, .01f * Time.timeSinceLevelLoad);

            }

            else if (bankAmount > 0)
            {
                bankAmount -= Time.deltaTime * 2;
                tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                //tilter.transform.rotation = Quaternion.Lerp(tilter.transform.rotation, transform.rotation, .01f * Time.timeSinceLevelLoad);
            }




            braking = false;
            rb.drag = 1f;
            left.emitting = false;
            right.emitting = false;
            smoke.Stop();

        }




        if (speed < maxSpeed)
        {
            speed = speed + acceleration * Time.deltaTime;
        }
        else if (speed > -maxSpeed)
        {
            speed = speed - acceleration * Time.deltaTime;
        }
        
        if (gameOver == false)
        {
            rb.AddForce(Direction * speed * Time.deltaTime * 60 * 1.2f);
        }
        else if (gameOver == true)
        {
            Time.timeScale = 0.5f;
        }



            
    }
    void ConTrolComputer()
    {
        Vector3 Direction = new Vector3(transform.forward.x, rb.velocity.y / speed, transform.forward.z);
        if (Input.GetKey(KeyCode.A))
        {
            if (grounded == true)
            {
            

                rb.drag = dragRate;
                haveBeenTurningFor += Time.deltaTime;
                transform.Rotate(0, -30 * Time.deltaTime * turnRate, 0, Space.Self);
                braking = true;
                speed = speed - deceleration / 40 * Time.deltaTime;

                if (speed >= 30)
                {

                    turnTimeLeft += Time.deltaTime;
                    if (bankAmount < 1)
                    {
                        bankAmount += Time.deltaTime *3 ;
                        tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                        if (bankAmount > 0) //Nho hon 1 cai so turn nao day tinh bang euler angle
                        {
                            
                            //tilter.transform.Rotate(0, 0, -bankAmount * bankMultiplier, Space.Self);
                            
                        }
                        if (turnTimeLeft > .3f)
                        {

                            left.emitting = true;
                            right.emitting = true;
                            smoke.Play();
                        }
                    }






                    //  tilter.transform.localEulerAngles = new Vector3(0, 12 * bankAmount, 12 * bankAmount);

                }

                //transform.localEulerAngles = new Vector3(0, 0, 5);
            }
        }
        
        else if (Input.GetKey(KeyCode.D))
        {
            
            if (grounded == true)
            {
                

                rb.drag = dragRate;
                haveBeenTurningFor += Time.deltaTime;
             
                transform.Rotate(0, 30 * Time.deltaTime * turnRate, 0, Space.Self);
                braking = true;
                speed = speed - deceleration / 40 * Time.deltaTime;
                if (speed >= 30)
                {
                
                    turnTimeRight += Time.deltaTime;
                    if (bankAmount > -1)
                    {
                        bankAmount -= Time.deltaTime * 3;
                        tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                        if (bankAmount < 0) //Lon hon 1 cai so turn nao day tinh bang euler angle
                        {

                            //tilter.transform.Rotate(0, 0, -bankAmount * bankMultiplier, Space.Self);
                            
                        }
                        if (turnTimeRight > .3f)
                        {

                            left.emitting = true;
                            right.emitting = true;
                            smoke.Play();
                        }
                    }




                    //   tilter.transform.localEulerAngles = new Vector3(0, -12 * bankAmount, -12 * bankAmount);

                }
                //transform.localEulerAngles = new Vector3(0, 0, -5);
            }
        }

        else if (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false || Input.touchCount == 0 || Input.GetKey(KeyCode.A) == true && Input.GetKey(KeyCode.D) == true || Input.touchCount == 2)
        {
            if (turnTimeLeft >= 0)
            {
                turnTimeLeft -= Time.deltaTime;
            }
            if (turnTimeRight >= 0)
            {
                turnTimeRight -= Time.deltaTime;
            }


            if (bankAmount < 0)
            {
                bankAmount += Time.deltaTime * 3;
                tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                //tilter.transform.rotation = Quaternion.Lerp(tilter.transform.rotation, transform.rotation, .01f * Time.timeSinceLevelLoad);

            }
            else if (bankAmount > 0)
            {
                bankAmount -= Time.deltaTime * 3;
                tilter.transform.localEulerAngles = new Vector3(0, 0, -bankAmount * bankMultiplier);
                //tilter.transform.rotation = Quaternion.Lerp(tilter.transform.rotation, transform.rotation, .01f * Time.timeSinceLevelLoad);
            }
           
            



            braking = false;
            rb.drag = 1f;
            left.emitting = false;
            right.emitting = false;
            smoke.Stop();

        }
        if (Input.GetKey(KeyCode.Space))
        {

            if (grounded == true)
            {

                braking = true;
                turnRate = brakeTurnRate;
                if (speed > -maxSpeed)
                {
                    speed = speed - deceleration * Time.deltaTime;
                }
            }
        }
        else
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
        if (speed < maxSpeed)
        {
            speed = speed + acceleration * Time.deltaTime;
        }
        else if (speed > -maxSpeed)
        {
            speed = speed - acceleration * Time.deltaTime;
        }

        if (gameOver == false)
        {
            rb.AddForce(Direction * speed * Time.deltaTime * 60 * 1.2f);
        }
        else if (gameOver == true)
        {
            Time.timeScale = 0.5f;
        }
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

   
    void TargetFunc ()
    {
     
        foreach (Transform enemy in allEnemy.transform)
        {
           
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
       
        if (currentTarget != null && currentTarget.transform.position.y >= -1)
        {
           
            ableFire = true;
          
            float lookStep = 540 * Time.deltaTime;

            for (int i = 0; i <= gunObj.Length - 1; i++)
            {
                
                lookAtEnemy = Quaternion.LookRotation(currentTarget.GetComponent<EnemyTargetScript>().thisTarget.transform.position - gunObj[i].transform.position);
                gunObj[i].transform.rotation = Quaternion.RotateTowards(gunObj[i].transform.rotation, lookAtEnemy, lookStep);
                if (attackTime >= attackSpeed && ableFire == true)
                {
                    FindObjectOfType<AudioManager>().Play("Shoot");
                    shootPE.Play();
                    GameObject bullet = SCR_Pool.GetFreeObject(playerBullet);
                    bullet.GetComponent<PlayerBulletScript>().Spawn(gunHead[i].transform.position, gunObj[i].transform.rotation);
                    if (gunObj.Length >= 2)
                    {
                        GameObject bullet1 = SCR_Pool.GetFreeObject(playerBullet);
                        bullet1.GetComponent<PlayerBulletScript>().Spawn(gunHead[i+1].transform.position, gunObj[i].transform.rotation);
                    }
                    
                    attackTime = 0;
                }
            }
          
           
           
        }
        else if (currentTarget == null)
        {
            ableFire = false;
        }



    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 16 && collision.gameObject.activeSelf == true)
        {
         
                this.gameObject.GetComponent<Rigidbody>().freezeRotation = false;

                StartCoroutine(cameraShake.Shake(.5f, .5f));
                playerHealth = playerHealth - 50f;
                
                
                if (collision.gameObject.GetComponent<Rigidbody>() != null)
                {
                    collision.gameObject.GetComponent<Rigidbody>().AddForce(5, 5, 5, ForceMode.Impulse);
                }
                else
                {
                    collision.gameObject.SetActive(false);
                }
                
                //gameObject.SetActive(false);

        }
        if (collision.gameObject.layer == 10)
        {
            playerHealth = playerHealth - 100f;
            
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            if (other.gameObject.activeSelf == true)
            {
                this.gameObject.GetComponent<Rigidbody>().freezeRotation = false;
                if (playerHealth > 0)
                {
                    StartCoroutine(cameraShake.Shake(.5f, .5f));
                }
                
                playerHealth = playerHealth - 50f;


                if (other.gameObject.GetComponent<Rigidbody>() != null)
                {
                    other.gameObject.GetComponent<Rigidbody>().AddForce(5, 5, 5, ForceMode.Impulse);
                }
                else
                {
                    other.gameObject.SetActive(false);
                }

                //gameObject.SetActive(false);
            }

        }
    }

}
