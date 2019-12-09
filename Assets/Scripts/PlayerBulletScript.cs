using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    Rigidbody rb;
    public float bulletSpeed;
    float liveTime = 4f;
    float countTime = 0.0f;


    [Header("Spawn")]
    public GameObject gameController;

    [Header("Bullet Properties")]
    public bool pierce = false;
    public int targetPierce = 3;
    public bool chain = false;
    public int targetChain = 0;
    public bool fork = false;
    public bool poison = false;
    public float poisonDPS = 0;

    [Header("Fork Bullet")]
    //public GameObject[] forkBullet = new GameObject[2];
    Vector3 leftDir;
    Vector3 rightDir;
    public GameObject forkBulletObj;

    [Header("Chain Bullet")]
    public GameObject allEnemy;
    public float sumEnemy;
    public GameObject[] eachEnemy;
    public GameObject currentTarget = null;
   // public GameObject[] potentialTarget = new GameObject[100];
    public float maxRange;
    public float minRange = 1;
    public Vector3 chainDir;
    public List<Transform> enemyDistanceList = new List<Transform>();


    [Header("ExplodeBullet")]
    public GameObject bulletExplo;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        allEnemy = GameObject.FindGameObjectWithTag("AllEnemy");
        
    }
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        transform.position = position;
        transform.rotation = rotation;
        if (gameController.GetComponent<SceneController>().bulletChainStatus == true)
        {
            chain = true;
            targetChain = gameController.GetComponent<SceneController>().bulletChainLevel;
        }
        if (gameController.GetComponent<SceneController>().bulletForkStatus == true)
        {
            fork = true;
        }
        if (gameController.GetComponent<SceneController>().bulletPierceStatus == true)
        {
            pierce = true;
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
       
        MoveForward();
        leftDir = transform.forward + -transform.right;
        rightDir = transform.forward + transform.right;
        Debug.DrawRay(transform.position, leftDir, Color.cyan);
        Debug.DrawRay(transform.position, rightDir, Color.cyan);
        
        
    }
    void MoveForward()
    {
        rb.velocity = transform.forward * bulletSpeed; 
        countTime = countTime + Time.deltaTime;
        if (countTime > liveTime) // if a bullet has been alive for too long, destroy it
        {
            gameObject.SetActive(false);
           // Destroy(gameObject);
            countTime = 0.0f;
        }
    }   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 || other.gameObject.layer == 16 || other.gameObject.layer == 10)
        {
            //Destroy(gameObject);
            //bulletExplo.GetComponent<ParticleSystem>().Play();
            GameObject bulletExplosion = SCR_Pool.GetFreeObject(bulletExplo);
            bulletExplosion.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Targets"))
        {
            GameObject bulletExplosion = SCR_Pool.GetFreeObject(bulletExplo);
            bulletExplosion.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
           // bulletExplo.GetComponent<ParticleSystem>().Play();
            if (chain == true || fork == true || pierce == true)
            {
                Chain();
                Fork();
                Pierce();
                if (other.gameObject.GetComponent<ChainOrNot>() != null)
                {
                    other.gameObject.GetComponent<ChainOrNot>().chained = true;
                }
               
                
            }
            else if (chain == false && fork == false && pierce == false)
            {
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }

        }
    }
    void Pierce()
    {
        if (pierce == true && targetPierce >= 0)
        {
            targetPierce -= 1;
        }
        else if (targetPierce <= 0) //check if it can pierce the target
        {
            pierce = false;
        }
    }
    void Chain ()
    {
        if (chain == true)
        {
            TargetFunc();
            
        }
        
        if (chain == true && targetChain >= 0 && currentTarget != null)
        {
            rb.velocity = chainDir * 5;
            targetChain = targetChain - 1;
            
        } 
        if (targetChain <= 0 || currentTarget == null)
        {
            chain = false; 
        }
        if (currentTarget != null)
        {
            currentTarget = null;
        }
    }
    void Fork()
    {
        if (fork == true)
        {
            
            
            for (int i = 0; i <= 1; i++ )
            {
                GameObject forkBullet = SCR_Pool.GetFreeObject(forkBulletObj);
                forkBullet.GetComponent<ForkBulletScript>().Spawn(transform.position, transform.rotation);
                // Instantiate(forkBullet[i], transform.position, transform.rotation);
                forkBullet.GetComponent<ForkBulletScript>().forkNum = i;

            }
            gameObject.SetActive(false);
           // Destroy(gameObject);
        }
    }
    void TargetFunc()
    {

        //allEnemy.GetComponentsInChildren<Transform>();
        int indexNumber;
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
        foreach (Transform enemy in allEnemy.transform)
        {
                
                    //if the enemy that the foreach loop check is near the player
                    if (Vector3.Distance(enemy.position, transform.position) <= maxRange && Vector3.Distance(enemy.position, transform.position) >= minRange)
                    {
                        //Debug.Log("Nho hon maxRange");
                        if (currentTarget == null && enemy.gameObject.GetComponent<ChainOrNot>().chained == false && enemy.gameObject.activeSelf == true) //if you currently have no target, make it a target
                        {

                            currentTarget = enemy.gameObject;
                            chainDir = enemy.gameObject.transform.position - transform.position;

                        }
                        if (currentTarget != null && currentTarget.activeSelf == true) //if you already have a target then look at that target
                        {
                            transform.LookAt(currentTarget.transform);

                        }
                        //if target ==null -> attack 
                        //This results in the first enemy that enter your range being attacked
                        //if he dies because the code is in update it will choose another enemy to attack, though not necessarily the closest one
                        //At least now we know where the enemies are in the editor 

                    }
                
            
           
            if (currentTarget != null)
            {
                if (Vector3.Distance(currentTarget.transform.position, transform.position) >= maxRange)
                {
                    //if too far then stop looking/targeting it
                    currentTarget = null;
                }
            }

        }
    }
}
