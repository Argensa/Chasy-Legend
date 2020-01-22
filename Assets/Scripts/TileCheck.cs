using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCheck : MonoBehaviour
{
    public float iAmA;
    public GameObject mapBuilder;
    // Start is called before the first frame update
    void Start()
    {
        mapBuilder = GameObject.FindGameObjectWithTag("MapBuilder");
    }

    // Update is called once per frame
 
}
