using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainOrNot : MonoBehaviour
{
    public bool chained = false;
    public float resetSpeed;
    public float resetCount;
    public GameObject playerHolder;
    // Start is called before the first frame update
    void Start()
    {
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
        if (playerHolder != null)
        {
            resetSpeed = playerHolder.GetComponent<PlayerController>().attackSpeed / 1.5f;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (chained == true)
        {
            resetCount = resetCount + Time.deltaTime;
            if (resetCount > resetSpeed)
            {
                
                chained = false;
                resetCount = 0;
            }
        }
       
    }
}
