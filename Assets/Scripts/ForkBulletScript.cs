using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkBulletScript : MonoBehaviour
{
    Rigidbody rb;
    public float bulletSpeed;
    float liveTime = 6f;
    float countTime = 0.0f;
    Vector3 leftDir;
    Vector3 rightDir;
    public float forkNum;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
       
    }
    public void Spawn (Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        leftDir = 2 * transform.forward + -transform.right;
        rightDir = 2 * transform.forward + transform.right;
    }
    // Update is called once per frame
    void Update()
    {
        MoveForward();
     //   Debug.DrawRay(transform.position, leftDir, Color.cyan);
     //   Debug.DrawRay(transform.position, rightDir, Color.cyan);
    }
    void MoveForward()
    {
        if (forkNum == 0)
        {
            rb.velocity = leftDir * 10;
        }
        if (forkNum == 1)
        {
            rb.velocity = rightDir * 10;
        }
        countTime = countTime + Time.deltaTime;
        if (countTime > liveTime) // if a bullet has been alive for too long, destroy it
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
            countTime = 0.0f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            gameObject.SetActive(false);
        }
    }
}
