using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    public GameObject mapBuilder;
    public GameObject playerHolder;
    Transform tile;
    // Start is called before the first frame update
    void Start()
    {
        mapBuilder = GameObject.FindGameObjectWithTag("MapBuilder");
        playerHolder = GameObject.FindGameObjectWithTag("PlayerHolder");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHolder != null)
        {
            if (Vector3.Distance(transform.position, playerHolder.transform.position) >= 200f)
            {
                Destroy(gameObject);
                //gameObject.SetActive(false);
            }
            else
            {
                //gameObject.SetActive(true);
            }
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHolder"))
        {
            if (mapBuilder != null && mapBuilder.GetComponent<MapBuilder>() != null)
            {
                mapBuilder.GetComponent<MapBuilder>().onTile = transform;
            }
          
        }
    }
}
