using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    float exploTime;
    float bulletExplosionTime;
    // Start is called before the first frame update
    void Start()
    {
      
        
    }
    public void Spawn(Vector3 position, Quaternion rotation)
    {
        if (this.gameObject.CompareTag("GiftBox"))
        {
            gameObject.GetComponent<GiftBox>().randomNum = Random.Range(0, 6);
        }
        transform.position = position;
        transform.rotation = rotation;
        if (this.gameObject.layer == 10)
        {
            if (gameObject.CompareTag("APC"))
            {
                gameObject.GetComponent<EnemyTargetScript>().health = 100f;
            } 
            if (gameObject.CompareTag("PoliceCar"))
            {
                gameObject.GetComponent<EnemyTargetScript>().health = 60f;
            }
            if (gameObject.CompareTag("Tank"))
            {
                gameObject.GetComponent<EnemyTargetScript>().health = 250f;
            }
        }
        if (this.gameObject.layer == 14)
        {
            gameObject.GetComponent<ParticleSystem>().Play();
            gameObject.GetComponentInChildren<ParticleSystem>().Play();
            exploTime += Time.deltaTime;
            if (exploTime > 0.6)
            {
                gameObject.SetActive(false);
            }
          
        }
        if (this.gameObject.CompareTag("BulletExplosion"))
        {
            bulletExplosionTime += Time.deltaTime;
            if (bulletExplosionTime >= 0.5f)
            {
                this.gameObject.SetActive(false);
            }
        }
       
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
