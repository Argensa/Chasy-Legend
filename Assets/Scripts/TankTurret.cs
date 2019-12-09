using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTurret : MonoBehaviour
{
    public GameObject tank;
    public GameObject playerHolder;
    float attackCount;
    float attackSpeed = 0.5f;
    public GameObject enemyBulletObj;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerHolder != null)
        {
            transform.position = new Vector3(tank.transform.position.x, tank.transform.position.y + 0.8f, tank.transform.position.z);
            transform.LookAt(new Vector3(playerHolder.transform.position.x, transform.position.y, playerHolder.transform.position.z));
            attackCount = attackCount + Time.deltaTime;
            if (Vector3.Distance(playerHolder.transform.position, transform.position) < 25)
            {
                if (attackCount > attackSpeed)
                {
                    GameObject enemyBullet = SCR_Pool.GetFreeObject(enemyBulletObj);
                    enemyBullet.GetComponent<EnemyBulletScript>().Spawn(transform.position, transform.rotation);
                    attackCount = 0f;
                }
            }
        }
       
        
    }
}
