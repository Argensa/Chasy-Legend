using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSphere : MonoBehaviour
{
    Vector3 targetScale;
    public GameObject playerHolder;
    float speed = 7f;
    public Vector3 originalScale;
    public bool amGrown = false;
    public float growCount;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        Spawn();
    }
    void Spawn()
    {
        targetScale = originalScale * playerHolder.GetComponent<PlayerController>().attackRange * 2f;
        growCount += Time.deltaTime;
        if (growCount >= .5f)
        {
            amGrown = true;
            growCount = 0f;
        }
       
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, speed * Time.deltaTime);
        if (playerHolder.GetComponent<PlayerController>().currentTarget == null && amGrown == true)
        {
            transform.localScale = originalScale;
            amGrown = false;
        }
        if (amGrown == false)
        {
            Spawn();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            playerHolder.GetComponent<PlayerController>().currentTarget = other.gameObject;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            //other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
            playerHolder.GetComponent<PlayerController>().currentTarget = null;
        }
    }
}
