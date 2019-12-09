﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyTargetScript : MonoBehaviour
{
    public GameObject thisTarget;
    public float health = 100f;
    public GameObject playerHolder;
    public GameObject explosion;
    public GameObject gameController;
    public bool killAlready;
    public CameraShake cameraShake;
    bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();

    }

    // Update is called once per frame
    void Update()
    {
        if (playerHolder != null)
        {
            if (Vector3.Distance(playerHolder.transform.position, transform.position) >= 60f && isGrounded == true)
            {

                Physics.IgnoreLayerCollision(10, 16, true);
            }
            else if (Vector3.Distance(playerHolder.transform.position, transform.position) < 60f)
            {
                Physics.IgnoreLayerCollision(10, 16, false);
                if (health <= 0 && killAlready == false && playerHolder != null)
                {
                    // StartCoroutine(cameraShake.Shake(.15f, .4f));
                    playerHolder.GetComponent<PlayerController>().currentTarget = null;
                    GameObject explosionObj = SCR_Pool.GetFreeObject(explosion);
                    explosionObj.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
                    gameController.GetComponent<SceneController>().killCount += 1;
                    /*   for (int i = 0; i <= transform.childCount - 1; i++)
                       {
                           transform.GetChild(i).gameObject.SetActive(false);
                       } */

                    killAlready = true;
                    gameObject.SetActive(false);

                }
                else if (health > 0)
                {
                    killAlready = false;
                }
                else if (health <= 0 && killAlready == true)
                {
                    killAlready = false;
                }
            }
        }
        
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            health = health - playerHolder.GetComponent<PlayerController>().attackDamage;
            //other.gameObject.SetActive(false);
            StartCoroutine(cameraShake.Shake(.1f, .1f));
        }
        if (other.gameObject.layer == 16)
        {
            health = 0f;
            other.gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10 )
        {
            health = 0f;
        }
        if (collision.gameObject.layer == 16 )
        {
            health = 0f;
            collision.gameObject.SetActive(false);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            isGrounded = true;
        }
    }

}