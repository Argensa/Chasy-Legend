using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public GameObject money;
    public GameObject playerHolder;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        money = GameObject.FindGameObjectWithTag("Dollar");
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerHolder.transform.position;
        //direction = new Vector3(money.transform.position.x - transform.position.x, transform.position.y, money.transform.position.z - transform.position.z);
        transform.LookAt(money.transform);
    }
}
