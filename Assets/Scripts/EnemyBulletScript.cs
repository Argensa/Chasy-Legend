using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    Rigidbody rb;
    public float bulletSpeed = 15f;
    float countTime;
    float liveTime = 6f;
    public GameObject bulletExplo;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreLayerCollision(11, 9);
        Physics.IgnoreLayerCollision(11, 8);
        Physics.IgnoreLayerCollision(11, 10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveForward();
        countTime = countTime + Time.deltaTime;
      
    }
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
    void MoveForward()
    {
        rb.velocity = transform.forward * bulletSpeed;
        if (countTime > liveTime) // if a bullet has been alive for too long, destroy it
        {
            gameObject.SetActive(false);
            // Destroy(gameObject);
            countTime = 0.0f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16 || other.gameObject.layer == 9)
        {
            GameObject bulletExplosion = SCR_Pool.GetFreeObject(bulletExplo);
            bulletExplosion.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
            gameObject.SetActive(false);
        } 

        if (other.gameObject.CompareTag("ForceField"))
        {
            GameObject bulletExplosion = SCR_Pool.GetFreeObject(bulletExplo);
            bulletExplosion.GetComponent<SpawnScript>().Spawn(transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }
}
