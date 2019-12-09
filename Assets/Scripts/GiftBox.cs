using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftBox : MonoBehaviour
{

    public int randomNum;
    public GameObject playerHolder;
    Rigidbody rb;
    public GameObject gameController;
    public GameObject mainText;
    public CameraShake cameraShake;
    public GameObject allEnemy;
    public GameObject forceField;
    public Animation anim;
    float textTime;
    // Start is called before the first frame update
    void Start()
    {
        randomNum = Random.Range(0, 6);

        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(13, 10);
        mainText = GameObject.FindGameObjectWithTag("Main text");
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
        allEnemy = GameObject.FindGameObjectWithTag("AllEnemy");
        anim = mainText.GetComponent<Animation>();
        forceField = GameObject.FindGameObjectWithTag("ForceField");
    }
    // Update is called once per frame
    void Update()
    {
        if (playerHolder != null)
        {
            transform.Rotate(0, 3, 0);
            if (Vector3.Distance(playerHolder.transform.position, transform.position) > 1)
            {
                Vector3 Direction = playerHolder.transform.position - transform.position;
                //   rb.AddForce(Direction.normalized * 25);
                rb.velocity = Direction.normalized * 25f;
            }
          
          
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHolder"))
        {
            if (randomNum == 0)
            {
                
                if (gameController.GetComponent<SceneController>().bulletChainStatus == true)
                {
                    gameController.GetComponent<SceneController>().bulletChainLevel += 1;
                    anim.Play();
                    mainText.GetComponent<Text>().text = "CHAIN +1";
                }
                if (gameController.GetComponent<SceneController>().bulletChainStatus != true)
                {
                    gameController.GetComponent<SceneController>().bulletChainStatus = true;
                    mainText.GetComponent<Text>().text = "CHAIN";
                }
            } else if (randomNum == 1)
            {
                if (gameController.GetComponent<SceneController>().bulletForkStatus == true)
                {
                    anim.Play();
                    playerHolder.GetComponent<PlayerController>().attackDamage = playerHolder.GetComponent<PlayerController>().attackDamage + 5f;
                    mainText.GetComponent<Text>().text = "ATTACK DAMAGE ++";
                }
                else if (gameController.GetComponent<SceneController>().bulletForkStatus == false)
                {
                    anim.Play();
                    gameController.GetComponent<SceneController>().bulletForkStatus = true;
                    mainText.GetComponent<Text>().text = "FORK";
                }
               
            } else if (randomNum == 2)
            {
                if (gameController.GetComponent<SceneController>().bulletPierceStatus == true)
                {
                    anim.Play();
                    playerHolder.GetComponent<PlayerController>().attackSpeed = playerHolder.GetComponent<PlayerController>().attackSpeed / 1.5f;
                    mainText.GetComponent<Text>().text = "ATTACK SPEED ++";
                } else if (gameController.GetComponent<SceneController>().bulletPierceStatus == false)
                {
                    anim.Play();
                    gameController.GetComponent<SceneController>().bulletPierceStatus = true;
                    mainText.GetComponent<Text>().text = "PIERCE";
                }
                
            } else if (randomNum == 3)
            {
                anim.Play();
                playerHolder.GetComponent<PlayerController>().attackSpeed = playerHolder.GetComponent<PlayerController>().attackSpeed / 1.2f;
                mainText.GetComponent<Text>().text = "ATTACK SPEED ++";
            } else if (randomNum == 4)
            {
                anim.Play();
                playerHolder.GetComponent<PlayerController>().attackDamage = playerHolder.GetComponent<PlayerController>().attackDamage + 5f;
                mainText.GetComponent<Text>().text = "ATTACK DAMAGE ++";
            } else if (randomNum == 5)
            {
                anim.Play();
                StartCoroutine(cameraShake.Shake(.20f, .5f));
                for (int i = 0; i <= allEnemy.transform.childCount -1; i++)
                {
                    allEnemy.transform.GetChild(i).GetComponent<EnemyTargetScript>().health = 0f;
                }
                
                mainText.GetComponent<Text>().text = "BOMB";
            } else if (randomNum == 6)
            {
                anim.Play();
                StartCoroutine(cameraShake.Shake(.20f, .5f));
                forceField.GetComponent<ForceField>().active = true;
                mainText.GetComponent<Text>().text = "FORCE FIELD";
            }
            textTime += Time.deltaTime;
            if (textTime >= 2)
            {
                mainText.GetComponent<Text>().text = "";
            }
            gameObject.SetActive(false);
        }
    }
}
